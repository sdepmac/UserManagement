namespace UsersApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using UsersApi.DTOs;
using UsersApi.Models;
using UsersApi.Services;

/// <summary>
/// Users Controller.
/// </summary>
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService userService;
    private readonly ILogger<UsersController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="userService">user service.</param>
    /// <param name="logger">logger.</param>
    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        this.userService = userService;
        this.logger = logger;
    }

    /// <summary>
    /// Endpoint to Get all users.
    /// </summary>
    /// <returns>List of Users.</returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        this.logger.LogDebug("Get All Users Request Received");

        IEnumerable<User> users = this.userService.GetUsers();
        return this.Ok(users);
    }

    /// <summary>
    /// Get user by id.
    /// </summary>
    /// <param name="id">id.</param>
    /// <returns>User.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        this.logger.LogDebug("Get User Request Received");

        var user = await this.userService.GetById(id);
        return this.Ok(user);
    }

    /// <summary>
    /// Create new user.
    /// </summary>
    /// <param name="requestDto">request object.</param>
    /// <returns>Result containing id.</returns>
    [HttpPost]
    public async Task<IActionResult> Create(CreateRequestDto requestDto)
    {
        this.logger.LogDebug("Create User Request Received");

        Guid id = await this.userService.Create(requestDto);
        return this.Ok(id);
    }

    /// <summary>
    /// Update existing user.
    /// </summary>
    /// <param name="id">user id.</param>
    /// <param name="requestDto">request object.</param>
    /// <returns>Result.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateRequestDto requestDto)
    {
        this.logger.LogDebug("Update User Request Received");

        await this.userService.Update(id, requestDto);
        return this.Ok();
    }

    /// <summary>
    /// Delete user by id.
    /// </summary>
    /// <param name="id">user id.</param>
    /// <returns>result.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        this.logger.LogDebug("Delete User Request Received");

        await this.userService.Delete(id);
        return this.Ok();
    }
}
