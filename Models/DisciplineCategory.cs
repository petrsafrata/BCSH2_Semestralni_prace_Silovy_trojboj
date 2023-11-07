namespace Semestralni_prace_Silovy_trojboj.Models
{
    public class DisciplineCategory
    {
        public int DisciplineId { get; set; }
        public Discipline Discipline { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
