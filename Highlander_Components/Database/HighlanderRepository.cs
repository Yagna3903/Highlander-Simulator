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
    }
}