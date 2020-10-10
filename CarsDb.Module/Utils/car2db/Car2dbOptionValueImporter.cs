using DevExpress.Xpo;
using CarsDb.Module.BusinessObjects;

using kashiash.utils;
using kashiash.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Module.Utils;

namespace CarsDb.Module.Utils
{
    public class Car2dbOptionValueImporter : CSVImporter
    {
        UnitOfWork unitOfWork;
        Session _session;
        CultureInfo culture = CultureInfo.InvariantCulture;

      

        public void Import(string FileName, bool deleteFile = false)
        {

            if (File.Exists(FileName))
            {


                ImportujPlik(FileName, ',');
                if (unitOfWork != null)
                {
                    unitOfWork.CommitChanges();
                }


                if (deleteFile)
                {
                    File.Delete(FileName);
                }
            }
        }



        public Car2dbOptionValueImporter()
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
            var rec = unitOfWork.GetObjectByKey<car_option_value>(csv[0].ToInt());
            if (rec == null)
            {
                rec = new car_option_value(unitOfWork);
            }
            int i = 0;

//            'id_car_option_value','id_car_option','id_car_equipment','is_base','date_create','date_update','id_car_type'
//              '1451700','2','1','1','1545067201','1545067201','1'

            rec.id_car_option_value = csv[i].ToInt(); i++;
            rec.id_car_option = unitOfWork.GetObjectByKey<car_option>(csv[i].ToInt()); i++;

            rec.id_car_equipment = unitOfWork.GetObjectByKey<car_equipment>(csv[i].ToInt()); i++;

            rec.is_base = csv[i].ToInt();i++;
     

            rec.date_create = csv[i].ToInt(); i++;
            rec.date_update = csv[i].ToInt();i++;
            rec.id_car_type = unitOfWork.GetObjectByKey<car_type>(csv[i].ToInt()); i++;
            rec.Save();

          //  Console.WriteLine($"   {rec.value1}");
            if (rowCnt % 100000 == 0)
            {
                Console.WriteLine($"recs: {rowCnt} Execution Time: {watch.ElapsedMilliseconds} ms");
                unitOfWork.CommitChanges();
                unitOfWork.Dispose();
                unitOfWork = null;

               

                Console.WriteLine($"After commit Execution Time: {watch.ElapsedMilliseconds} ms");
                //Console.ReadLine();
                //watch.Restart();

            }
        }

    }
}
