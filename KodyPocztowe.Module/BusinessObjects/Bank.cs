using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KodyPocztowe.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(Symbol))]
    [NavigationItem("Administracyjne")]
    public class Bank : XPObject
    {
        public Bank(Session session) : base(session)
        { }


        string nazwa;
        string iBAN;
        string symbol;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Symbol
        {
            get => symbol;
            set => SetPropertyValue(nameof(Symbol), ref symbol, value);
        }
        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Nazwa
        {
            get => nazwa;
            set => SetPropertyValue(nameof(Nazwa), ref nazwa, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string IBAN
        {
            get => iBAN;
            set => SetPropertyValue(nameof(IBAN), ref iBAN, value);
        }
    }
}
