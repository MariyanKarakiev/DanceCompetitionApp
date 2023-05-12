namespace Executable_test.Models
{
    public class JudgingViewModel
    {
        public string CompetitionName { get; set; }
        public Dictionary<string, List<(string Couple, string Place)>> JudgePlacing { get; set; } = new Dictionary<string, List<(string Couple, string Place)>>();
       
    }
}
