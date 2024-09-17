using BlazorDownloadFile;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UmojaCampus.Client;
using UmojaCampus.Client.AuthProvider;
using UmojaCampus.Client.Helpers;
using UmojaCampus.Client.Services.Contracts;
using UmojaCampus.Client.Services.Implementations;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<CustomHttpHandler>();
builder.Services.AddHttpClient("UmojaCampus", client =>
{
    client.BaseAddress = new Uri("https://localhost:7125/");
}).AddHttpMessageHandler<CustomHttpHandler>();

builder.Services.AddBlazorDownloadFile();
//builder.Services.AddScoped(sp => new HttpClient 
//{ 
//    //BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
//    BaseAddress = new Uri("https://localhost:7125/")
//});

// Register JsInterop as a singleton service
builder.Services.AddScoped<JsInterop>();

//Register Services
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<GetHttpClient>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();

await builder.Build().RunAsync();
