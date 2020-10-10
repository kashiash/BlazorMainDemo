using System;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.Persistent.Base;
using MainDemo.Blazor.ServerSide.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using DevExpress.ExpressApp.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ReportsV2.Blazor;
using DevExpress.Blazor.Reporting;
using System.Security.Claims;
using MainDemo.Module.BusinessObjects;
using System.Security.Principal;
using DevExpress.ExpressApp;
using MainDemo.Blazor.ServerSide.CustomAuthenticationExtenders;

namespace MainDemo.Blazor.ServerSide {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpContextAccessor();
            services.AddSingleton<XpoDataStoreProviderAccessor>();
            services.AddScoped<CircuitHandler, CircuitHandlerProxy>();
            services.AddXaf<MainDemoBlazorApplication>(Configuration);
            services.AddXafReporting();

            services.AddXafSecurity(options => {
                options.RoleType = typeof(PermissionPolicyRole);
                options.UserType = typeof(PermissionPolicyUser);
                options.Events.OnSecurityStrategyCreated = securityStrategy => ((SecurityStrategy)securityStrategy).RegisterXPOAdapterProviders();
            })
                .AddExternalAuthentication<HttpContextPrincipalProvider>(
                    options => {
                        options.Events.Authenticate = (objectSpace, externalUser) => {
                            bool autoCreateUserByExternalProviderInfo = true;
                            switch(externalUser.Identity.AuthenticationType) {
                                case SignInMiddlewareDefaults.DefaultClaimsIssuer:
                                    return ProcessStandartLogin(objectSpace, externalUser);

                                default:
                                    return ProcessExternalLogin(objectSpace, externalUser, autoCreateUserByExternalProviderInfo);
                            }

                            PermissionPolicyUser ProcessStandartLogin(IObjectSpace os, IPrincipal _externalUser) {
                                var user = objectSpace.FindObject<PermissionPolicyUser>(new BinaryOperator(nameof(PermissionPolicyUser.UserName), externalUser.Identity.Name));
                                if(user != null && user.IsAuthenticationStandardEnabled(os)) {
                                    return user;
                                }
                                return null;
                            }

                            PermissionPolicyUser ProcessExternalLogin(IObjectSpace os, IPrincipal _externalUser, bool autoCreateUser) {
                                var userIdClaim = ((ClaimsPrincipal)_externalUser).FindFirst("sub") ??
                                 ((ClaimsPrincipal)_externalUser).FindFirst(ClaimTypes.NameIdentifier) ??
                                 throw new Exception("Unknown user id");

                                var providerUserId = userIdClaim.Value;
                                var userLoginInfo = os.FindObject<UserLoginInfo>(CriteriaOperator.And(
                                        new BinaryOperator(nameof(UserLoginInfo.LoginProviderName), _externalUser.Identity.AuthenticationType),
                                        new BinaryOperator(nameof(UserLoginInfo.ProviderUserKey), providerUserId)
                                    ));
                                if(userLoginInfo != null) {
                                    return userLoginInfo.User;
                                }
                                else {
                                    if(autoCreateUser) {
                                        var user = CreatePermissionPolicyUser(os, _externalUser);
                                        if(user != null) {
                                            user.CreateUserLoginInfo(os, _externalUser.Identity.AuthenticationType, providerUserId);
                                        }
                                        return user;
                                    }
                                }
                                return null;
                            }

                            PermissionPolicyUser CreatePermissionPolicyUser(IObjectSpace os, IPrincipal _externalUser) {
                                string userName = _externalUser.Identity.Name;
                                var user = os.CreateObject<PermissionPolicyUser>();
                                user.UserName = _externalUser.Identity.Name;

                                //TODO description -- For backward compatibility
                                user.SetPassword(Guid.NewGuid().ToString());

                                user.Roles.Add(os.FindObject<PermissionPolicyRole>(new BinaryOperator(nameof(PermissionPolicyRole.Name), "Users")));
                                os.CommitChanges();
                                return user;
                            }
                        };
                    })
                .AddAuthenticationProvider<AuthenticationStandardProviderOptions, CustomAuthenticationProvider>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = "/XafLoginPage")
                .AddGoogle(options => {
                    Configuration.Bind("Authentication:Google", options);
                    options.AuthorizationEndpoint += "?prompt=consent";
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                //App registration https://portal.azure.com/#blade/Microsoft_AAD_RegisteredApps/ApplicationsListBlade
                .AddAzureAD(options => {
                    Configuration.Bind("Authentication:AzureAd", options);
                    options.CookieSchemeName = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddOAuth("GitHub", "GitHub", options => {
                    Configuration.Bind("Authentication:GitHub", options);
                    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");

                    //options.ClaimActions.MapJsonKey("urn:github:name", "name");
                    //options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email", ClaimValueTypes.Email);
                    //options.ClaimActions.MapJsonKey("urn:github:url", "url");

                    options.Events = new OAuthEvents {
                        OnCreatingTicket = async context => {
                            // Get the GitHub user
                            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                            response.EnsureSuccessStatusCode();
                            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                            context.RunClaimActions(json.RootElement);
                        }
                    };
                });

            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options => {
                options.Authority = options.Authority + "/v2.0/"; // Microsoft identity platform
                options.TokenValidationParameters = new TokenValidationParameters {
                    // This claim is in the Azure AD B2C token; this code tells the web app to "absorb" the token "name" and place it in the user object
                    NameClaimType = "preferred_username"
                };
                options.TokenValidationParameters.ValidateIssuer = false; // accept several tenants (here simplified)
            });

            services.Configure<IISServerOptions>(options => {
                options.AuthenticationDisplayName = "Windows";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseMiddleware<WindowsSignInMiddleware>();
            app.UseXaf();
            app.UseDevExpressBlazorReporting();
            app.UseEndpoints(endpoints => {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapControllers();
            });
        }
    }
}
