using Semestralni_prace_Silovy_trojboj.Models;
using System;
using System.Data;
using System.Data.SQLite;

namespace Semestralni_prace_Silovy_trojboj.Database
{
    public class ApplicationDbContext : IDisposable
    {
        private readonly SQLiteConnection _connection;

        public ApplicationDbContext()
        {
            _connection = new SQLiteConnection("Data Source=Power triathlonDB.db;Version=3");
        }

        public void AddCompetitor(string firstName, string lastName, int categoryId, double weight)
        {
            using (var command = new SQLiteCommand())
            {
                command.Connection = _connection;
                command.CommandText = "INSERT INTO Competitor (CompetitorFirstName, CompetitorLastName, CategoryId, Weight) VALUES (@CompetitorFirstName, @CompetitorLastName, @CategoryId, @Weight)";
                command.Parameters.AddWithValue("@CompetitorFirstName", firstName);
                command.Parameters.AddWithValue("@CompetitorLastName", lastName);
                command.Parameters.AddWithValue("@CategoryId", categoryId);
                command.Parameters.AddWithValue("@Weight", weight);
                command.ExecuteNonQuery();
            }
        }

        public List<Competitor> GetAllCompetitors()
        {
            var competitors = new List<Competitor>();
            using (var command = new SQLiteCommand("SELECT * FROM Competitor", _connection)) {
                _connection.Open();
            using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var competitor = new Competitor
                        {
                            CompetitorId = Convert.ToInt32(reader["CompetitorId"]),
                            CompetitorFirstName = Convert.ToString(reader["CompetitorFirstName"]),
                            CompetitorLastName = Convert.ToString(reader["CompetitorLastName"]),
                            CategoryId = Convert.ToInt32(reader["CategoryId"]),
                            Weight = Convert.ToDouble(reader["Weight"])
                        };

                        var category = GetAllCategories().FirstOrDefault(c => c.CategoryId == competitor.CategoryId);
                        if (category != null)
                        {
                            competitor.Category = category;
                        }

                        var results = GetAllResults().Where(r => r.CompetitorId == competitor.CompetitorId).ToList();
                        if (results != null)
                        {
                            competitor.Results = results;
                        }

                        competitors.Add(competitor);
                    }
                    _connection.Close();
                }
            }
            return competitors;
        }

        public void UpdateCompetitor(Competitor competitor)
        {
            using (var command = new SQLiteCommand())
            {
                command.Connection = _connection;
                command.CommandText = "UPDATE Competitor SET CompetitorFirstName = @CompetitorFirstName, CompetitorLastName = @CompetitorLastName, CategoryId = @CategoryId, Weight = @Weight WHERE CompetitorId = @CompetitorId";
                command.Parameters.AddWithValue("@CompetitorFirstName", competitor.CompetitorFirstName);
                command.Parameters.AddWithValue("@CompetitorLastName", competitor.CompetitorLastName);
                command.Parameters.AddWithValue("@CategoryId", competitor.CategoryId);
                command.Parameters.AddWithValue("@CompetitorId", competitor.CompetitorId);
                command.Parameters.AddWithValue("@Weight", competitor.Weight);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteCompetitor(int competitorId)
        {
            using (var command = new SQLiteCommand())
            {
                command.Connection = _connection;
                command.CommandText = "DELETE FROM Competitor WHERE CompetitorId = @CompetitorId";
                command.Parameters.AddWithValue("@CompetitorId", competitorId);
                command.ExecuteNonQuery();
            }
        }

        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();
            using (var command = new SQLiteCommand("SELECT * FROM Categories", _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var category = new Category
                        {
                            CategoryId = Convert.ToInt32(reader["CategoryId"]),
                            Name = Convert.ToString(reader["Name"]),
                            Weight = Convert.ToDouble(reader["Weight"])
                        };
                        categories.Add(category);
                    }
                }
            }
            return categories;
        }

        public List<Result> GetAllResults()
        {
            var results = new List<Result>();
            using (var command = new SQLiteCommand("SELECT * FROM Result", _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var result = new Result
                        {
                            ResultId = Convert.ToInt32(reader["ResultId"]),
                            CompetitorId = Convert.ToInt32(reader["CompetitorId"]),
                            DisciplineId = Convert.ToInt32(reader["DisciplineId"]),
                            Score = Convert.ToInt32(reader["Score"])
                        };
                        results.Add(result);
                    }
                }
            }
            return results;
        }


        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
