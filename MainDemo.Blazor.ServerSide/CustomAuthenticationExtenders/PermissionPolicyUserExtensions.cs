using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using MainDemo.Module.BusinessObjects;

namespace MainDemo.Blazor.ServerSide.CustomAuthenticationExtenders {
    public static class PermissionPolicyUserExtensions {
        public static bool IsAuthenticationStandardEnabled(this PermissionPolicyUser user, IObjectSpace os) {
            if(os == null) {
                return false;
            }

            var userLoginInfo = os.FindObject<UserLoginInfo>(CriteriaOperator.And(
                new BinaryOperator(nameof(UserLoginInfo.LoginProviderName), SignInMiddlewareDefaults.DefaultClaimsIssuer),
                new BinaryOperator("User.Oid", user.Oid)
            ));

            if(userLoginInfo != null) {
                return true;
            }
            else {
                if(os.GetObjectsCount(typeof(UserLoginInfo), new BinaryOperator("User.Oid", user.Oid)) == 0) {
                    user.CreateUserLoginInfo(os, SignInMiddlewareDefaults.DefaultClaimsIssuer, null);
                    return true;
                }
            }
            return false;
        }
        public static void CreateUserLoginInfo(this PermissionPolicyUser user, IObjectSpace os, string providerName, string providerUserKey) {
            var userLoginInfo = os.CreateObject<UserLoginInfo>();
            userLoginInfo.ProviderUserKey = providerUserKey;
            userLoginInfo.LoginProviderName = providerName;
            userLoginInfo.User = user;
            os.CommitChanges();
        }
    }
}
