using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.Win32;
using OutlookBlazorTestApp2.Components;
using OutlookBlazorTestApp2.Middleware;
using OutlookBlazorTestApp2.services;
using OutlookBlazorTestApp2.Services;


var builder = WebApplication.CreateBuilder(args);

// Add Microsoft Identity (Azure AD)
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

// Cookie auth for local
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

// Blazor Server services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler(); // hook for MSAL consent

// Our app services
builder.Services.AddHttpContextAccessor();

//Register custom services
builder.Services.AddSingleton<RoleService>();
builder.Services.AddScoped<AuthStateService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthStateService>();
builder.Services.AddScoped<CrmService>();
builder.Services.AddScoped<IAutoLinkService, AutoLinkService>();// stub service (replace with real)
builder.Services.AddScoped<EmailService>(); // stub service

// Claims transformation -> map groups/claims to roles (demo)
builder.Services.AddScoped<IClaimsTransformation, DemoClaimsTransformer>();

// Error handling middleware
builder.Services.AddTransient<ErrorhandlingMiddleware>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

// Authentication/Authorization
app.UseAuthentication();
app.UseAuthorization();

// Add middleware if you want global error logging
app.UseMiddleware<OutlookBlazorTestApp2.Middleware.ErrorhandlingMiddleware>();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
