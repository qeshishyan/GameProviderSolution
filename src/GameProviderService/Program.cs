using GameProviderService.DAL.Extensions;
using GameProviderService.Service.Extensions;
using Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7189); // Listen on port 80 from any IP address.
});

// Add services to the container.
builder.Services.AddClients();
builder.Services.AddServices();
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

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

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
