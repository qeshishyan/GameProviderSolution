using CrashGameService.DAL.Extensions;
using CrashGameService.Repository.Context;
using CrashGameService.Service.Extensions;
using CrashGameService.Service.Hubs;
using Microsoft.EntityFrameworkCore;
using Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7021);
});
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices();
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddClients(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        //.WithOrigins("http://crashGame-react-app-url.com")
        .WithOrigins("http://localhost:7021", "null")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});
builder.Services.AddSignalR();

var app = builder.Build();

#if DEBUG
#else
using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CrashDbContext>();
    context.Database.Migrate();
}
#endif


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseRouting();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("CorsPolicy");

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<GameHub>("/gamehub");
    endpoints.MapControllers();
});

app.Run();
