namespace Demo;

public sealed class UserService(IUserRepository repository)
{
    public async Task<User?> GetUserAsync(int id, CancellationToken ct) =>
        await repository.FindByIdAsync(id, ct);
}

public interface IUserRepository
{
    Task<User?> FindByIdAsync(int id, CancellationToken ct);
}
