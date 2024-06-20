using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace ProjetoFinal.Models;

public class TaskService : HelperBase
{
    public List<Task> List()
    {
        List<Task> tasks = new List<Task>();

        DataTable docs = new DataTable();
        SqlDataAdapter telefone = new SqlDataAdapter();
        SqlCommand comando = new SqlCommand();
        SqlConnection conexao = new SqlConnection(DBConnection);

        comando.CommandType = CommandType.Text;
        comando.CommandText = "SELECT * FROM tTask";
        comando.Connection = conexao;
        telefone.SelectCommand = comando;
        telefone.Fill(docs);

        conexao.Close();
        conexao.Dispose();

        foreach (DataRow linhadoc in docs.Rows)
        {
            Task doc = new Task();
            doc.Id = "" + linhadoc["Id"];
            doc.Title = "" + linhadoc["Name"];
            doc.Description = "" + linhadoc["Color"];
            doc.EstimatedTime = Convert.ToInt32("" + linhadoc["Color"]);
            doc.StageId = "" + linhadoc["Color"];
            tasks.Add(doc);
        }

        return tasks;
    }

    public void Save(Task task)
    {
        if (string.IsNullOrEmpty(task.Id))
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(DBConnection);
            comando.Connection = conexao;
            comando.CommandType = CommandType.Text;
            comando.CommandText = " INSERT INTO tTask (Id, Title, Description, EstimatedTime, DateCreated, User, Stage) " +
                                  " VALUES (@Id, @Title, @Description, @EstimatedTime, @DateCreated, @User, @Stage)";
            comando.Parameters.AddWithValue("@Id", task.Id);
            comando.Parameters.AddWithValue("@Title", task.Title);
            comando.Parameters.AddWithValue("@Description", task.Description);
            comando.Parameters.AddWithValue("@EstimatedTime", task.EstimatedTime);
            comando.Parameters.AddWithValue("@DateCreated", task.DateCreated);
            comando.Parameters.AddWithValue("@User", task.User);
            comando.Parameters.AddWithValue("@Stage", task.Stage);
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
                                  " Description = @Color, " +
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
        Stage? stage = null;

        DataTable docs = new DataTable();
        SqlDataAdapter telefone = new SqlDataAdapter();
        SqlCommand comando = new SqlCommand();
        SqlConnection conexao = new SqlConnection(DBConnection);

        comando.CommandType = CommandType.Text;
        comando.CommandText = "SELECT * FROM tStage WHERE Id=@Id";
        comando.Parameters.AddWithValue("@Id", id);

        comando.Connection = conexao;
        telefone.SelectCommand = comando;
        telefone.Fill(docs);

        conexao.Close();
        conexao.Dispose();

        // if (docs.Rows.Count > 1)
        //     return stage;
        //
        // if (docs.Rows.Count == 0)
        //     return stage;

        DataRow docLine = docs.Rows[0];

        stage = new Stage
        {
            Id = docLine["Id"].ToString(),
            Name = docLine["Name"].ToString(),
            Color = docLine["Color"].ToString(),
            Sort = Convert.ToInt32(docLine["Sort"])
        };

        return new();
    }

    public void Delete(string id)
    {
        SqlCommand comando = new SqlCommand();
        SqlConnection conexao = new SqlConnection(DBConnection);
        comando.Connection = conexao;
        comando.CommandType = CommandType.Text;
        comando.CommandText = "DELETE FROM tStage WHERE Id = @Id";
        comando.Parameters.AddWithValue("@Id", id);
        conexao.Open();
        comando.ExecuteNonQuery();
        conexao.Close();
        conexao.Dispose();
    }
}