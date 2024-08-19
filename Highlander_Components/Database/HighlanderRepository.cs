using System.Data.SqlClient;
using System;
using Highlander_Components.Database;

class HighlanderRepository
{
    // Connection string to the database (modify with your details)
    //    private static string connectionString = "Server=./;Database=Highlanders;User=UserHighlanders;Password=12345";
    private static SqlConnection connection = new DatabaseContext().GetConnection();
    private static SqlCommand command;
    // Create: Insert a new record
    public static int AddRecord(string winner, int victimsNum, int totalPower, int goodRemaining, int badRemaining)
    {
        try
        {
            Console.WriteLine("Add record called");
            connection.Open();
            string query = "INSERT INTO game_results (winner, victims_num, total_power, good_remaining, bad_remaining) " +
                           "VALUES (@Winner, @VictimsNum, @TotalPower, @GoodRemaining, @BadRemaining); " +
                           "SELECT SCOPE_IDENTITY();";
            command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Winner", winner);
            command.Parameters.AddWithValue("@VictimsNum", victimsNum);
            command.Parameters.AddWithValue("@TotalPower", totalPower);
            command.Parameters.AddWithValue("@GoodRemaining", goodRemaining);
            command.Parameters.AddWithValue("@BadRemaining", badRemaining);

            int id = Convert.ToInt32(command.ExecuteScalar());
            return id;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return -1;
        }
        finally
        {
            connection.Close();
        }
    }

    // Read: Display all records
    public static void RefreshData()
    {
        try
        {
            connection.Open();
            string query = "SELECT * FROM game_results";
            command = new SqlCommand(query, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["id"]}, Winner: {reader["winner"]}, VictimsNum: {reader["victims_num"]}, " +
                                  $"TotalPower: {reader["total_power"]}, GoodRemaining: {reader["good_remaining"]}, BadRemaining: {reader["bad_remaining"]}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }

    // Update: Update an existing record
    public static void UpdateRecord(int id, string winner, int victimsNum, int totalPower, int goodRemaining, int badRemaining)
    {
        try
        {
            connection.Open();
            string query = "UPDATE game_results SET winner = @Winner, victims_num = @VictimsNum, total_power = @TotalPower, " +
                           "good_remaining = @GoodRemaining, bad_remaining = @BadRemaining WHERE id = @Id";
            command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Winner", winner);
            command.Parameters.AddWithValue("@VictimsNum", victimsNum);
            command.Parameters.AddWithValue("@TotalPower", totalPower);
            command.Parameters.AddWithValue("@GoodRemaining", goodRemaining);
            command.Parameters.AddWithValue("@BadRemaining", badRemaining);

            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }

    // Delete: Delete a record
    public static void DeleteRecord(int id)
    {
        try
        {
            connection.Open();
            string query = "DELETE FROM game_results WHERE id = @Id";
            command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }
}