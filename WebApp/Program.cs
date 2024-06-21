using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json;
using WebApp.Models.PoliceEvent;
using WebApp.Models.PoliceStation;
using WebApp.Repositories;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IReadData<JsonDocument>, PoliceAPICaller>(client =>
{
    client.BaseAddress = new Uri("https://polisen.se/api/");
}).SetHandlerLifetime(TimeSpan.FromMinutes(15));

builder.Services.AddSingleton<IRepository<PoliceEvent>, PoliceEventsRepository>();
builder.Services.AddSingleton<IRepository<PoliceStation>, PoliceStationsRepository>();
builder.Services.AddControllers();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.UseStaticFiles();

app.Run();