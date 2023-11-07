namespace Semestralni_prace_Silovy_trojboj.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public List<DisciplineCategory> DisciplineCategory { get; set; }
        public List<Competitor> Competitors { get; set; }
    }
}
