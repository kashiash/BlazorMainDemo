using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Text;
using AggregatedAttribute = DevExpress.Xpo.AggregatedAttribute;

namespace KodyPocztowe.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDefaultProperty(nameof(NazwaPowiatu))]
    [NavigationItem("Administracyjne")]
    public class Powiat : XPObject
	{
		public Powiat(Session session) : base(session)
		{ }


        Wojewodztwo wojewodztwo;
        string nazwaPowiatu;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string NazwaPowiatu
        {
            get => nazwaPowiatu;
            set => SetPropertyValue(nameof(NazwaPowiatu), ref nazwaPowiatu, value);
        }

        
        [Association("Wojewodztwo-Powiaty")]
        public Wojewodztwo Wojewodztwo
        {
            get => wojewodztwo;
            set => SetPropertyValue(nameof(Wojewodztwo), ref wojewodztwo, value);
        }

        [Association("Powiat-Gminy"),Aggregated]
        public XPCollection<Gmina> Gminy
        {
            get
            {
                return GetCollection<Gmina>(nameof(Gminy));
            }
        }
        [Association("Powiat-KodyPocztowe"), DevExpress.Xpo.Aggregated]
        public XPCollection<KodPocztowy> KodyPocztowe
        {
            get
            {
                return GetCollection<KodPocztowy>(nameof(KodyPocztowe));
            }
        }
    }
}
