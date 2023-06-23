namespace UsersApi.Repositories;

using UsersApi.Models;

/// <summary>
/// User repository.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Get all users.
    /// </summary>
    /// <returns>user enumerable.</returns>
    IEnumerable<User> GetUsers();

    /// <summary>
    /// Get user by id.
    /// </summary>
    /// <param name="id">user id.</param>
    /// <returns>User.</returns>
    Task<User> GetById(Guid id);

    /// <summary>
    /// Create new user.
    /// </summary>
    /// <param name="user">user entity.</param>
    /// <returns>task.</returns>
    Task Create(User user);

    /// <summary>
    /// Update existing object.
    /// </summary>
    /// <param name="user">user entity.</param>
    /// <returns>task.</returns>
    Task Update(User user);

    /// <summary>
    /// Delete user.
    /// </summary>
    /// <param name="user">user entity.</param>
    /// <returns>Task.</returns>
    Task Delete(User user);

    /// <summary>
    /// Get user by email.
    /// </summary>
    /// <param name="email">email.</param>
    /// <returns>user entity.</returns>
    User GetByEmail(string email);
}