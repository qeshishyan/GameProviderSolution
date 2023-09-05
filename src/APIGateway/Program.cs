using APIGateway.Middlewares;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .AddJsonFile("configuration.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// Add services to the container.
builder.Services.AddOcelot(configuration)
    .AddDelegatingHandler<ExceptionHandlingDelegatingHandler>();

builder.WebHost.UseKestrel();
builder.WebHost.UseIISIntegration();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7001); // Listen on port 80 from any IP address.
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseOcelot().Wait();

app.Run();