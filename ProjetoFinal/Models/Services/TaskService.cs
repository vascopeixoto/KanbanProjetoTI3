using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace ProjetoFinal.Models;

public class TaskService : HelperBase
{
    public List<Task> List(string userId)
    {
        List<Task> tasks = new List<Task>();

        DataTable docs = new DataTable();
        SqlDataAdapter telefone = new SqlDataAdapter();
        SqlCommand comando = new SqlCommand();
        SqlConnection conexao = new SqlConnection(DBConnection);

        comando.CommandType = CommandType.Text;
        comando.CommandText = "SELECT * FROM tTask WHERE UserId = @UserId";
        comando.Parameters.AddWithValue("@UserId", userId);
        comando.Connection = conexao;
        telefone.SelectCommand = comando;
        telefone.Fill(docs);

        conexao.Close();
        conexao.Dispose();

        foreach (DataRow linhadoc in docs.Rows)
        {
            Task doc = new Task();
            doc.Id = "" + linhadoc["Id"];
            doc.Title = "" + linhadoc["Title"];
            doc.Description = "" + linhadoc["Description"];
            doc.EstimatedTime = Convert.ToInt32("" + linhadoc["EstimatedTime"]);
            doc.StageId = "" + linhadoc["StageId"];
            tasks.Add(doc);
        }

        return tasks;
    }

    public void Save(Task task)
    {
        var id = Guid.NewGuid().ToString();
        if (string.IsNullOrEmpty(task.Id))
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(DBConnection);
            comando.Connection = conexao;
            comando.CommandType = CommandType.Text;
            comando.CommandText = " INSERT INTO tTask (Id, Title, Description, EstimatedTime, DateCreated, UserId, StageId) " +
                                  " VALUES (@Id, @Title, @Description, @EstimatedTime, @DateCreated, @UserId, @StageId)";
            comando.Parameters.AddWithValue("@Id", id);
            comando.Parameters.AddWithValue("@Title", task.Title);
            comando.Parameters.AddWithValue("@Description", task.Description);
            comando.Parameters.AddWithValue("@EstimatedTime", task.EstimatedTime);
            comando.Parameters.AddWithValue("@DateCreated", task.DateCreated);
            comando.Parameters.AddWithValue("@UserId", task.User.Id);
            comando.Parameters.AddWithValue("@StageId", task.Stage.Id);
            conexao.Open();
            comando.ExecuteNonQuery();
            conexao.Close();
            conexao.Dispose();
        }
        else
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(DBConnection);
            comando.Connection = conexao;
            comando.CommandType = CommandType.Text;
            comando.CommandText = " UPDATE tTask " +
                                  " SET Title = @Title, " +
                                  " Description = @Description, " +
                                  " EstimatedTime = @EstimatedTime " +
                                  " WHERE Id = @Id ";
            comando.Parameters.AddWithValue("@Id", task.Id);
            comando.Parameters.AddWithValue("@Title", task.Title);
            comando.Parameters.AddWithValue("@Description", task.Description);
            comando.Parameters.AddWithValue("@EstimatedTime", task.EstimatedTime);
            conexao.Open();
            comando.ExecuteNonQuery();
            conexao.Close();
            conexao.Dispose();
        }
    }

    public Task? GetById(string id)
    {
        Task? task = null;

        DataTable docs = new DataTable();
        SqlDataAdapter telefone = new SqlDataAdapter();
        SqlCommand comando = new SqlCommand();
        SqlConnection conexao = new SqlConnection(DBConnection);

        comando.CommandType = CommandType.Text;
        comando.CommandText = "SELECT * FROM tTask WHERE Id=@Id";
        comando.Parameters.AddWithValue("@Id", id);

        comando.Connection = conexao;
        telefone.SelectCommand = comando;
        telefone.Fill(docs);

        conexao.Close();
        conexao.Dispose();

        if (docs.Rows.Count > 1)
            return task;
        
        if (docs.Rows.Count == 0)
            return task;

        DataRow docLine = docs.Rows[0];

        task = new Task
        {
            Id = docLine["Id"].ToString(),
            Title = docLine["Title"].ToString(),
            Description = docLine["Description"].ToString(),
            EstimatedTime = Convert.ToInt32(docLine["EstimatedTime"])
        };

        return task;
    }

    public void Delete(string id)
    {
        SqlCommand comando = new SqlCommand();
        SqlConnection conexao = new SqlConnection(DBConnection);
        comando.Connection = conexao;
        comando.CommandType = CommandType.Text;
        comando.CommandText = "DELETE FROM tTask WHERE Id = @Id";
        comando.Parameters.AddWithValue("@Id", id);
        conexao.Open();
        comando.ExecuteNonQuery();
        conexao.Close();
        conexao.Dispose();
    }
}