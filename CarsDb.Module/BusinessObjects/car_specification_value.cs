using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace CarsDb.Module.BusinessObjects
{
    [System.ComponentModel.DefaultProperty("value1")]

    //[XafDisplayName("Specyfikacje")]
    //[NavigationItem("Słowniki")]
    //[DefaultClassOptions]

    public partial class car_specification_value
    {
        public car_specification_value(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
