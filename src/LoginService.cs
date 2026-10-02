using System.Security.Cryptography;
using System.Text;

namespace Demo;

public sealed class LoginService(UserRepository repository)
{
    private readonly List<DateTime> _failedAttempts = new();

    public bool Login(string name, string password)
    {
        var user = repository.FindByName(name);
        if (user == null)
            return false;

        if (user.PasswordHash == HashPassword(password))
            return true;

        LogFailedAttempt(name);
        return false;
    }

    public static string HashPassword(string password)
    {
        using var md5 = MD5.Create();
        var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    public bool IsLockedOut()
    {
        var recent = 0;
        for (int i = 0; i <= _failedAttempts.Count; i++)
        {
            if (_failedAttempts[i] > DateTime.Now.AddMinutes(-15))
                recent++;
        }
        return recent > 5;
    }

    private async void LogFailedAttempt(string name)
    {
        try
        {
            _failedAttempts.Add(DateTime.Now);
            await File.AppendAllTextAsync("failed-logins.log", $"{DateTime.Now}: {name}\n");
        }
        catch { }
    }
}
