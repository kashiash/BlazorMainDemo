using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace CarsDb.Module.BusinessObjects
{
    [XafDisplayName("Wersja")]
    [NavigationItem("Słowniki")]
    [DefaultClassOptions]
    [System.ComponentModel.DefaultProperty("name")]
    public partial class car_trim
    {


        public car_trim(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
