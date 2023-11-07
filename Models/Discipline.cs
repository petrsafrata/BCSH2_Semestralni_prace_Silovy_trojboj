namespace Semestralni_prace_Silovy_trojboj.Models
{
    public class Discipline
    {
        public int DisciplineId { get; set; }
        public string Name { get; set; }
        public List<DisciplineCategory> DisciplineCategories { get; set; }
        public List<Result> Results { get; set; }
    }
}
