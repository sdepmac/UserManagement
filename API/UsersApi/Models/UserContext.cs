namespace UsersApi.Models;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// User context.
/// </summary>
public class UserContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserContext"/> class.
    /// </summary>
    /// <param name="options">context options.</param>
    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets users.
    /// </summary>
    public DbSet<User> Users { get; set; }
}
