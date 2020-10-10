using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Text;

namespace KodyPocztowe.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDefaultProperty(nameof(NazwaGminy))]
    [NavigationItem("Administracyjne")]
    public class Gmina : XPObject
	{
		public Gmina(Session session) : base(session)
		{ }


        Wojewodztwo wojewodztwo;
        Powiat powiat;
        string nazwaGminy;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string NazwaGminy
        {
            get => nazwaGminy;
            set => SetPropertyValue(nameof(NazwaGminy), ref nazwaGminy, value);
        }

        
        [Association("Wojewodztwo-Gminy")]
        public Wojewodztwo Wojewodztwo
        {
            get => wojewodztwo;
            set => SetPropertyValue(nameof(Wojewodztwo), ref wojewodztwo, value);
        }

        [Association("Powiat-Gminy")]
        public Powiat Powiat
        {
            get => powiat;
            set => SetPropertyValue(nameof(Powiat), ref powiat, value);
        }

        [Association("Gmina-KodyPocztowe"), DevExpress.Xpo.Aggregated]
        public XPCollection<KodPocztowy> KodyPocztowe
        {
            get
            {
                return GetCollection<KodPocztowy>(nameof(KodyPocztowe));
            }
        }
    }
}
