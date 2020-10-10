using DevExpress.Xpo;
using CarsDb.Module.BusinessObjects;

using kashiash.utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kashiash.utils;
using Common.Module.Utils;

namespace CarsDb.Module.Utils
{
    public class Car2dbTrimImporter : CSVImporter
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

        public Car2dbTrimImporter(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Car2dbTrimImporter()
        {
            _session = new Session() { ConnectionString = AppSettings.ConnectionString };
       
        }

        public override void ImportRow(CsvRow csv)
        {
            if (unitOfWork == null)
            {
                unitOfWork = new UnitOfWork(_session.DataLayer);
            }
            // throw new NotImplementedException();
            var rec = unitOfWork.GetObjectByKey<car_trim>(csv[0].ToInt());
            if (rec == null)
            {
                rec = new car_trim(unitOfWork);
            }

            rec.id_car_trim = csv[0].ToInt();
            rec.id_car_serie = unitOfWork.GetObjectByKey<car_serie>(csv[1].ToInt());
            rec.id_car_model = unitOfWork.GetObjectByKey<car_model>(csv[2].ToInt());
            rec.name = csv[3].Truncate(100);
            rec.StartProductionYear = csv[4].ToInt();
            rec.EndProductionYear = csv[5].ToInt();
            rec.date_create = csv[6].ToInt();
            rec.date_update = csv[7].ToInt();
            rec.id_car_type = unitOfWork.GetObjectByKey<car_type>(csv[8].ToInt());
            rec.Save();

          //  Console.WriteLine($"{rec.id_car_model.name} {rec.id_car_serie.name}  {rec.name}");
            if (rowCnt % 10000 == 0)
            {
                unitOfWork.CommitChanges();
            }
        }

    }
}
