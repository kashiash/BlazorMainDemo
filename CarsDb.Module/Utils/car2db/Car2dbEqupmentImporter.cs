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
using Common.Module.Utils;

namespace CarsDb.Module.Utils
{
    public class Car2dbEquipmentImporter : CSVImporter
    {
        UnitOfWork unitOfWork;
        Session _session;
        CultureInfo culture = CultureInfo.InvariantCulture;


        public void Import(string FileName, bool deleteFile = false)
        {

            if (File.Exists(FileName))
            {
                ImportujPlik(FileName,',');
                unitOfWork.CommitChanges();
                if (deleteFile)
                {
                    File.Delete(FileName);
                }
            }
        }

        public Car2dbEquipmentImporter(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Car2dbEquipmentImporter()
        {
            _session = new Session() { ConnectionString = AppSettings.ConnectionString };
            unitOfWork = new UnitOfWork(_session.DataLayer);
        }

        public override void ImportRow(CsvRow csv)
        {
            // throw new NotImplementedException();
            var rec = unitOfWork.GetObjectByKey<car_equipment>(csv[0].ToInt());
            if (rec == null)
            {
                rec = new car_equipment(unitOfWork);
            }
            int i = 0;
            rec.id_car_equipment = csv[i].ToInt(); i++;
            rec.id_car_trim = unitOfWork.GetObjectByKey<car_trim>(csv[i].ToInt());i++;
            rec.name = csv[i].Truncate(100);i++;
            rec.year = csv[i].ToInt();i++;
 
            rec.date_create = csv[i].ToInt();i++;
            rec.date_update = csv[i].ToInt();i++;
            rec.id_car_type = unitOfWork.GetObjectByKey<car_type>(csv[i].ToInt());
            rec.Save();

          //  Console.WriteLine($"{rec.id_car_type.name} {rec.id_car_trim.name}  {rec.name}");
            if (rowCnt % 100 == 0)
            {
                unitOfWork.CommitChanges();
            }
        }

    }
}
