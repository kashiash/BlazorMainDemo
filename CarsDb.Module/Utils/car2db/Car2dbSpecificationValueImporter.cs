using DevExpress.Xpo;
using CarsDb.Module.BusinessObjects;


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kashiash.utils;
using Common.Module.Utils;

namespace CarsDb.Module.Utils
{
    public class Car2dbSpecificationValueImporter : CSVImporter
    {
        UnitOfWork unitOfWork;
        Session _session;
        CultureInfo culture = CultureInfo.InvariantCulture;


        public void Import(string FileName, bool deleteFile = false)
        {

            if (File.Exists(FileName))
            {
                 watch = new System.Diagnostics.Stopwatch();

                watch.Start();

                ImportujPlik(FileName, ',');
                if (unitOfWork != null)
                {
                    unitOfWork.CommitChanges();
                }

                Console.WriteLine($"Specification Value Import Time: {watch.ElapsedMilliseconds} ms");


                if (deleteFile)
                {
                    File.Delete(FileName);
                }
            }
        }



        public Car2dbSpecificationValueImporter()
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
            var rec = unitOfWork.GetObjectByKey<car_specification_value>(csv[0].ToInt());
            if (rec == null)
            {
                rec = new car_specification_value(unitOfWork);
            }
            int i = 0;
            rec.id_car_specification_value = csv[i].ToInt(); i++;
            rec.id_car_trim = unitOfWork.GetObjectByKey<car_trim>(csv[i].ToInt()); i++;

            rec.id_car_specification = unitOfWork.GetObjectByKey<car_specification>(csv[i].ToInt()); i++;

            rec.value1 = csv[i].Truncate(100);i++;
            rec.unit = csv[i].Truncate(100);i++;

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
