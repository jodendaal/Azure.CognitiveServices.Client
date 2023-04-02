using Azure.CognitiveService.Client.BlazorApp.Models;
using Azure.CognitiveServices.Client;
using Azure.CognitiveServices.Client.OpenAI.Models;
using BlazorStrap;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;
using System.Security.Claims;

namespace Azure.CognitiveService.Client.BlazorApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add Azure OpenAI
            builder.Services
               .AddAzureOpenAIHttpService(httpClientOptions => GetRetryPolicy())
               .AddAzureOpenAITextCompletion()
               .AddAzureOpenAITextEmbeddings()
               .AddAzureOpenAIChatCompletion();

            //Add Azure OpenAI Config
            builder.Services.Configure<AzureOpenAIConfiguration>(builder.Configuration.GetSection("OpenAI"));

            builder.Services.AddOptions<AzureOpenAIConfig>("textCompletion").Configure<IOptions<AzureOpenAIConfiguration>>((o, e) =>
            {
                o.ApiVersion = e.Value.TextCompletion.ApiVersion;
                o.ApiKey = e.Value.TextCompletion.ApiKey;
                o.ApiUrl = e.Value.TextCompletion.ApiUrl;
                o.DeploymentName = e.Value.TextCompletion.DeploymentName;
            });

            builder.Services.AddOptions<AzureOpenAIConfig>("textEmbeddings").Configure<IOptions<AzureOpenAIConfiguration>>((o, e) =>
            {
                o.ApiVersion = e.Value.Embeddings.ApiVersion;
                o.ApiKey = e.Value.Embeddings.ApiKey;
                o.ApiUrl = e.Value.Embeddings.ApiUrl;
                o.DeploymentName = e.Value.Embeddings.DeploymentName;
            });

            builder.Services.AddOptions<AzureOpenAIConfig>("chat").Configure<IOptions<AzureOpenAIConfiguration>>((o, e) =>
            {
                o.ApiVersion = e.Value.Chat.ApiVersion;
                o.ApiKey = e.Value.Chat.ApiKey;
                o.ApiUrl = e.Value.Chat.ApiUrl;
                o.DeploymentName = e.Value.Chat.DeploymentName;
            });

            //Authentication
            builder.Services
                .AddAuthentication()
                .AddOpenIdConnect("oidc", "Identity Server", options =>
            {
                options.Prompt = builder.Configuration["IdentityServer:Prompt"].ToString();
                options.Authority = builder.Configuration["IdentityServer:Url"].ToString();
                options.ClientId = builder.Configuration["IdentityServer:ClientId"].ToString();
                options.ClientSecret = builder.Configuration["IdentityServer:Secret"].ToString();
                options.ResponseType = "code";
                options.ResponseMode = "query";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("email");
                options.Scope.Add("profile");

                options.Scope.Add("offline_access");
                options.ClaimActions.MapJsonKey(claimType: ClaimTypes.Email, jsonKey: "email");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            }
        );

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.Unspecified;
            });

            builder.Services.AddAntiforgery(opts => {
                opts.Cookie.SameSite = SameSiteMode.Unspecified;
            });


            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            builder.Services.AddBlazorStrap();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }


        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }
    }
}