using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace CarsDb.Module.BusinessObjects
{
    [System.ComponentModel.DefaultProperty("name")]
    public partial class car_specification
    {
        public car_specification(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
