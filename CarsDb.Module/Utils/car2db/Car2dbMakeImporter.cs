using DevExpress.Xpo;


using kashiash.utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsDb.Module.BusinessObjects;
using Common.Module.Utils;

namespace CarsDb.Module.Utils
{
    public class Car2dbMakeImporter : CSVImporter
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

        public Car2dbMakeImporter(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Car2dbMakeImporter()
        {
            _session = new Session() { ConnectionString = AppSettings.ConnectionString };
            unitOfWork = new UnitOfWork(_session.DataLayer);
        }

        public override void ImportRow(CsvRow csv)
        {
            // throw new NotImplementedException();

            var rec = unitOfWork.GetObjectByKey<car_make>(csv[0].ToInt());
            if (rec == null)
            {
                rec = new car_make(unitOfWork);
            }
            rec.id_car_make = csv[0].ToInt();
            rec.name = csv[1].Truncate(100);
            rec.id_car_type = GetType(csv[4].ToInt());

            rec.Save();
         //   Console.WriteLine($" {rec.name}");
            if (rowCnt % 100 == 0)
            {
                unitOfWork.CommitChanges();
            }
        }

        private car_type GetType(int type)
        {
            return unitOfWork.GetObjectByKey<car_type>(type);
        }
    }
}
