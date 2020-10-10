using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace CarsDb.Module.BusinessObjects
{
    [XafDisplayName("Typy pojazdów")]
    [NavigationItem("Słowniki")]
    [DefaultClassOptions]
    [System.ComponentModel.DefaultProperty("name")]

    public partial class car_type
    {
        public car_type(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
