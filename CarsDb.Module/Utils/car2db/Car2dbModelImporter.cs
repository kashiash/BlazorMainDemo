using DevExpress.Xpo;
using CarsDb.Module.BusinessObjects;

using kashiash.utils;
using kashiash.utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Module.Utils;

namespace CarsDb.Module.Utils
{
    public class Car2dbModelImporter : CSVImporter
    {
        UnitOfWork unitOfWork;
        Session _session;
        CultureInfo culture = CultureInfo.InvariantCulture;


        public void Import(string FileName, bool deleteFile = false)
        {

            if (File.Exists(FileName))
            {
                ImportujPlik(FileName, ',');
                unitOfWork.CommitChanges();
                if (deleteFile)
                {
                    File.Delete(FileName);
                }
            }
        }

        public Car2dbModelImporter(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Car2dbModelImporter()
        {
            _session = new Session() { ConnectionString = AppSettings.ConnectionString };
            unitOfWork = new UnitOfWork(_session.DataLayer);
        }

        public override void ImportRow(CsvRow csv)
        {
            // throw new NotImplementedException();
            var rec = unitOfWork.GetObjectByKey<car_model>(csv[0].ToInt());
            if (rec == null)
            {
                rec = new car_model(unitOfWork);
            }
          
            rec.id_car_model= csv[0].ToInt();
            rec.id_car_make = unitOfWork.GetObjectByKey<car_make>(csv[1].ToInt());
            rec.name = csv[2].Truncate(100);
            rec.id_car_type = unitOfWork.GetObjectByKey<car_type>(csv[5].ToInt());
            rec.Save();

          //  Console.WriteLine($"{rec.id_car_make.name} {rec.name}");
            if (rowCnt % 100 == 0)
            {
                unitOfWork.CommitChanges();
            }
        }

    }
}
