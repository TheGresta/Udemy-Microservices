using FreeCourse.Gateway.DelegateHandlers;
using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationSheme", options =>
{
    options.Authority = builder.Configuration["IdentityServerURL"];
    options.Audience = "resource_gateway";
    options.RequireHttpsMetadata = false;
});

builder.Services.AddHttpClient<TokenExchangeDelegateHandler>();

builder.Services.AddOcelot(builder.Configuration).AddDelegatingHandler<TokenExchangeDelegateHandler>();

builder.Configuration.AddJsonFile($"configuration.{builder.Environment.EnvironmentName.ToLower()}.json").AddEnvironmentVariables();

var app = builder.Build();

await app.UseOcelot();

app.Run();
