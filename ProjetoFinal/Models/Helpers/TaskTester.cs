using System.Data.SqlClient;

namespace ProjetoFinal.Models;

public class TaskTester
{
    private readonly string _connectionString;

    public TaskTester(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void TestInsertTask()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("InsertTask", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
            command.Parameters.AddWithValue("@Title", "Test Task");
            command.Parameters.AddWithValue("@Description", "This is a test task.");
            command.Parameters.AddWithValue("@EstimatedTime", 5);
            command.Parameters.AddWithValue("@StageId", Guid.NewGuid().ToString());

            connection.Open();
            command.ExecuteNonQuery();
            Console.WriteLine("Task inserted successfully.");
        }
    }

    public void TestGetAllTasks()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("GetAllTasks", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine(
                        $"Id: {reader["Id"]}, Title: {reader["Title"]}, Description: {reader["Description"]}");
                }
            }
        }
    }

    public void TestUpdateTask(string taskId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("UpdateTask", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Id", taskId);
            command.Parameters.AddWithValue("@Title", "Updated Task");
            command.Parameters.AddWithValue("@Description", "This task has been updated.");
            command.Parameters.AddWithValue("@EstimatedTime", 10);
            command.Parameters.AddWithValue("@StageId", Guid.NewGuid().ToString());

            connection.Open();
            command.ExecuteNonQuery();
            Console.WriteLine("Task updated successfully.");
        }
    }

    public void TestDeleteTask(string taskId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("DeleteTask", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Id", taskId);

            connection.Open();
            command.ExecuteNonQuery();
            Console.WriteLine("Task deleted successfully.");
        }
    }
}