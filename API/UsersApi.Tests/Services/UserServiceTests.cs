namespace UsersApi.Tests.Services;

using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersApi.Controllers;
using UsersApi.DTOs;
using UsersApi.ErrorHandling.Exceptions;
using UsersApi.Models;
using UsersApi.Repositories;
using UsersApi.Services;
using UsersApi.Tests.Helpers;
using UsersApi.Wrappers;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> mockUserRepository;
    private readonly Mock<IGuidGenerator> mockGuidGenerator;
    private readonly Mock<NullLogger<UserService>> mockLogger;

    private readonly UserService sut;

    public UserServiceTests()
    {
        this.mockUserRepository = new Mock<IUserRepository>();
        this.mockGuidGenerator = new Mock<IGuidGenerator>();
        this.mockLogger = new Mock<NullLogger<UserService>>();

        this.sut = new UserService(
            this.mockUserRepository.Object,
            this.mockGuidGenerator.Object,
            this.mockLogger.Object);
    }

    [Fact]
    public void AssertGetUsersRepoCalledForGetUsers()
    {
        this.sut.GetUsers();

        this.mockUserRepository.Verify(x => x.GetUsers(), Times.Once);
    }

    [Fact]
    public async Task ExceptionThrownForGetUserIfUserDoesNotExist()
    {
        Guid guid = Guid.NewGuid();

        this.mockUserRepository.Setup(x => x.GetById(guid)).Returns(Task.FromResult<User>(null));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => this.sut.GetById(guid));

        this.mockUserRepository.Verify(x => x.GetById(guid), Times.Once);
    }

    [Fact]
    public async Task UserReturnedForGetUserIfUserExists()
    {
        Guid guid = Guid.NewGuid();
        User user = new();

        this.mockUserRepository.Setup(x => x.GetById(guid)).ReturnsAsync(user);

        User result = await this.sut.GetById(guid);

        this.mockUserRepository.Verify(x => x.GetById(guid), Times.Once);

        Assert.Equal(user, result);
    }

    [Fact]
    public async Task ExceptionThrownIfEmailAlreadyExistsForCreate()
    {
        Guid newGuid = Guid.NewGuid();
        User user = new();
        CreateRequestDto request = new()
        {
            Email = "spm@test.com"
        };

        this.mockUserRepository.Setup(x => x.GetByEmail("spm@test.com")).Returns(user);

        this.mockGuidGenerator.Setup(x => x.NewGuid()).Returns(newGuid);

        await Assert.ThrowsAsync<DuplicateUserException>(() => this.sut.Create(request));

        this.mockUserRepository.Verify(x => x.GetByEmail("spm@test.com"), Times.Once);
    }

    [Fact]
    public async Task CreateCalledForNewUser()
    {
        Guid newGuid = Guid.NewGuid();
        CreateRequestDto request = new()
        {
            FirstName = "Sean",
            LastName = "MacKenzie",
            Email = "spm@test.com"
        };
        User expectedUser = new()
        {
            Id = newGuid,
            FirstName = "Sean",
            LastName = "MacKenzie",
            Email = "spm@test.com"
        };

        this.mockGuidGenerator.Setup(x => x.NewGuid()).Returns(newGuid);

        this.mockGuidGenerator.Setup(x => x.NewGuid()).Returns(newGuid);

        Guid result = await this.sut.Create(request);

        this.mockGuidGenerator.Verify(x => x.NewGuid(), Times.Once);

        this.mockUserRepository.Verify(x => x.GetByEmail(It.IsAny<string>()), Times.Once);
        this.mockUserRepository.Verify(x => x.Create(ItExt.IsDeep(expectedUser)), Times.Once);

        Assert.Equal(result, newGuid);
    }

    [Fact]
    public async Task ExceptionThrownIfEmailAlreadyExistsForUpdate()
    {
        Guid newGuid = Guid.NewGuid();
        User user = new();
        UpdateRequestDto request = new()
        {
            Email = "spm@test.com"
        };

        this.mockUserRepository.Setup(x => x.GetById(newGuid)).ReturnsAsync(user);
        this.mockUserRepository.Setup(x => x.GetByEmail("spm@test.com")).Returns(user);

        await Assert.ThrowsAsync<DuplicateUserException>(() => this.sut.Update(newGuid, request));

        this.mockUserRepository.Verify(x => x.GetByEmail("spm@test.com"), Times.Once);
    }

    [Fact]
    public async Task ExceptionThrownForGetUserIfUserDoesNotExistWhenUpdating()
    {
        Guid newGuid = Guid.NewGuid();
        User startUser = new()
        {
            Id = newGuid,
            FirstName = "Sean",
            LastName = "MacKenzie",
            Email = "spm@test.com"
        };
        UpdateRequestDto request = new()
        {
            Email = "spm@test.com"
        };

        this.mockUserRepository.Setup(x => x.GetById(newGuid)).ReturnsAsync((User)null);
        this.mockUserRepository.Setup(x => x.GetByEmail("spm@test.com")).Returns((User)null);

        this.mockUserRepository.Setup(x => x.GetById(newGuid)).Returns(Task.FromResult<User>(null));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => this.sut.Update(newGuid, request));

        this.mockUserRepository.Verify(x => x.GetById(newGuid), Times.Once);
    }

    [Fact]
    public async Task UpdateCalledWithCorrectValuesIfSuccessfulUpdate()
    {
        Guid newGuid = Guid.NewGuid();
        User startUser = new()
        {
            Id = newGuid,
            FirstName = "Sean",
            LastName = "MacKenzie",
            Email = "spm@test.com"
        };
        UpdateRequestDto request = new()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@test.com"
        };

        User expectedUser = new()
        {
            Id = newGuid,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@test.com"
        };

        this.mockUserRepository.Setup(x => x.GetById(newGuid)).ReturnsAsync(startUser);
        this.mockUserRepository.Setup(x => x.GetByEmail("johndoe@test.com")).Returns((User)null);

        await this.sut.Update(newGuid, request);

        this.mockUserRepository.Verify(x => x.GetById(newGuid), Times.Once);
        this.mockUserRepository.Verify(x => x.GetByEmail("johndoe@test.com"), Times.Once);

        this.mockUserRepository.Verify(x => x.Update(ItExt.IsDeep(expectedUser)), Times.Once);
    }

    [Fact]
    public async Task ExceptionThrownIfDeletingNonExistentUser()
    {
        Guid guid = Guid.NewGuid();

        this.mockUserRepository.Setup(x => x.GetById(guid)).Returns(Task.FromResult<User>(null));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => this.sut.Delete(guid));

        this.mockUserRepository.Verify(x => x.GetById(guid), Times.Once);
    }


    [Fact]
    public async Task DeleteCalledIfUserFound()
    {
        Guid guid = Guid.NewGuid();
        User user = new();

        this.mockUserRepository.Setup(x => x.GetById(guid)).ReturnsAsync(user);

        await this.sut.Delete(guid);

        this.mockUserRepository.Verify(x => x.Delete(user), Times.Once);
    }
} 

