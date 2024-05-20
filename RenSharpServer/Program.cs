using DAL.Repositories;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RenSharpServer.Hubs;
using RenSharpServer.Services;
using RenSharpServer.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMvc().AddNewtonsoftJson();

builder.Services.AddTransient<IDialogRepository, DialogRepository>();
builder.Services.AddTransient<IGptService, GptService>();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options => //CookieAuthenticationOptions
				{
					options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
				});

builder.Services.AddSignalR();

string? connection = builder.Configuration["DB_CONNECTION_STRING"];
if (connection == null)
    throw new Exception("Connection string is null. Check environment variable 'DB_CONNECTION_STRING'");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connection));

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();

}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "swagger";
});

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<EventsHub>("/signalr", options =>
{
	options.Transports =
		HttpTransportType.WebSockets;
});
app.MapControllerRoute("default", "{controller=Home}/{action=Index}");




app.Run();
