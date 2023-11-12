using System.Text;

namespace GameProviderService.Service.Services
{
    public class SignatureService : ISignatureService
    { 
        public string GenerateSign(IDictionary<string, string> parameters, string secretKey)
        {
            // Concatenate parameter values in alphabetical order of keys
            var concatenatedParams = string.Join("", parameters.OrderBy(p => p.Key).Select(p => p.Value));

            // Append the secret key (salt)
            concatenatedParams += secretKey;

            // Compute SHA1 hash
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
            {
                byte[] computedHash = sha1.ComputeHash(Encoding.UTF8.GetBytes(concatenatedParams));
                return BitConverter.ToString(computedHash).Replace("-", "").ToUpper();
            }
        }
    }
}
