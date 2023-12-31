using System.ComponentModel.DataAnnotations;

namespace Semestralni_prace_Silovy_trojboj.Models
{
    public class Competitor
    {
        public int CompetitorId { get; set; }
        
        public string? CompetitorFirstName { get; set; }
        public string? CompetitorLastName { get; set; }

        [Range(20, 120, ErrorMessage = "Weight must be between 20 and 120.")]
        public double? Weight { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Result> Results { get; set; } = new List<Result>();

        public int? TotalScore
        {
            get
            {
                if (Results == null || Results.Count == 0)
                {
                    return 0;
                }

                return Results.Sum(r => r.Score);
            }
        }
    }
}
