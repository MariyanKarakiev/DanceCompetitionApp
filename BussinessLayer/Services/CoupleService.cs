using BussinessLayer.CsvHepler;
using BussinessLayer.Models;
using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Dynamic;
using System.Numerics;

namespace BussinessLayer.Services
{
    public class CoupleService
    {
        private const string couplesFile = "./Couples.csv";

        public CoupleService() { }


        public IEnumerable<Couple> GetAll(string compClass)
        {
            var couples = ReadCsv();

            var result = couples.Where(c => c.CompetetiveClass == compClass).Select(c => new Couple()
            {
                Name = c.Name,
                CompetetiveClass = c.CompetetiveClass
            }).ToList();

            return result;
        }
        public Couple Get(string name, string compClass)
        {
            //get the one with the right compClass
            var couples = ReadCsv();

            var result = couples.Where(c => c.Name == name && c.CompetetiveClass == compClass).Select(c =>
            new Couple()
            {
                Name = c.Name,
                CompetetiveClass = c.CompetetiveClass,

            }).FirstOrDefault();

            return result;
        }

        public void Create(Couple couple, int judgesCount)
        {
            var couples = ReadCsv();

            couple.CreatedOn = DateTime.Now;
            var couplesWithJudges = AddJudges(couple, judgesCount);

            var coupleToAdd = couples.Concat(couplesWithJudges);

            WriteCsv(coupleToAdd, true);
        }
        public void Create(IEnumerable<Couple> couple, int judgesCount)
        {


            if (couple == null)
            {
                throw new Exception("couples is null");
            }
            if (judgesCount == 0)
            {
                throw new Exception("judgesCount is null");
            }

            var couplesWithJudges = AddJudges(couple, 5);
            //  WriteCsv(couplesWithJudges, true);
        }

        public void Delete(string name, string compClass)
        {
            var couples = ReadCsv();

            if (compClass != null)
            {
                IEnumerable<dynamic> coupleToDelete = couples.Where(c => c.Name != name && c.CompetetiveClass == compClass).ToList();

                WriteCsv(coupleToDelete, false);
            }
        }

        private IEnumerable<dynamic> AddJudges(IEnumerable<Couple> couples, int judgesCount)
        {
            var jsonString = JsonSerializer.Serialize(couples);

            var jsonObj = JsonSerializer.Deserialize<IEnumerable<ExpandoObject>>(jsonString);

            var judgesList = new List<string>();

            for (int i = 65; i < judgesCount + 65; i++)
            {
                judgesList.Add("Judge" + ((char)i).ToString());
            }


            foreach (var couple in jsonObj)
            {
                IDictionary<string, object> coupleDict = couple;

                foreach (var judge in judgesList)
                {
                    if (coupleDict.ContainsKey(judge))
                    {
                        coupleDict.Add(judge, 0);
                    }
                    else
                    {
                        coupleDict[judge] = 0;
                    }
                }
            }

            var couplesWithJudges = jsonObj.Select(o => (dynamic)o);

            return couplesWithJudges;
        }

        private IEnumerable<dynamic> AddJudges(Couple couple, int judgesCount)
        {
            var coupleList = new List<Couple>() { couple };

            return AddJudges(coupleList, judgesCount);
        }



        private void WriteCsv(IEnumerable<dynamic> objects, bool creating)
        {
            if (objects != null)
            {
                var couples = ReadCsv();

                if (couples.Count() == 0 && creating)
                {
                    using (var writer = new StreamWriter(couplesFile))
                    {
                        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                        {
                            csv.WriteRecords(objects);
                        }
                    }

                }

                else
                {
                    using (var writer = new StreamWriter(couplesFile))
                    {
                        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                        {
                            csv.WriteRecords(objects);
                        }
                    }
                }
            }
        }
        private IEnumerable<dynamic> ReadCsv()
        {
            IEnumerable<dynamic> result;

            using (var reader = new StreamReader(couplesFile))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {

                    result = csv.GetRecords<dynamic>().ToList();
                }
            }
            return result;
        }
    }
}
