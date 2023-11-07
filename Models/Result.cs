namespace Semestralni_prace_Silovy_trojboj.Models
{
    public class Result
    {
        public int ResultId { get; set; }
        public int CompetitorId { get; set; }
        public Competitor Competitor { get; set; }
        public int DisciplineId { get; set; }
        public Discipline Discipline { get; set; }
        public int Score { get; set; }
    }
}
