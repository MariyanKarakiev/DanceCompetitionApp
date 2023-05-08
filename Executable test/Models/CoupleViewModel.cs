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
        public int JudgesCount { get; set; }

        public Dictionary<string,int> JudgePlace { get; set; }

        public DateTime CreatedOn { get; init; }
        public DateTime UpdatedOn { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}
