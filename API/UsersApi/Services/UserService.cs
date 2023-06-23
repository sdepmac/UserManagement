namespace UsersApi.Services;

using UsersApi.DTOs;
using UsersApi.ErrorHandling.Exceptions;
using UsersApi.Models;
using UsersApi.Repositories;
using UsersApi.Wrappers;

/// <inheritdoc/>
public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IGuidGenerator guidGenerator;
    private readonly ILogger<UserService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="userRepository">user repository.</param>
    /// <param name="guidGenerator">guid generator.</param>
    /// <param name="logger">logger.</param>
    public UserService(IUserRepository userRepository, IGuidGenerator guidGenerator, ILogger<UserService> logger)
    {
        this.userRepository = userRepository;
        this.guidGenerator = guidGenerator;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public IEnumerable<User> GetUsers()
    {
        return this.userRepository.GetUsers();
    }

    /// <inheritdoc/>
    public async Task<User> GetById(Guid id)
    {
        User user = await this.GetUserByIdOrThrow(id);

        return user;
    }

    /// <inheritdoc/>
    public async Task<Guid> Create(CreateRequestDto userDto)
    {
         this.ThrowIfUserExists(userDto.Email);

        // would consider moving this to a mapper service in larger more complex cases
         User user = new ()
        {
            Id = this.guidGenerator.NewGuid(),
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
        };

         await this.userRepository.Create(user);

         return user.Id;
    }

    /// <inheritdoc/>
    public async Task Update(Guid id, UpdateRequestDto userDto)
    {
        User user = await this.GetUserByIdOrThrow(id);

        // again would consider using something like automapper
        // with a non null update rules if the object was more complex
        if (userDto.Email is not null && user.Email != userDto.Email)
        {
            this.ThrowIfUserExists(userDto.Email);
            user.Email = userDto.Email;
        }

        if (userDto.FirstName is not null)
        {
            user.FirstName = userDto.FirstName;
        }

        if (userDto.LastName is not null)
        {
            user.LastName = userDto.LastName;
        }

        await this.userRepository.Update(user);
    }

    /// <inheritdoc/>
    public async Task Delete(Guid id)
    {
        User user = await this.GetUserByIdOrThrow(id);

        await this.userRepository.Delete(user);
    }

    private async Task<User> GetUserByIdOrThrow(Guid id)
    {
        User user = await this.userRepository.GetById(id);

        if (user == null)
        {
            this.logger.LogError($"User With ID {id} not found");
            throw new KeyNotFoundException("User Not Found");
        }

        return user;
    }

    private void ThrowIfUserExists(string email)
    {
        User user = this.userRepository.GetByEmail(email);

        if (user is not null)
        {
            this.logger.LogError($"User With email {email} already exists");
            throw new DuplicateUserException();
        }
    }

    private void ThrowIfUserNot(string email)
    {
        User user = this.userRepository.GetByEmail(email);

        if (user is not null)
        {
            this.logger.LogError($"User With email {email} already exists");
            throw new DuplicateUserException();
        }
    }
}
