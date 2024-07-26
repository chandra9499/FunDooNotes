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
            .UseSqlServer(configaration.GetConnectionString("FunDooConnection"))
            .Options;

        _context = new FunDooDataBaseContext(option);
        _context.Database.EnsureCreated();

        userDAL = new UsarDAL(_context,new TokenGenarator(configaration));
        userBLL = new UserBLL(userDAL);
        userController = new UserController(userBLL);
    }

    [Test]
    public void RegisterUser_ReturnOkStatus()
    {
        //Act
        var registerUser = new RegisterUserModel()
        {
            FirstName = "komal",
            LastName = "K",
            Email = "komal@gmail.com",
            Password = "komal@123"
        };
        //Arrange
        var result = userController.RegisterUser(registerUser);
        //Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        
        var okObject = result as OkObjectResult;

        var responceModel = okObject.Value as ResponseModel<User>;

        Assert.IsNotNull(responceModel);
        Assert.AreEqual(registerUser.Email,responceModel.Data.Email);
    }
    [Test]
    public void Registering_AllReadyExstingUSer()
    {
        var registerUserModel = new RegisterUserModel()
        {
            FirstName = "pooja",
            LastName = "k",
            Email = "pooja@gmail.com",
            Password = "pooja@123"
        };
        var result = userController.RegisterUser(registerUserModel);

        Assert.IsInstanceOf<ConflictObjectResult>(result);
        
        var okObject = result as ConflictObjectResult;

        var responceModel = okObject.Value as ResponseModel<User>;
        Assert.IsNotNull(responceModel);
        Assert.AreEqual("The Email is already registered",responceModel.Message);
    }
}