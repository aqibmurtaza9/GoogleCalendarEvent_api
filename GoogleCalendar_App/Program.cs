using Google.Apis.Auth.AspNetCore3;
using GoogleCalendar_App.IService;
using GoogleCalendar_App.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddRazorPages();
builder.Services.AddScoped<IGoogleCalendarService, GoogleCalendarService>();
builder.Services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
// Add Google authentication
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//})
//.AddCookie()
//.AddGoogle(options =>
//{
//    options.ClientId = "798723233176-5s05agfpq2p5jamqs2rglkuu444ohfef.apps.googleusercontent.com";
//    options.ClientSecret = "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";
//});

builder.Services
       .AddAuthentication(o =>
       {
           // This forces challenge results to be handled by Google OpenID Handler, so there's no
           // need to add an AccountController that emits challenges for Login.
           o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
           // This forces forbid results to be handled by Google OpenID Handler, which checks if
           // extra scopes are required and does automatic incremental auth.
           o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
           // Default scheme that will handle everything else.
           // Once a user is authenticated, the OAuth2 token info is stored in cookies.
           o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
       })
       .AddCookie()
       .AddGoogleOpenIdConnect(options =>
       {
           options.ClientId = "798723233176-5s05agfpq2p5jamqs2rglkuu444ohfef.apps.googleusercontent.com";
           options.ClientSecret = "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";
       });

IdentityModelEventSource.ShowPII = true;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
