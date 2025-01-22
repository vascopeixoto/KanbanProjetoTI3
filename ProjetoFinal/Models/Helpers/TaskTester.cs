using System.Data;
using System.Data.SqlClient;

namespace ProjetoFinal.Models;

public class TaskTester : HelperBase
{
    Random r;

    public TaskTester()
    {
        r = new Random();
    }

    public string Criar(Task task)
    {
        string retID = "";
        try
        {
            SqlCommand sqlC = new SqlCommand();
            //----
            sqlC.Connection = new SqlConnection(DBConnection);
            sqlC.Connection.Open();
            sqlC.CommandType = CommandType.StoredProcedure;
            sqlC.CommandText = "AddTask";
            sqlC.Parameters.Add("@Id", SqlDbType.VarChar, 255).Value = task.Id;
            sqlC.Parameters.Add("@Title", SqlDbType.VarChar, 255).Value = task.Title;
            sqlC.Parameters.Add("@Description", SqlDbType.Text).Value = task.Description;
            sqlC.Parameters.Add("@EstimatedTime", SqlDbType.Int).Value = task.EstimatedTime;
            sqlC.Parameters.Add("@DateCreated", SqlDbType.DateTime).Value = task.DateCreated;
            sqlC.Parameters.Add("@UserId", SqlDbType.VarChar, 255).Value = task.UserId;
            sqlC.Parameters.Add("@StageId", SqlDbType.VarChar, 255).Value = task.StageId;

            sqlC.ExecuteNonQuery();

            retID = task.Id;
            sqlC.Connection.Close();
            sqlC.Connection.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERRO: " + ex.Message); //Nota--> Escrever em log de erros e não na console
            retID = "";
        }

        return retID;
    }

    public string Atualizar(Task task)
    {
        string retID = "";
        try
        {
            SqlCommand sqlC = new SqlCommand();
            //----
            sqlC.Connection = new SqlConnection(DBConnection);
            sqlC.Connection.Open();
            sqlC.CommandType = CommandType.StoredProcedure;
            sqlC.CommandText = "UpdateTask";
            sqlC.Parameters.Add("@Id", SqlDbType.VarChar, 255).Value = task.Id;
            sqlC.Parameters.Add("@Title", SqlDbType.VarChar, 255).Value = task.Title;
            sqlC.Parameters.Add("@Description", SqlDbType.Text).Value = task.Description;
            sqlC.Parameters.Add("@EstimatedTime", SqlDbType.Int).Value = task.EstimatedTime;
            sqlC.Parameters.Add("@UserId", SqlDbType.VarChar, 255).Value = task.UserId;
            sqlC.Parameters.Add("@StageId", SqlDbType.VarChar, 255).Value = task.StageId;
            sqlC.ExecuteNonQuery();

            sqlC.Connection.Close();
            sqlC.Connection.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERRO: " + ex.Message);
            retID = "";
        }

        return retID;
    }

    public List<Task> GetRandom10PercentTask()
    {
        List<Task> retList = new List<Task>();
        DataTable dtC = new DataTable();
        SqlDataAdapter SqlA = new SqlDataAdapter();
        //--
        try
        {
            SqlA.SelectCommand = new SqlCommand();
            SqlA.SelectCommand.Connection = new SqlConnection(DBConnection);
            SqlA.SelectCommand.Connection.Open();
            SqlA.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlA.SelectCommand.CommandText = "QTester_GetTop10Percent_Task";
            //---
            SqlA.Fill(dtC);
            //--- 
            SqlA.SelectCommand.Connection.Close();
            SqlA.SelectCommand.Connection.Dispose();
        }
        catch (Exception ex)
        {
            dtC = null;
        }

        if (dtC != null)
        {
            foreach (DataRow r in dtC.Rows)
            {
                Task m = new Task();
                m.Id = r["Id"].ToString();
                m.Title = r["Title"].ToString();
                m.Description = r["Description"].ToString();
                m.StageId = r["StageId"].ToString();
                m.UserId = r["UserId"].ToString();
                m.Stage = new Stage { Name = r["StageName"].ToString() };
                m.User = new User { Name = r["UserName"].ToString() };
                m.EstimatedTime = Convert.ToInt32(r["EstimatedTime"]);
                retList.Add(m);
            }
        }

        return retList;
    }

    public string geraDescricao()
    {
        int tamanho = r.Next(2, 20);
        int contador = 0;
        string Descricao = "";
        while (contador < tamanho)
        {
            Descricao += " " + geraNomeSimples(r.Next(3, 8));
            contador++;
        }

        return Descricao;
    }

    public string geraTitle()
    {
        int tamanho = r.Next(2, 4);
        int contador = 0;
        string Title = "";
        while (contador < tamanho)
        {
            Title += " " + geraNomeSimples(r.Next(3, 8));
            contador++;
        }

        return Title;
    }

    private string geraNomeSimples(int comprimento)
    {
        string[] consoantes =
        {
            "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v",
            "w", "x"
        };
        string[] vogais = { "a", "e", "i", "o", "u", "ão", "ou", "io", "ui" };
        string Name = "";
        Name += consoantes[r.Next(consoantes.Length)].ToUpper();
        Name += vogais[r.Next(vogais.Length)];
        int b = 2;
        while (b < comprimento)
        {
            Name += consoantes[r.Next(consoantes.Length)];
            b++;
            Name += vogais[r.Next(vogais.Length)];
            b++;
        }

        return Name;
    }

    public int GeraEstimatedDate()
    {
        return r.Next(1, 99);
    }
}