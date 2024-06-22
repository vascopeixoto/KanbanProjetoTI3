using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace ProjetoFinal.Models;

public class StagesService : HelperBase
{
    public List<Stage> List(string userId)
    {
        List<Stage> stages = new List<Stage>();

        DataTable docs = new DataTable();
        SqlDataAdapter telefone = new SqlDataAdapter();
        SqlCommand comando = new SqlCommand();
        SqlConnection conexao = new SqlConnection(DBConnection);

        comando.CommandType = CommandType.Text;
        comando.CommandText = "SELECT * FROM tStage WHERE UserId = @UserId";
        comando.Parameters.AddWithValue("@UserId", userId);
        comando.Connection = conexao;
        telefone.SelectCommand = comando;
        telefone.Fill(docs);

        conexao.Close();
        conexao.Dispose();

        foreach (DataRow linhadoc in docs.Rows)
        {
            Stage doc = new Stage();
            doc.Id = "" + linhadoc["Id"];
            doc.Name = "" + linhadoc["Name"];
            doc.Color = "" + linhadoc["Color"];
            doc.Sort = Convert.ToInt32(linhadoc["Sort"]);
            stages.Add(doc);
        }

        return stages;
    }

    public void Save(Stage stage)
    {
        var id = Guid.NewGuid().ToString();

        if (string.IsNullOrEmpty(stage.Id))
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(DBConnection);
            comando.Connection = conexao;
            comando.CommandType = CommandType.Text;
            comando.CommandText = " INSERT INTO tStage (Id, Name, Color, Sort, UserId) " +
                                  " VALUES (@Id, @Name, @Color, @Sort, @UserId)";
            comando.Parameters.AddWithValue("@Id", id);
            comando.Parameters.AddWithValue("@Name", stage.Name);
            comando.Parameters.AddWithValue("@Color", stage.Color);
            comando.Parameters.AddWithValue("@Sort", stage.Sort);
            comando.Parameters.AddWithValue("@UserId", stage.UserId);
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
            comando.CommandText = " UPDATE tStage " +
                                  " SET Name = @Name, " +
                                  " Color = @Color, " +
                                  " Sort = @Sort " +
                                  " WHERE Id = @Id ";
            comando.Parameters.AddWithValue("@Id", stage.Id);
            comando.Parameters.AddWithValue("@Name", stage.Name);
            comando.Parameters.AddWithValue("@Color", stage.Color);
            comando.Parameters.AddWithValue("@Sort", stage.Sort);
            conexao.Open();
            comando.ExecuteNonQuery();
            conexao.Close();
            conexao.Dispose();
        }
    }

    public Stage? GetById(string id)
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

        if (docs.Rows.Count > 1)
            return stage;

        if (docs.Rows.Count == 0)
            return stage;

        DataRow docLine = docs.Rows[0];

        stage = new Stage
        {
            Id = docLine["Id"].ToString(),
            Name = docLine["Name"].ToString(),
            Color = docLine["Color"].ToString(),
            Sort = Convert.ToInt32(docLine["Sort"])
        };

        return stage;
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