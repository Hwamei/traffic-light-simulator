using TrafficLightSimulator.Server.Hubs;
using TrafficLightSimulator.Server.Models;
using TrafficLightSimulator.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyMethod().AllowAnyHeader()
           .WithOrigins("https://localhost:44431")
           .WithOrigins("http://localhost:4200")
           .AllowCredentials();
}));

builder.Services.AddSignalR();
builder.Services.AddSingleton<ITrafficLightService, TrafficLightService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandler("/Error");

app.UseCors("CorsPolicy");

app.UseRouting();

app.MapHub<TrafficLightHub>("/hub");

app.Run();