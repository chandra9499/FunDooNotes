using BusinessLayer.Interface;
using BusinessLayer.Service;
using BusinessLogicLayer.Interface;
using BusinessLogicLayer.Service;
using DataBaseLayer.Helper;
using DataBaseLayer.Interface;
using DataBaseLayer.Repository;
using DataBaseLogicLayer.Context;
using DataBaseLogicLayer.Helper;
using DataBaseLogicLayer.Interface;
using DataBaseLogicLayer.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();

//let's initialize the database context
builder.Services.AddDbContext<FunDooDataBaseContext>(
    cfg=>cfg.UseSqlServer(builder.Configuration.GetConnectionString("FunDooConnection")));
builder.Services.AddTransient<IUserBLL,UserBLL>();
builder.Services.AddTransient<IUserDAL, UsarDAL>();
builder.Services.AddTransient<INoteBLL,NoteBLL>();
builder.Services.AddTransient<INotesDAL,NotesDAL>();
builder.Services.AddTransient<ICollaboratorBLL, CollaboratorBLL>();
builder.Services.AddTransient<ICollaboratorDAL, CollaboratorDAL>();
builder.Services.AddTransient<ILabelBLL, LabelBLL>();
builder.Services.AddTransient<ILabelDAL, LabelDAL>();
builder.Services.AddTransient<TokenGenarator>();
builder.Services.AddTransient<GlobalExceptionHandler>();

// configure Redis into the application
var redisConnectionString = builder.Configuration["Redis:ConnectionString"];
var connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
builder.Services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
builder.Services.AddScoped<ICacheDL, CacheDL>();

// Add services to the container;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,//valideates the server
            ValidateAudience = true,//validates the user
            ValidateLifetime = true,//validatwe the key is vali or within the expiering time
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:secret"]))
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Userid", policy =>
        policy.RequireClaim(ClaimTypes.Sid));//I am using this claim from jwt by using police
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(mid =>
{
    mid.SwaggerDoc("v1", new OpenApiInfo()
    {
        Version = "v1",
        Title = "JWTToken_Auth_API"
    });
    mid.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter The JWT Token with bearer formet like bearer[space] token"
    });
    mid.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference=new OpenApiReference()
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandler>();
app.MapControllers();

//app.Run(async context => await context.Response.WriteAsync("hello world"));
app.Run();
