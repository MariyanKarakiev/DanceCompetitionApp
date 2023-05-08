using BussinessLayer.Models;
using BussinessLayer.Models;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.CsvHepler
{
   public class CompetitionMap : ClassMap<Competition>
    {
        public CompetitionMap()
        {
            Map(m => m.Name).Index(0).Name("Име");
            Map(m => m.CreatedOn).Index(2).Name("Създанено");
            Map(m => m.UpdatedOn).Index(3).Name("Редактирано");
            Map(m => m.DeletedOn).Index(4).Name("Изтрито");
        }
    }
}
