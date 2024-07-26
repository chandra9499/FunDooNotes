using BusinessLogicLayer.Interface;
using DataBaseLogicLayer.Context;
using DataBaseLogicLayer.Interface;
using FunDooNotes.Controllers;
using Microsoft.EntityFrameworkCore;
using DataBaseLogicLayer.Repository;
using DataBaseLogicLayer.Helper;
using Microsoft.Extensions.Configuration;
using BusinessLogicLayer.Service;
using Model.Models.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using Model.Models.Utility;
using Model.Models.Entity;

namespace Testing;

[TestFixture]
public class Tests
{
    private FunDooDataBaseContext _context;
    private IUserDAL userDAL;
    private IUserBLL userBLL;
    private UserController userController;
    [SetUp]
    public void Setup()
    {
        var configaration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var option = new DbContextOptionsBuilder<FunDooDataBaseContext>()
            .UseSqlServer(configaration.GetConnectionString("FunDooConnection_Test"))
            .Options;

        _context = new FunDooDataBaseContext(option);
        _context.Database.EnsureCreated();

        userDAL = new UsarDAL(_context, new TokenGenarator(configaration));
        userBLL = new UserBLL(userDAL);
        userController = new UserController(userBLL);
    }
    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public void RegisterUser_ReturnOkStatus()
    {
        //Arrange
        var registerUser = new RegisterUserModel()
        {
            FirstName = "chandra",
            LastName = "c",
            Email = "chandra@gmail.com",
            Password = "chandraS@123"
        };
        //Act
        var result = userController.RegisterUser(registerUser);
        //Assert
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okObject = result as OkObjectResult;
        var responceModel = okObject.Value as ResponseModel<User>;

        Assert.IsNotNull(responceModel);
        Assert.AreEqual(registerUser.Email, responceModel.Data.Email);
    }
    [Test]
    public void Registering_AllReadyExstingUSer()
    {
        //arrage
        var registerUserModel = new User()
        {
            FirstName = "akhil",
            LastName = "B",
            Email = "akhil@gmail.com",
            Password = "akhil@123"
        };
        _context.Add(registerUserModel);
        _context.SaveChanges();
        var exstingUserModel = new RegisterUserModel()
        {
            FirstName = "akhil",
            LastName = "B",
            Email = "akhil@gmail.com",
            Password = "akhil@123"
        };
        //act
        var result = userController.RegisterUser(exstingUserModel);

        //assert
        Assert.IsInstanceOf<ConflictObjectResult>(result);

        var okObject = result as ConflictObjectResult;

        var responceModel = okObject.Value as ResponseModel<User>;
        Assert.IsNotNull(responceModel);
        Assert.AreEqual("The Email is already registered", responceModel.Message);
    }
}