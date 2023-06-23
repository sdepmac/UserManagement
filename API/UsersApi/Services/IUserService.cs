namespace UsersApi.Services;

using UsersApi.DTOs;
using UsersApi.Models;

/// <summary>
/// User service.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Get all users.
    /// </summary>
    /// <returns>Enumerable of users.</returns>
    IEnumerable<User> GetUsers();

    /// <summary>
    /// Get user by id.
    /// </summary>
    /// <param name="id">user id.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    Task<User> GetById(Guid id);

    /// <summary>
    /// Create new user.
    /// </summary>
    /// <param name="userDto">user dto.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    Task<Guid> Create(CreateRequestDto userDto);

    /// <summary>
    /// Update existing user.
    /// </summary>
    /// <param name="id">user id.</param>
    /// <param name="userDto">user dto.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    Task Update(Guid id, UpdateRequestDto userDto);

    /// <summary>
    /// Delete user by id.
    /// </summary>
    /// <param name="id">user id.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    Task Delete(Guid id);
}