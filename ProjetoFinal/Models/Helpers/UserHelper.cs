using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace ProjetoFinal.Models;

public class UserHelper : HelperBase
{
    private Encryptor encryptor;
    private UserService userService;

    public UserHelper()
    {
        encryptor = new Encryptor(Program.Key, Program.IV);
        userService = new UserService();
    }

    public string AuthUser(UserLogin model)
    {
        var user = userService.GetByEmail(model.Email);
        if (user is not null)
        {
            if (user.Email.ToLower() == model.Email.ToLower())
            {
                if (encryptor.Decrypt(user.Pass) == model.Pass)
                {
                    return encryptor.Encrypt(user.Id);
                }
            }
        }

        return "";
    }

    public string RegistUser(UserRegist model)
    {
        var user = userService.GetByEmail(model.Email);
        if (user.Id != Guid.Empty.ToString())
            return "";

        if (model.Pass == model.ConfirmPass)
        {
            model.Pass = encryptor.Encrypt(model.Pass);
            if (userService.RegistUser(model))
            {
                user = userService.GetByEmail(model.Email);
                if (user is not null)
                {
                    return encryptor.Encrypt(user.Id);
                }
            }
        }

        return "";
    }
}