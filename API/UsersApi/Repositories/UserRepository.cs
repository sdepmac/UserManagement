namespace UsersApi.Repositories;

using UsersApi.Models;

/// <inheritdoc/>
public class UserRepository : IUserRepository
{
    private readonly UserContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">database context.</param>
    public UserRepository(UserContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public IEnumerable<User> GetUsers()
    {
        return this.context.Users;
    }

    /// <inheritdoc/>
    public async Task<User> GetById(Guid id)
    {
        return await this.context.Users.FindAsync(id);
    }

    /// <inheritdoc/>
    public async Task Create(User user)
    {
        await this.context.Users.AddAsync(user);
        await this.context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task Update(User user)
    {
        this.context.Users.Update(user);
        await this.context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task Delete(User user)
    {
        this.context.Users.Remove(user);
        await this.context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public User GetByEmail(string email)
    {
        return this.context.Users.FirstOrDefault(x => x.Email == email);
    }
}
