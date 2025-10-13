using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using OutlookBlazorTestApp2.Components;
using OutlookBlazorTestApp2.services;


var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Add Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ✅ Register CacheService here
builder.Services.AddSingleton<CacheService>();
builder.Services.AddScoped<RoleService>(); // 👈 Add this her

// 2️⃣ Add Microsoft Identity Authentication
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()  // Optional: if you call Microsoft Graph or other APIs
    .AddInMemoryTokenCaches();

// 3️⃣ Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

// 4️⃣ Build app
var app = builder.Build();

// 5️⃣ Configure the middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// 6️⃣ Map components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
