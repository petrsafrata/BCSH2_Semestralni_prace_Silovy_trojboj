using System.ComponentModel.DataAnnotations;

namespace Semestralni_prace_Silovy_trojboj.Models
{
    public class Result
    {
        public int ResultId { get; set; }
        public int CompetitorId { get; set; }
        public Competitor Competitor { get; set; }
        public int DisciplineId { get; set; }
        public Discipline Discipline { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Score must be greater than or equal to 0.")]
        public int? Score { get; set; }
    }
}
