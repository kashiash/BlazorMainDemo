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

    public partial class car_serie : XPLiteObject
    {
        int fid_car_serie;
        [Key(false)]
        public int id_car_serie
        {
            get { return fid_car_serie; }
            set { SetPropertyValue<int>("id_car_serie", ref fid_car_serie, value); }
        }
        car_model fid_car_model;
        [Association(@"car_serieReferencescar_model")]
        public car_model id_car_model
        {
            get { return fid_car_model; }
            set { SetPropertyValue<car_model>("id_car_model", ref fid_car_model, value); }
        }
        string fname;
        [Size(255)]
        public string name
        {
            get { return fname; }
            set { SetPropertyValue<string>("name", ref fname, value); }
        }
        int fdate_create;
        [Browsable(false)]
        public int date_create
        {
            get { return fdate_create; }
            set { SetPropertyValue<int>("date_create", ref fdate_create, value); }
        }
        int fdate_update;
        [Browsable(false)]
        public int date_update
        {
            get { return fdate_update; }
            set { SetPropertyValue<int>("date_update", ref fdate_update, value); }
        }
        car_generation fid_car_generation;
        [Association(@"car_serieReferencescar_generation")]
        public car_generation id_car_generation
        {
            get { return fid_car_generation; }
            set { SetPropertyValue<car_generation>("id_car_generation", ref fid_car_generation, value); }
        }
        car_type fid_car_type;
        [Association(@"car_serieReferencescar_type")]
        public car_type id_car_type
        {
            get { return fid_car_type; }
            set { SetPropertyValue<car_type>("id_car_type", ref fid_car_type, value); }
        }
        [Association(@"car_trimReferencescar_serie")]
        public XPCollection<car_trim> car_trims { get { return GetCollection<car_trim>("car_trims"); } }
    }

}