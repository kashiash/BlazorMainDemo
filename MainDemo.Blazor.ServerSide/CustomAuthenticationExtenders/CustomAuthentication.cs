using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;

namespace MainDemo.Blazor.ServerSide.CustomAuthenticationExtenders {
    public class CustomAuthentication : AuthenticationStandard {
        public CustomAuthentication(Type userType, Type logonParametersType) :
            base(userType, logonParametersType) {
        }

        public override object Authenticate(IObjectSpace objectSpace) {
            PermissionPolicyUser result = (PermissionPolicyUser)base.Authenticate(objectSpace);
            if(result.IsAuthenticationStandardEnabled(objectSpace)) {
                return result;
            }
            throw new AuthenticationException(result.UserName);
        }
    }
}
