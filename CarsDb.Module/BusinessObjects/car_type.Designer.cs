﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace CarsDb.Module.BusinessObjects
{

    public partial class car_type : XPLiteObject
    {
        int fid_car_type;
        [Key(false)]
        public int id_car_type
        {
            get { return fid_car_type; }
            set { SetPropertyValue<int>("id_car_type", ref fid_car_type, value); }
        }
        string fname;
        [Size(255)]
        public string name
        {
            get { return fname; }
            set { SetPropertyValue<string>("name", ref fname, value); }
        }
        [Association(@"car_modelReferencescar_type")]
        public XPCollection<car_model> car_models { get { return GetCollection<car_model>("car_models"); } }
        [Association(@"car_serieReferencescar_type")]
        public XPCollection<car_serie> car_series { get { return GetCollection<car_serie>("car_series"); } }
        [Association(@"car_specification_valueReferencescar_type")]
        public XPCollection<car_specification_value> car_specification_values { get { return GetCollection<car_specification_value>("car_specification_values"); } }
        [Association(@"car_optionReferencescar_type")]
        public XPCollection<car_option> car_options { get { return GetCollection<car_option>("car_options"); } }
        [Association(@"car_specificationReferencescar_type")]
        public XPCollection<car_specification> car_specifications { get { return GetCollection<car_specification>("car_specifications"); } }
        [Association(@"car_generationReferencescar_type")]
        public XPCollection<car_generation> car_generations { get { return GetCollection<car_generation>("car_generations"); } }
        [Association(@"car_trimReferencescar_type")]
        public XPCollection<car_trim> car_trims { get { return GetCollection<car_trim>("car_trims"); } }
        [Association(@"car_equipmentReferencescar_type")]
        public XPCollection<car_equipment> car_equipments { get { return GetCollection<car_equipment>("car_equipments"); } }
    }

}