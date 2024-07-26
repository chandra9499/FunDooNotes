//using BusinessLogicLayer.Interface;
//using DataBaseLogicLayer.Context;
//using DataBaseLogicLayer.Interface;
//using FunDooNotes.Controllers;
//using FunDooTesting.Testing;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.DependencyInjection;
//using Model.Models.DTOs.User;
//using Model.Models.Entity;
//using Model.Models.Utility;

//namespace FunDooTesting
//{
//    [TestFixture]
//    public class Tests
//    {
//        private ServiceProvider _serviceProvider;
//        private FunDooDataBaseContext _context;
//        private IUserDAL _userDAL;
//        private IUserBLL _userBLL;
//        private UserController _userController;

//        [SetUp]
//        public void Setup()
//        {
//            var serviceCollection = new ServiceCollection();
//            var startup = new TestStartup();
//            startup.ConfigureServices(serviceCollection);
//            _serviceProvider = serviceCollection.BuildServiceProvider();

//            _context = _serviceProvider.GetService<FunDooDataBaseContext>();
//            _context.Database.EnsureCreated();

//            _userDAL = _serviceProvider.GetService<IUserDAL>();
//            _userBLL = _serviceProvider.GetService<IUserBLL>();
//            _userController = _serviceProvider.GetService<UserController>();
//        }

//        [TearDown]
//        public void TearDown()
//        {
//            _context.Database.EnsureDeleted();
//            _serviceProvider.Dispose();
//        }

//        [Test]
//        public void RegisterUser_ReturnOkStatus()
//        {
//            //Arrange
//            var registerUser = new RegisterUserModel()
//            {
//                FirstName = "akhil",
//                LastName = "B",
//                Email = "akhil@gmail.com",
//                Password = "akhil@123"
//            };
//            //Act
//            var result = _userController.RegisterUser(registerUser);
//            //Assert
//            Assert.IsInstanceOf<OkObjectResult>(result);

//            var okObject = result as OkObjectResult;
//            var responseModel = okObject.Value as ResponseModel<User>;

//            Assert.IsNotNull(responseModel);
//            Assert.AreEqual(registerUser.Email, responseModel.Data.Email);
//        }

//        //[Test]
//        //public void Registering_AllReadyExistingUser()
//        //{
//        //    var existingUserModel = new RegisterUserModel()
//        //    {
//        //        FirstName = "akhil",
//        //        LastName = "B",
//        //        Email = "akhil@gmail.com",
//        //        Password = "akhil@123"
//        //    };
//        //    var result = _userController.RegisterUser(existingUserModel);

//        //    Assert.IsInstanceOf<ConflictObjectResult>(result);

//        //    var conflictObject = result as ConflictObjectResult;
//        //    var responseModel = conflictObject.Value as ResponseModel<User>;

//        //    Assert.IsNotNull(responseModel);
//        //    Assert.AreEqual("The Email is already registered", responseModel.Message);
//        //}
//    }
//}