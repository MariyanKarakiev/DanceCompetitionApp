using BussinessLayer.CsvHepler;
using BussinessLayer.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Services
{
    public class CompetitionService
    {
        string file = @".\Database.csv";

        public List<Competition> Competitions { get; set; } = new List<Competition>();
        public CompetitionService()
        { }
        public List<Competition> GetAll()
        {
            var competitions = ReadCsv();
            return competitions;
        }
        public Competition Get(string name)
        {
            var competitions = ReadCsv();
            var competitionToDelete = competitions.Where(c => c.Name == name).FirstOrDefault();
            return competitionToDelete;
        }
        public void Create(string name)
        {
            var competitionToCreate = new Competition()
            {
                Name = name
            };

            var competitions = ReadCsv();

            if (competitions != null)
            {
                competitions.Add(competitionToCreate);
                WriteCsv(competitions, true);
            }
        }
        public void Delete(string name)
        {
            var competitions = ReadCsv();

            if (competitions != null)
            {
                competitions.Remove(competitions.Where(c => c.Name == name).FirstOrDefault());
                WriteCsv(competitions, false);
            }
        }
        public void Update(string name, Competition competition)
        {
            var competitions = ReadCsv();

            if (competitions != null)
            {
                var competitionToUpdate = competitions.Where(c => c.Name == name).FirstOrDefault();

                competitionToUpdate.Name = competition.Name;
                competitionToUpdate.UpdatedOn = competition.UpdatedOn;
                competitionToUpdate.CreatedOn = competition.CreatedOn;
                competitionToUpdate.DeletedOn = competition.DeletedOn;

                WriteCsv(competitions, false);
            }
        }
        private void WriteCsv(List<Competition> competitions, bool creating)
        {
            if (ReadCsv() == null && creating)
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    // Don't write the header again.
                    HasHeaderRecord = false,
                };
                using (var stream = File.Open(file, FileMode.Append))
                {
                    using (var writer = new StreamWriter(file))
                    {
                        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                        {
                            csv.WriteRecords(competitions);
                        }
                    }
                }
            }

            else
            {
                using (var writer = new StreamWriter(file))
                {
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.Context.RegisterClassMap<CompetitionMap>();
                        csv.WriteRecords(competitions);
                    }
                }
            }

        }
        private List<Competition> ReadCsv()
        {
            using (var reader = new StreamReader(file))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<CompetitionMap>();
                return csv.GetRecords<Competition>().ToList();
            }
        }
    }
}
