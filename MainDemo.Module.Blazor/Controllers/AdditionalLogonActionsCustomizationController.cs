using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.SystemModule;

namespace MainDemo.Module.Blazor.Controllers {
    public class AdditionalLogonActionsCustomizationController : WindowController {
        protected override void OnActivated() {
            base.OnActivated();
            AdditionalLogonActionsController additionalLogonActionsController = Frame.GetController<AdditionalLogonActionsController>();
            if(additionalLogonActionsController != null) {
                var action = additionalLogonActionsController.Actions.Where(action => action.Id == "AzureAD").FirstOrDefault();
                if(action != null) {
                    action.Caption = "Azure";
                }
            }
        }
    }
}
