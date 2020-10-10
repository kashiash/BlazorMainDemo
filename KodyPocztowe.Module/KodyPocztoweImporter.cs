using Common.Module.Utils;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;

using kashiash.utils;
using KodyPocztowe.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace KodyPocztowe.Module.Imports
{


    public class KodyPocztoweImporter : CSVImporter
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

                ImportujPlik(FileName,';');
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



        public KodyPocztoweImporter()
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

            var rec = new KodPocztowy(unitOfWork);

            //PNA     ; MIEJSCOWOŚĆ; ULICA; NUMERY; GMINA; POWIAT; WOJEWÓDZTWO
            //83 - 440; Abisynia; ; ; Karsin; kościerski; pomorskie


            var woj = unitOfWork.FindObject<Wojewodztwo>(new BinaryOperator(nameof(Wojewodztwo.NazwaWojewodztwa), csv[6]));
            if (woj == null)
            {
                woj = new Wojewodztwo(unitOfWork);
                woj.NazwaWojewodztwa = csv[6].Truncate(100);
                woj.Save();
                unitOfWork.CommitChanges();
            }

            var pow = unitOfWork.FindObject<Powiat>(new BinaryOperator(nameof(Powiat.NazwaPowiatu), csv[5]));
            if (pow == null)
            {
                pow = new Powiat(unitOfWork);
                pow.NazwaPowiatu = csv[5].Truncate(100);
                pow.Wojewodztwo = woj;
                pow.Save();
                unitOfWork.CommitChanges();
            }

            var gmi = unitOfWork.FindObject<Gmina>(new BinaryOperator(nameof(Gmina.NazwaGminy), csv[4]));
            if (gmi == null)
            {
                gmi = new Gmina(unitOfWork);
                gmi.NazwaGminy = csv[4].Truncate(100);
                gmi.Wojewodztwo = woj;
                gmi.Powiat = pow;
                gmi.Save();
                unitOfWork.CommitChanges();

            }
            rec.Wojewodztwo = woj;
            rec.Powiat = pow;
            rec.Gmina = gmi;
            rec.Numery = csv[3].Truncate(100);
            rec.Ulica = csv[2].Truncate(100);
            rec.Miejscowosc = csv[1].Truncate(100);
            rec.Kod = csv[0];
            rec.Save();

            //  Console.WriteLine($"   {rec.value1}");
            if (rowCnt % 1000 == 0)
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
