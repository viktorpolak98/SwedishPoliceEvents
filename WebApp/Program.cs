using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using WebApp.Repositories;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpClient<IReadData<JsonDocument>, PoliceAPICaller>(client =>
{
    client.BaseAddress = new Uri("https://polisen.se/api");
}).SetHandlerLifetime(TimeSpan.FromMinutes(15));

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
});

app.Run();