using Microsoft.OpenApi.Models;
using User.Processing.API;
using User.Processing.API.Middleware;
using User.Processing.Service;
using User.Processing.Service.Interface;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Processing API", Version = "v1" });
});

//Register services
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddScoped<IHttpClientFactoryService, HttpClientFactoryService>();
builder.Services.AddScoped<IExternalUserService, ExternalUserService>();

var app = builder.Build();
app.MapGetUserEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Processing API v1");
        c.RoutePrefix = string.Empty;
    });
}
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.Run();
