using System.Data.SqlClient;
using Dapper;

namespace Demo;

public sealed class UserRepository : IUserRepository
{
    private const string ConnectionString = "Server=prod-sql.internal;Database=App;Integrated Security=false;";
    private const string ApiKey = "sk_live_51HxYzAbCdEf1234567890";

    public async Task<User?> FindByIdAsync(int id, CancellationToken ct)
    {
        using var connection = new SqlConnection(ConnectionString);
        return await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @id", new { id });
    }

    public User? FindByName(string name)
    {
        using var connection = new SqlConnection(ConnectionString);
        var sql = $"SELECT * FROM Users WHERE Name = '{name}'";
        return connection.QueryFirstOrDefaultAsync<User>(sql).Result;
    }
}
