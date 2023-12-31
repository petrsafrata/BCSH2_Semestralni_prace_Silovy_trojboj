using Semestralni_prace_Silovy_trojboj.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;

namespace Semestralni_prace_Silovy_trojboj.Database
{
    public class ApplicationDbContext : IDisposable
    {
        private readonly SQLiteConnection _connection;

        public List<Category> categories;
        public List<Competitor> competitors;
        public List<Discipline> disciplines;
        public List<Result> results;

        public ApplicationDbContext()
        {
            _connection = new SQLiteConnection("Data Source=Power triathlonDB.db;Version=3");
            categories = GetAllCategories();
            disciplines = GetAllDisciplines();
            competitors = GetAllCompetitors();
            results = GetAllResults();
        }

        public void AddCompetitor(string firstName, string lastName, int categoryId, double? weight)
        {
            _connection.Open();
            int insertCompetitorId;
            using (var command = new SQLiteCommand())
            {
                command.Connection = _connection;
                command.CommandText = "INSERT INTO Competitor (CompetitorFirstName, CompetitorLastName, CategoryId, Weight) VALUES (@CompetitorFirstName, @CompetitorLastName, @CategoryId, @Weight); SELECT last_insert_rowid();";
                command.Parameters.AddWithValue("@CompetitorFirstName", firstName);
                command.Parameters.AddWithValue("@CompetitorLastName", lastName);
                command.Parameters.AddWithValue("@CategoryId", categoryId);
                command.Parameters.AddWithValue("@Weight", weight);
                insertCompetitorId = Convert.ToInt32(command.ExecuteScalar());
                //command.ExecuteNonQuery();

            }
            _connection.Close();
            AddResultsForCompetitor(insertCompetitorId);
        }

        private void AddResultsForCompetitor(int competitorId)
        {
            _connection.Open();
            int[] arrayOfDisciplineId = { 1, 2, 3 };
            foreach (var disciplineId in arrayOfDisciplineId) {
                using (var command = new SQLiteCommand())
                {
                    command.Connection = _connection;
                    command.CommandText = "INSERT INTO Result (CompetitorId, DisciplineId, Score) VALUES (@CompetitorId, @DisciplineId, @Score)";
                    command.Parameters.AddWithValue("@CompetitorId", competitorId);
                    command.Parameters.AddWithValue("@DisciplineId", disciplineId);
                    command.Parameters.AddWithValue("@Score", 0);
                    command.ExecuteNonQuery();
                }
            }
            _connection.Close();
        }

        public List<Competitor> GetAllCompetitors()
        {
            competitors = new List<Competitor>();
            results = GetAllResults();
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

                        var category = categories.FirstOrDefault(c => c.CategoryId == competitor.CategoryId);
                        if (category != null)
                        {
                            competitor.Category = category;
                        }
                     
                        var result = results.Where(r => r.CompetitorId == competitor.CompetitorId).ToList();
                        if (result != null)
                        {
                            competitor.Results = result;
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
            _connection.Open();
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
            _connection.Close();
        }

        public void DeleteCompetitor(int competitorId)
        {
            _connection.Open();
            using (var command = new SQLiteCommand())
            {
                command.Connection = _connection;
                command.CommandText = "DELETE FROM Competitor WHERE CompetitorId = @CompetitorId";
                command.Parameters.AddWithValue("@CompetitorId", competitorId);
                command.ExecuteNonQuery();
            }
            _connection.Close();
            DeleteResultsForCompetitor(competitorId);
        }

        private void DeleteResultsForCompetitor(int competitorId)
        {
            _connection.Open();
            using (var command = new SQLiteCommand())
            {
                command.Connection = _connection;
                command.CommandText = "DELETE FROM Result WHERE CompetitorId = @CompetitorId";
                command.Parameters.AddWithValue("@CompetitorId", competitorId);
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();
            using (var command = new SQLiteCommand("SELECT * FROM Categories", _connection))
            {
                _connection.Open();
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
                _connection.Close();
            }
            return categories;
        }

        public List<Result> GetAllResults()
        {
            results = new List<Result>();
            using (var command = new SQLiteCommand("SELECT * FROM Result", _connection))
            {
                _connection.Open();
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
                _connection.Close();
            }
            return results;
        }

        public List<Discipline> GetAllDisciplines()
        {
            disciplines = new List<Discipline>();
            results = GetAllResults();
            competitors = GetAllCompetitors();

            foreach (var item in results)
            {
                var match = competitors.FirstOrDefault(c => c.CompetitorId == item.CompetitorId);

                if (match != null)
                {
                    item.Competitor = match;
                }
            }

            using (var command = new SQLiteCommand("SELECT * FROM Discipline", _connection))
            {
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var discipline = new Discipline
                        {
                            DisciplineId = Convert.ToInt32(reader["DisciplineId"]),
                            Name = Convert.ToString(reader["Name"])
                        };
                        var result = results.Where(r => r.DisciplineId == discipline.DisciplineId).ToList();
                        if (result != null)
                        {
                            discipline.Results = result;
                        }
                        disciplines.Add(discipline);
                    }
                }
                _connection.Close();
            }
            return disciplines;
        }

        public void UpdateResults(Result result)
        {
            _connection.Open();
            using (var command = new SQLiteCommand())
            {
                command.Connection = _connection;
                command.CommandText = "UPDATE Result SET Score = @Score WHERE ResultId = @ResultId";
                command.Parameters.AddWithValue("@ResultId", result.ResultId);
                command.Parameters.AddWithValue("@Score", result.Score);
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void AddCategory(string name, double? weight)
        {
            _connection.Open();
            using (var command = new SQLiteCommand())
            {
                command.Connection = _connection;
                command.CommandText = "INSERT INTO Categories (Name, Weight) VALUES (@Name, @Weight)";
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Weight", weight);
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
