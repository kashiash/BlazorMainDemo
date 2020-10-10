using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace MainDemo.Blazor.ServerSide.Services {
    internal class WindowsSignInMiddleware {
        private readonly RequestDelegate next;
        public WindowsSignInMiddleware(RequestDelegate next) {
            this.next = next;
        }
        public async Task Invoke(HttpContext context) {
            string requestPath = context.Request.Path.Value.TrimStart('/');
            string returnUrl = context.Request.Query["ReturnUrl"];
            string schemeName = context.Request.Query["schemeName"];

            if(String.IsNullOrEmpty(returnUrl)) {
                returnUrl = "/";
            }
            if(requestPath.StartsWith("api/challenge") && schemeName == "Windows") {
                var user = context.User;
                if(user is WindowsPrincipal wPrincipal) {
                    var props = new AuthenticationProperties() {
                        RedirectUri = returnUrl,
                        Items = {
                        { "returnUrl", returnUrl },
                        { "scheme", "Windows" },
                    }
                    };

                    var id = new ClaimsIdentity("Windows");
                    id.AddClaim(new Claim(ClaimTypes.Name, wPrincipal.Identity.Name));
                    id.AddClaim(new Claim(ClaimTypes.NameIdentifier, wPrincipal.Identity.Name));
                    await context.SignInAsync(new ClaimsPrincipal(id), props);
                    context.Response.Redirect(returnUrl);
                }
                else {
                    await context.ChallengeAsync("Windows");
                }
            }
            else {
                await next(context);
            }
        }
    }
}
