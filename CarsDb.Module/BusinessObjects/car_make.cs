using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace CarsDb.Module.BusinessObjects
{
    [XafDisplayName("Producenci")]
    [NavigationItem("Słowniki")]
    [DefaultClassOptions]
    [System.ComponentModel.DefaultProperty("name")]
    public partial class car_make
    {
        public car_make(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        [Action(ToolTip = "Ustaw producenta jako archiwalnego",ConfirmationMessage = "Czy chcesz ustawić producenta jako archiwalnego? Nie będzie on wtedy dostepny na liście wyboru w oknie pojazdu.")]

        public void Archiwizuj()
        {

            Archiwalny = true;
        }

        [Action(ToolTip = "Ustaw producenta jako aktualnego (niearchiwalnego)",Caption ="Aktualny")]
        public void UstawAktywnego()
        {

            Archiwalny = false;
        }
    }

}
