using APIGateway.Models;
using Newtonsoft.Json;
using System.Net;

namespace APIGateway.Middlewares
{
    public class ExceptionHandlingDelegatingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
                var standardResponse = new ResponseModel
                {
                    StatusCode = (int)response.StatusCode,
                    Message = response.StatusCode.ToString()
                };
                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    object? data = JsonConvert.DeserializeObject(content);
                    standardResponse.Data = data;
                }
                var standardContent = JsonConvert.SerializeObject(standardResponse);
                response.Content = new StringContent(standardContent, System.Text.Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                var standardContent = JsonConvert.SerializeObject(new ResponseModel
                {
                    StatusCode = StatusCodes.Status502BadGateway,
                    Message = ex.Message
                });
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(standardContent, System.Text.Encoding.UTF8, "application/json")
                };
            }
        }
    }
}
