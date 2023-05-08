using BussinessLayer.CsvHepler;
using BussinessLayer.Models;
using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace BussinessLayer.Services
{
    public class CompetetiveClassService
    {
        string compClassesFile = @".\CompetetiveClasses.csv";
        string judgesFile = @".\CompetetiveClasses.csv";

        public List<CompetetiveClass> GetAll()
        {
            var compClass = ReadCsv();
            return compClass;
        }
        public CompetetiveClass Get(string name)
        {
            var compClass = ReadCsv();

            var competitionToDelete = compClass.Where(c => c.Name == name).FirstOrDefault();
            return competitionToDelete;
        }
        public void Create(string name, string competitionName)
        {
            var competitionToCreate = new CompetetiveClass()
            {
                Name = name,
                CompetitionName = competitionName,
                CreatedOn = DateTime.Now
            };

            var compClass = ReadCsv();

            if (compClass != null)
            {
                compClass.Add(competitionToCreate);
                WriteCsv(compClass, true);
            }
        }
        public void Delete(string name)
        {
            var compClass = ReadCsv();

            if (compClass != null)
            {
                compClass.Remove(compClass.Where(c => c.Name == name).FirstOrDefault());
                WriteCsv(compClass, false);
            }
        }
        public void Update(string name, CompetetiveClass compClass)
        {
            var compClasses = ReadCsv();

            if (compClass != null)
            {
                var competitionToUpdate = compClasses.Where(c => c.Name == name).FirstOrDefault();

                competitionToUpdate.Name = compClass.Name;
                competitionToUpdate.CompetitionName = compClass.CompetitionName;
                competitionToUpdate.UpdatedOn = DateTime.Now;
                competitionToUpdate.CreatedOn = compClass.CreatedOn;
                competitionToUpdate.DeletedOn = compClass.DeletedOn;

                WriteCsv(compClasses, false);
            }
        }

        public void AddJudges(int count)
        {
            var judges = new List<string>();

            for (int i = 65; i < count+65; i++)
            {
                judges.Add(((char)i).ToString());
            }
        }

        private void WriteCsv(List<CompetetiveClass> compClass, bool creating)
        {
            if (ReadCsv() == null && creating)
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    // Don't write the header again.
                    HasHeaderRecord = false,
                };
                using (var stream = File.Open(compClassesFile, FileMode.Append))
                {
                    using (var writer = new StreamWriter(compClassesFile))
                    {
                        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                        {
                            csv.WriteRecords(compClass);
                        }
                    }
                }
            }
            else
            {
                using (var writer = new StreamWriter(compClassesFile))
                {
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.Context.RegisterClassMap<CompetetiveClassMap>();
                        csv.WriteRecords(compClass);
                    }
                }
            }

        }
    

        

        private List<CompetetiveClass> ReadCsv()
        {
            using (var reader = new StreamReader(compClassesFile))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<CompetetiveClassMap>();
                return csv.GetRecords<CompetetiveClass>().ToList();
            }
        }
    }
}
