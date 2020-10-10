using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Utils.Reflection;
using Microsoft.Extensions.Options;

namespace MainDemo.Blazor.ServerSide.CustomAuthenticationExtenders {
    public class CustomAuthenticationProvider : AuthenticationStandardProviderV2 {
        public CustomAuthenticationProvider(IOptions<AuthenticationStandardProviderOptions> options, IOptions<SecurityOptions> securityOptions) :
            base(options, securityOptions){
        }

        protected override AuthenticationBase CreateAuthentication(Type userType, Type logonParametersType) {
            return new CustomAuthentication(userType, logonParametersType);
        }
    }
}
