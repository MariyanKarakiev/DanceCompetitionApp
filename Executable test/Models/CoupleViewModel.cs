using BussinessLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Models
{
    public class CoupleViewModel
    { 
        public string Name { get; set; }
        public string NewName { get; set; }
        public string CompetetiveClass { get; set; }
        public int Sum { get; set; }

        public int Place { get; set; }

        public List<(string, object?)> JudgePlaceList { get; set; } = new List<(string, object?)>();
        public DateTime CreatedOn { get; init; }
        public DateTime UpdatedOn { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}
