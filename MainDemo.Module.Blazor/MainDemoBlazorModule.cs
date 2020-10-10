using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Blazor.Internal.Compatibility.System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using MainDemo.Module.Blazor.Controllers;
using MainDemo.Module.BusinessObjects;

namespace MainDemo.Module.Blazor {
    public partial class MainDemoBlazorModule : ModuleBase {
        public MainDemoBlazorModule() {
            InitializeComponent();
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            application.CreateCustomLogonWindowControllers += Application_CreateCustomLogonWindowControllers;
        }

        private void Application_CreateCustomLogonWindowControllers(object sender, CreateCustomLogonWindowControllersEventArgs e) {
            e.Controllers.Add(Application.CreateController<LogonParametersViewController>());
        }

        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
            ITypeInfo userTypeInfo = typesInfo.FindTypeInfo(typeof(PermissionPolicyUser));
            IMemberInfo userLoginsProperty = userTypeInfo.FindMember("UserLogins");
            if (userLoginsProperty == null) {
                userLoginsProperty = userTypeInfo.CreateMember("UserLogins", typeof(XPCollection<UserLoginInfo>));
                userLoginsProperty.AddAttribute(new DevExpress.Xpo.AssociationAttribute(typeof(UserLoginInfo)), true);
                userLoginsProperty.AddAttribute(new DevExpress.Xpo.AggregatedAttribute(), true);
                userLoginsProperty.AddAttribute(new VisibleInDetailViewAttribute(false), true);
                ITypeInfo userLoginTypeInfo = typesInfo.FindTypeInfo(typeof(UserLoginInfo));
                userLoginTypeInfo.FindMember("User").AddAttribute(new DevExpress.Xpo.AssociationAttribute(typeof(PermissionPolicyUser)), false);
                ((XafMemberInfo)userLoginsProperty).Refresh();
            }
        }
    }
}
