using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace CarsDb.Module.BusinessObjects
{
    [XafDisplayName("Modele")]
    [NavigationItem("Słowniki")]
    [DefaultClassOptions]
    [System.ComponentModel.DefaultProperty("name")]
    public partial class car_model
    {
        public car_model(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }


        [Action(ToolTip = "Ustaw model jako archiwalny", ConfirmationMessage = "Czy chcesz ustawić model jako archiwalny? Nie będzie on wtedy dostepny na liście wyboru w oknie pojazdu.")]
        public void Archiwizuj()
        {

            Archiwalny = true;
        }

        [Action(ToolTip = "Ustaw model jako aktualny",Caption ="Aktualny")]
        public void UstawAktywnego()
        {

            Archiwalny = false;
        }
    }

}
