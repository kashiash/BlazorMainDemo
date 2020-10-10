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
    public class Car2dbGenerationImporter : CSVImporter
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

        public Car2dbGenerationImporter(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Car2dbGenerationImporter()
        {
            _session = new Session() { ConnectionString = AppSettings.ConnectionString };
            unitOfWork = new UnitOfWork(_session.DataLayer);
        }

        public override void ImportRow(CsvRow csv)
        {
            // throw new NotImplementedException();
            var rec = unitOfWork.GetObjectByKey<car_generation>(csv[0].ToInt());
            if (rec == null)
            {
                rec = new car_generation(unitOfWork);
            }
          
            rec.id_car_generation= csv[0].ToInt();
            rec.id_car_model = unitOfWork.GetObjectByKey<car_model>(csv[1].ToInt());
            rec.name = csv[2].Truncate(100);
            rec.year_begin = csv[3];
            rec.year_end = csv[4];

            rec.id_car_type = unitOfWork.GetObjectByKey<car_type>(csv[7].ToInt());
            rec.Save();

         //   Console.WriteLine($"{rec.id_car_model.name} {rec.name}");
            if (rowCnt % 10000 == 9999)
            {
                unitOfWork.CommitChanges();
            }
        }

    }
}
