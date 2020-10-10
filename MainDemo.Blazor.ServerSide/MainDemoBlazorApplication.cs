using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using MainDemo.Blazor.ServerSide.Services;
using MainDemo.Module.Blazor.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace MainDemo.Blazor.ServerSide {
    public partial class MainDemoBlazorApplication : BlazorApplication {
        class EmptySettingsStorage : SettingsStorage {
            public override string LoadOption(string optionPath, string optionName) => null;
            public override void SaveOption(string optionPath, string optionName, string optionValue) { }
        }

#region Default XAF configuration options (https://www.devexpress.com/kb=T501418)
        static MainDemoBlazorApplication() {
            DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
            DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;
        }
#endregion

        public MainDemoBlazorApplication() {
            InitializeComponent();
            AboutInfo.Instance.Version = "Version " + AssemblyInfo.FileVersion;
            AboutInfo.Instance.Copyright = AssemblyInfo.AssemblyCopyright + " All Rights Reserved";
        }
        protected override void OnSetupStarted() {
            base.OnSetupStarted();

            IConfiguration configuration = ServiceProvider.GetRequiredService<IConfiguration>();
            if(configuration.GetConnectionString("ConnectionString") != null) {
                ConnectionString = configuration.GetConnectionString("ConnectionString");
            }

#if DEBUG
            if(System.Diagnostics.Debugger.IsAttached && CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
        }
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            IXpoDataStoreProvider dataStoreProvider = GetDataStoreProvider(args.ConnectionString, args.Connection);
            args.ObjectSpaceProviders.Add(new SecuredObjectSpaceProvider((ISelectDataSecurityProvider)Security, dataStoreProvider, true));
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
        }
        private IXpoDataStoreProvider GetDataStoreProvider(string connectionString, System.Data.IDbConnection connection) {
            XpoDataStoreProviderAccessor accessor = ServiceProvider.GetRequiredService<XpoDataStoreProviderAccessor>();
            lock(accessor) {
                if(accessor.DataStoreProvider == null) {
                    accessor.DataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, connection, true);
                }
            }
            return accessor.DataStoreProvider;
        }
        private void MainDemoBlazorApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e) {
            e.Updater.Update();
            e.Handled = true;
        }
        protected override SettingsStorage CreateLogonParameterStoreCore() {
            return new EmptySettingsStorage();
        }
        private void MainDemoBlazorApplication_LastLogonParametersRead(object sender, LastLogonParametersReadEventArgs e) {
            if(e.LogonObject is AuthenticationStandardLogonParameters logonParameters && string.IsNullOrEmpty(logonParameters.UserName)) {
                logonParameters.UserName = "Sam";
            }
        }
        protected override List<Controller> CreateLogonWindowControllers() {
            var result = base.CreateLogonWindowControllers();
            result.Add(new AdditionalLogonActionsCustomizationController());
            return result;
        }
    }
}
