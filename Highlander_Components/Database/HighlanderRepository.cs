using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Highlander_Components.lander;

namespace Highlander_Component.Database
{
    public class HighlanderRepository
    {
        private readonly string _connectionString;

        public HighlanderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Fetch all Highlanders from the database
        public List<Highlander> GetAllHighlanders()
        {
            List<Highlander> highlanders = new List<Highlander>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Power, Age, PositionX, PositionY, Type, IsAlive FROM Highlanders";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        int power = reader.GetInt32(1);
                        int age = reader.GetInt32(2);
                        int posX = reader.GetInt32(3);
                        int posY = reader.GetInt32(4);
                        string type = reader.GetString(5);
                        bool isAlive = reader.GetBoolean(6);

                        Highlander highlander;

                        if (type == "Good")
                        {
                            highlander = new GoodHighlander(id, power, age, (posX, posY), isAlive);
                        }
                        else
                        {
                            highlander = new BadHighlander(id, power, age, (posX, posY), isAlive);
                        }

                        highlanders.Add(highlander);
                    }
                }
            }

            return highlanders;
        }

        // Save a list of Highlanders to the database
        public void SaveHighlanders(List<Highlander> highlanders)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                foreach (var highlander in highlanders)
                {
                    string query = "INSERT INTO Highlanders (Id, Power, Age, PositionX, PositionY, Type, IsAlive) " +
                                   "VALUES (@Id, @Power, @Age, @PositionX, @PositionY, @Type, @IsAlive)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", highlander.Id);
                        cmd.Parameters.AddWithValue("@Power", highlander.Power);
                        cmd.Parameters.AddWithValue("@Age", highlander.Age);
                        cmd.Parameters.AddWithValue("@PositionX", highlander.Position.Item1);
                        cmd.Parameters.AddWithValue("@PositionY", highlander.Position.Item2);
                        cmd.Parameters.AddWithValue("@Type", highlander is GoodHighlander ? "Good" : "Bad");
                        cmd.Parameters.AddWithValue("@IsAlive", highlander.IsAlive);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // Update an existing Highlander in the database
        public void UpdateHighlander(Highlander highlander)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE Highlanders SET Power = @Power, Age = @Age, PositionX = @PositionX, PositionY = @PositionY, IsAlive = @IsAlive WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Power", highlander.Power);
                    cmd.Parameters.AddWithValue("@Age", highlander.Age);
                    cmd.Parameters.AddWithValue("@PositionX", highlander.Position.Item1);
                    cmd.Parameters.AddWithValue("@PositionY", highlander.Position.Item2);
                    cmd.Parameters.AddWithValue("@IsAlive", highlander.IsAlive);
                    cmd.Parameters.AddWithValue("@Id", highlander.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Delete a Highlander from the database by Id
        public void DeleteHighlander(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Highlanders WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Add: Save game results for Option 1 (One Highlander left)
        public void SaveGameResultOption1(int winnerId, int totalIterations, List<Victim> victims)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string insertGameQuery = "INSERT INTO Games (OptionType, WinnerID, TotalIterations) OUTPUT INSERTED.GameID VALUES (1, @WinnerID, @TotalIterations)";
                int gameId;

                using (SqlCommand cmd = new SqlCommand(insertGameQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@WinnerID", winnerId);
                    cmd.Parameters.AddWithValue("@TotalIterations", totalIterations);
                    gameId = (int)cmd.ExecuteScalar();
                }

                string insertVictimQuery = "INSERT INTO Victims (GameID, KillerID, PowerAbsorbed, TimeStep) VALUES (@GameID, @KillerID, @PowerAbsorbed, @TimeStep)";

                foreach (var victim in victims)
                {
                    using (SqlCommand cmd = new SqlCommand(insertVictimQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@GameID", gameId);
                        cmd.Parameters.AddWithValue("@KillerID", winnerId);
                        cmd.Parameters.AddWithValue("@PowerAbsorbed", victim.PowerAbsorbed);
                        cmd.Parameters.AddWithValue("@TimeStep", victim.TimeStep);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // Add: Save game results for Option 2 (Simulation ends after set iterations)
        public void SaveGameResultOption2(int totalIterations, int goodHighlandersRemaining, int badHighlandersRemaining)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string insertGameQuery = "INSERT INTO Games (OptionType, TotalIterations, GoodHighlandersRemaining, BadHighlandersRemaining) VALUES (2, @TotalIterations, @GoodRemaining, @BadRemaining)";

                using (SqlCommand cmd = new SqlCommand(insertGameQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@TotalIterations", totalIterations);
                    cmd.Parameters.AddWithValue("@GoodRemaining", goodHighlandersRemaining);
                    cmd.Parameters.AddWithValue("@BadRemaining", badHighlandersRemaining);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Add: Retrieve results for Option 1
        public List<GameResultOption1> GetGameResultsOption1()
        {
            List<GameResultOption1> results = new List<GameResultOption1>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT G.GameID, H.Name AS WinnerName, COUNT(V.VictimID) AS NumberOfVictims, SUM(V.PowerAbsorbed) AS TotalPowerAbsorbed
                    FROM Games G
                    JOIN Highlanders H ON G.WinnerID = H.Id
                    LEFT JOIN Victims V ON V.GameID = G.GameID
                    WHERE G.OptionType = 1
                    GROUP BY G.GameID, H.Name";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        GameResultOption1 result = new GameResultOption1
                        {
                            GameID = reader.GetInt32(0),
                            WinnerName = reader.GetString(1),
                            NumberOfVictims = reader.GetInt32(2),
                            TotalPowerAbsorbed = reader.IsDBNull(3) ? 0 : reader.GetInt32(3)
                        };

                        results.Add(result);
                    }
                }
            }

            return results;
        }

        // Add: Retrieve results for Option 2
        public List<GameResultOption2> GetGameResultsOption2()
        {
            List<GameResultOption2> results = new List<GameResultOption2>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT GameID, TotalIterations, GoodHighlandersRemaining, BadHighlandersRemaining
                    FROM Games
                    WHERE OptionType = 2";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        GameResultOption2 result = new GameResultOption2
                        {
                            GameID = reader.GetInt32(0),
                            TotalIterations = reader.GetInt32(1),
                            GoodHighlandersRemaining = reader.GetInt32(2),
                            BadHighlandersRemaining = reader.GetInt32(3)
                        };

                        results.Add(result);
                    }
                }
            }

            return results;
        }
    }

    // Add: Helper classes for storing the result objects
    public class Victim
    {
        public int PowerAbsorbed { get; set; }
        public int TimeStep { get; set; }
    }

    public class GameResultOption1
    {
        public int GameID { get; set; }
        public string WinnerName { get; set; }
        public int NumberOfVictims { get; set; }
        public int TotalPowerAbsorbed { get; set; }
    }

    public class GameResultOption2
    {
        public int GameID { get; set; }
        public int TotalIterations { get; set; }
        public int GoodHighlandersRemaining { get; set; }
        public int BadHighlandersRemaining { get; set; }
    }
}
