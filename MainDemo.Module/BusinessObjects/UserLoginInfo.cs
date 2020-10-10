using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;

namespace MainDemo.Module.BusinessObjects {
    //TODO The probably rename. This is the same name as Microsoft.AspNetCore.Identity.UserLoginInfo
    [System.ComponentModel.Browsable(false)]
    public class UserLoginInfo : BaseObject {
        public UserLoginInfo(Session session) : base(session) { }

        private string loginProviderName;
        public string LoginProviderName {
            get { return loginProviderName; }
            set { SetPropertyValue(nameof(LoginProviderName), ref loginProviderName, value); }
        }

        private string providerUserKey;
        public string ProviderUserKey {
            get { return providerUserKey; }
            set { SetPropertyValue(nameof(ProviderUserKey), ref providerUserKey, value); }
        }

        private PermissionPolicyUser user;
        public PermissionPolicyUser User {
            get { return user; }
            set { SetPropertyValue(nameof(User), ref user, value); }
        }
    }
}
