using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Text;

namespace KodyPocztowe.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDefaultProperty(nameof(NazwaWojewodztwa))]
    [NavigationItem("Administracyjne")]
    public class Wojewodztwo : XPObject
    {
        public Wojewodztwo(Session session) : base(session)
        { }

        string nazwa;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string NazwaWojewodztwa
        {
            get => nazwa;
            set => SetPropertyValue(nameof(NazwaWojewodztwa), ref nazwa, value);
        }


        [Association("Wojewodztwo-Powiaty"), DevExpress.Xpo.Aggregated]
        public XPCollection<Powiat> Powiaty
        {
            get
            {
                return GetCollection<Powiat>(nameof(Powiaty));
            }
        }

        [Association("Wojewodztwo-Gminy"), DevExpress.Xpo.Aggregated]
        public XPCollection<Gmina> Gminy
        {
            get
            {
                return GetCollection<Gmina>(nameof(Gminy));
            }
        }

        [Association("Wojewodztwo-KodyPocztowe"), DevExpress.Xpo.Aggregated]
        public XPCollection<KodPocztowy> KodyPocztowe
        {
            get
            {
                return GetCollection<KodPocztowy>(nameof(KodyPocztowe));
            }
        }
    }

}
