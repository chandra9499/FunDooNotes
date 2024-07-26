//using BusinessLogicLayer.Interface;
//using DataBaseLogicLayer.Context;
//using DataBaseLogicLayer.Interface;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DataBaseLogicLayer.Repository;
//using BusinessLogicLayer.Service;
//using FunDooNotes.Controllers;
//using DataBaseLogicLayer.Helper;

//namespace FunDooTesting.Testing
//{
//    public class TestStartup
//    {
//        public IConfiguration Configuration { get; }

//        public TestStartup()
//        {
//            Configuration = new ConfigurationBuilder()
//                .SetBasePath(Directory.GetCurrentDirectory())
//                .AddJsonFile("appsettings.json")
//                .Build();
//        }

//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddDbContext<FunDooDataBaseContext>(options =>
//                options.UseSqlServer(Configuration.GetConnectionString("FunDooConnection_Test")));

//            services.AddScoped<IUserDAL, UsarDAL>();
//            services.AddScoped<IUserBLL, UserBLL>();
//            services.AddScoped<UserController>();

//            services.AddSingleton<TokenGenarator>();
//        }
//    }
//}
