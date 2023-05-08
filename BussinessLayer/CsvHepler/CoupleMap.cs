using BussinessLayer.Models;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.CsvHepler
{
   public class CoupleMap : ClassMap<Couple>
    {
        public CoupleMap()
        {
            Map(m => m.Name).Index(0).Name("Име");
            Map(m => m.CompetetiveClass ).Index(1).Name("Състезателен клас");
            Map(m => m.Sum).Index(2).Name("Сума");
            Map(m => m.CreatedOn).Index(3).Name("Създанено");
            Map(m => m.UpdatedOn).Index(4).Name("Редактирано");
            Map(m => m.DeletedOn).Index(5).Name("Изтрито");
        }
    }
}
