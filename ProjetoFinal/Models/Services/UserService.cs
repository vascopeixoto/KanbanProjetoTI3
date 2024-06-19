using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace ProjetoFinal.Models;

public class UserService : HelperBase
{
    private Encryptor encryptor;

    public UserService()
    {
        encryptor = new Encryptor(Program.Key, Program.IV);
    }
    
    public User SetGuest()
    {
        return new User
        {
            Id = Guid.Empty.ToString(),
            Name = "Anónimo",
            Email = "",
            AccessLevel = 0
        };
    }
    
    public User? GetBySession(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            return SetGuest();
        
        var userId = encryptor.Decrypt(hash);
        User? user;

        DataTable docs = new DataTable();
        SqlDataAdapter telefone = new SqlDataAdapter();
        SqlCommand comando = new SqlCommand();
        SqlConnection conexao = new SqlConnection(DBConnection);

        comando.CommandType = CommandType.Text;
        comando.CommandText = "SELECT * FROM tUser WHERE Id = @UserId";
        comando.Parameters.AddWithValue("@UserId", userId);
        comando.Connection = conexao;
        telefone.SelectCommand = comando;
        telefone.Fill(docs);

        conexao.Close();
        conexao.Dispose();

        user = SetGuest();;

        if (docs.Rows.Count > 1)
            return user;

        DataRow docLine = docs.Rows[0];

        user = new User
        {
            Id = docLine["Id"].ToString(),
            Name = docLine["Name"].ToString(),
            Email = docLine["Email"].ToString(),
            AccessLevel = Convert.ToInt32(docLine["AccessLevel"])
        };

        return user;
    }

    public User? GetByEmail(string email)
    {
        User? user = SetGuest();

        if (string.IsNullOrWhiteSpace(email))
            return user;

        DataTable docs = new DataTable();
        SqlDataAdapter telefone = new SqlDataAdapter();
        SqlCommand comando = new SqlCommand();
        SqlConnection conexao = new SqlConnection(DBConnection);

        comando.CommandType = CommandType.Text;
        comando.CommandText = "SELECT * FROM tUser WHERE Email = @Email";
        comando.Parameters.AddWithValue("@Email", email);
        comando.Connection = conexao;
        telefone.SelectCommand = comando;
        telefone.Fill(docs);

        conexao.Close();
        conexao.Dispose();


        if (docs.Rows.Count > 1)
            return user;
        
        if (docs.Rows.Count == 0)
            return user;
        
        DataRow docLine = docs.Rows[0];

        user = new User
        {
            Id = docLine["Id"].ToString(),
            Name = docLine["Name"].ToString(),
            Email = docLine["Email"].ToString(),
            Pass = docLine["Pass"].ToString(),
            AccessLevel = Convert.ToInt32(docLine["AccessLevel"])
        };

        return user;
    }

    public bool RegistUser(UserRegist newUser)
    {
        SqlCommand comando = new SqlCommand();
        SqlConnection conexao = new SqlConnection(DBConnection);
        comando.Connection = conexao;

        comando.CommandType = CommandType.Text;
        comando.CommandText =
            "INSERT INTO tUser (Id, Name, Email, Pass, AccessLevel) VALUES (@Id, @Name, @Email, @Pass, @AccessLevel)";
        comando.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
        comando.Parameters.AddWithValue("@Name", newUser.Name);
        comando.Parameters.AddWithValue("@Email", newUser.Email);
        comando.Parameters.AddWithValue("@Pass", newUser.Pass);
        comando.Parameters.AddWithValue("@AccessLevel", 1);
        
        try
        {
            conexao.Open();
            comando.ExecuteNonQuery();
            conexao.Close();
            conexao.Dispose();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}