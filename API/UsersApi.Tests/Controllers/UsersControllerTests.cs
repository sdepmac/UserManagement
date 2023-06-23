namespace UsersApi.Tests.Controllers;

using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersApi.Controllers;
using UsersApi.DTOs;
using UsersApi.Models;
using UsersApi.Services;

public class UsersControllerTests
{
    private readonly Mock<IUserService> mockUserService;
    private readonly Mock<NullLogger<UsersController>> mockLogger;
    private readonly UsersController controllerUnderTest;

    private readonly List<User> users =  new()
        {
            new()
            {
                FirstName = "Sean-Paul",
                LastName = "MacKenzie",
                Email = "sdepmac@icloud.com"
            },
            new()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "jd@icloud.com"
            },
        };

    public UsersControllerTests()
    {
        this.mockUserService = new Mock<IUserService>();
        this.mockLogger = new Mock<NullLogger<UsersController>>();

        this.controllerUnderTest = new UsersController(
            this.mockUserService.Object,
            this.mockLogger.Object
            );
    }

    [Fact]
    public void AssertGetUsersCalledWhenGetAllRequestRecieved()
    {
        this.mockUserService.Setup(x => x.GetUsers()).Returns(users);

        IActionResult result = this.controllerUnderTest.GetAll();

        this.mockUserService.Verify(x => x.GetUsers(), Times.Once);

        Assert.NotNull(result);
        Assert.True(result is OkObjectResult);

        ObjectResult okObjectResult = (ObjectResult)result;

        okObjectResult.Value.Should().BeEquivalentTo(users);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
    }

    [Fact]
    public async Task AssertGetByIdIsCalledWhenGetByIdReequestIsRecieved()
    {
        Guid testGuid = Guid.NewGuid();

        this.mockUserService.Setup(x => x.GetById(testGuid)).ReturnsAsync(users.First());

        IActionResult result = await this.controllerUnderTest.Get(testGuid);

        this.mockUserService.Verify(x => x.GetById(testGuid), Times.Once);

        Assert.NotNull(result);
        Assert.True(result is OkObjectResult);

        ObjectResult okObjectResult = (ObjectResult)result;

        okObjectResult.Value.Should().BeEquivalentTo(users.First());
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
    }

    [Fact]
    public async Task AssertCreateUserCallsCreateServiceAndReturnsId()
    {
        Guid testGuid = Guid.NewGuid();

        CreateRequestDto createRequestDto = new CreateRequestDto();

        this.mockUserService.Setup(x => x.Create(createRequestDto)).ReturnsAsync(testGuid);

        IActionResult result = await this.controllerUnderTest.Create(createRequestDto);

        this.mockUserService.Verify(x => x.Create(createRequestDto), Times.Once);

        Assert.NotNull(result);
        Assert.True(result is OkObjectResult);

        ObjectResult okObjectResult = (ObjectResult)result;

        okObjectResult.Value.Should().BeEquivalentTo(testGuid);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
    }

    [Fact]
    public async Task AssertUpdateCallsUpdate()
    {
        Guid testGuid = Guid.NewGuid();

        UpdateRequestDto udpateRequestDto = new();

        this.mockUserService.Setup(x => x.Update(testGuid, udpateRequestDto));

        IActionResult result = await this.controllerUnderTest.Update(testGuid, udpateRequestDto);

        this.mockUserService.Verify(x => x.Update(testGuid, udpateRequestDto), Times.Once);

        Assert.NotNull(result);
        Assert.True(result is OkResult);

        OkResult okObjectResult = (OkResult)result;
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
    }

    [Fact]
    public async Task AssertDeleteCallsDelete()
    {
        Guid testGuid = Guid.NewGuid();

        this.mockUserService.Setup(x => x.Delete(testGuid));

        IActionResult result = await this.controllerUnderTest.Delete(testGuid);

        this.mockUserService.Verify(x => x.Delete(testGuid), Times.Once);

        Assert.NotNull(result);
        Assert.True(result is OkResult);

        OkResult okObjectResult = (OkResult)result;
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
    }
}
