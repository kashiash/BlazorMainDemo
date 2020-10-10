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
    public class Car2dbSerieImporter : CSVImporter
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

        public Car2dbSerieImporter(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Car2dbSerieImporter()
        {
            _session = new Session() { ConnectionString = AppSettings.ConnectionString };
            unitOfWork = new UnitOfWork(_session.DataLayer);
        }

        public override void ImportRow(CsvRow csv)
        {
            // throw new NotImplementedException();
            var rec = unitOfWork.GetObjectByKey<car_serie>(csv[0].ToInt());
            if (rec == null)
            {
                rec = new car_serie(unitOfWork);
            }
          
            rec.id_car_serie= csv[0].ToInt();
            rec.id_car_model = unitOfWork.GetObjectByKey<car_model>(csv[1].ToInt());
            rec.id_car_generation =  unitOfWork.GetObjectByKey<car_generation>(csv[2].ToInt());
            rec.name = csv[3].Truncate(100);
            rec.id_car_type = unitOfWork.GetObjectByKey<car_type>(csv[6].ToInt());
            rec.Save();

      //      Console.WriteLine($"{rec.id_car_model.name} {rec.id_car_generation.name}  {rec.name}");
            if (rowCnt % 10000 == 9999)
            {
                unitOfWork.CommitChanges();
            }
        }

    }
}
