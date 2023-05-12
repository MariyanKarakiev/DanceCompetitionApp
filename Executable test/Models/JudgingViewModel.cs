namespace Executable_test.Models
{
    public class JudgingViewModel
    {

        public Dictionary<string, List<(string Couple, string Place)>> JudgePlacing { get; set; } = new Dictionary<string, List<(string Couple, string Place)>>();
       
    }
}
