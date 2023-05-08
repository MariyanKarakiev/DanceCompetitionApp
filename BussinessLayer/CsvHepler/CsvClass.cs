using BussinessLayer.Models;
using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.CsvHepler
{
    public class CsvClass<T> where T : class
    {
        //public void Write(string file, List<T> competitions, bool creating)
        //{
        //    if (Read(file) == null && creating)
        //    {
        //        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        //        {
        //            // Don't write the header again.
        //            HasHeaderRecord = false,
        //        };
        //        using (var stream = File.Open(file, FileMode.Append))
        //        using (var writer = new StreamWriter(file))
        //        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        //        {
        //            csv.WriteRecords(competitions);
        //        }
        //    }
        //    else
        //    {
        //        using (var writer = new StreamWriter(file))
        //        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        //        {
        //            csv.Context.RegisterClassMap<CompetitionMap>();
        //            csv.WriteRecords(competitions);
        //        }
        //    }

        //}
        //public List<T> Read(string file)
        //{
        //    using (var reader = new StreamReader(file))
        //    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        //    {
        //        var type = typeof(T);
        //        switch (typeof(T))
        //        {
        //            case (string)(Competition):
        //                csv.Context.RegisterClassMap<CompetitionMap>();
        //                break;
        //        }
        //        return csv.GetRecords<T>().ToList();
        //    }
        //}
    }
}
