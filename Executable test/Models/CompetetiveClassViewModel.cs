using BussinessLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Models
{
    public class CompetetiveClassViewModel
    { 
        public string Name { get; set; }
        public string NewName { get; set; }
        public string CompetitionName { get; set; }
        public DateTime CreatedOn { get; init; }
        public DateTime UpdatedOn { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}
