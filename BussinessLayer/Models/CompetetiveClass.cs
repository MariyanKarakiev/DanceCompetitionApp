using BussinessLayer.Common;

namespace BussinessLayer.Models
{
    public class CompetetiveClass : BaseClass
    {
        public string CompetitionName { get; set; }
        public int JudgesCount { get; set; }
        public int CouplesCount { get; set; }
    }
}