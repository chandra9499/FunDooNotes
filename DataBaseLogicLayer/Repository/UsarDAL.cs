﻿using DataBaseLogicLayer.Context;
using DataBaseLogicLayer.Helper;
using DataBaseLogicLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.Models.DTOs.Token;
using Model.Models.DTOs.User;
using Model.Models.Entity;
using Model.Models.Utility;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLogicLayer.Repo
{
    public class UsarDAL : IUserDAL
    {
        private readonly FunDooDataBaseContext _context;
        private readonly TokenGenarator _token;
        public UsarDAL(FunDooDataBaseContext context, TokenGenarator token)
        {
            _context = context;
            _token = token;
        }
        public bool ChangePassword(string userId, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }
        
        public ResponseModel<UserResponceModel> GetProfile(int userId)
        {
            var user = _context.Users.FirstOrDefault(user=>user.UserId.Equals(userId));
            if (user != null) 
            {
                return new ResponseModel<UserResponceModel>()
                {
                    StatusCode =(int) HttpStatusCode.OK,
                    Message = "user information is featched",
                    Data = new UserResponceModel() { FirstName=user.FirstName,LastName=user.LastName, Email=user.Email }
                };
            }
            return new ResponseModel<UserResponceModel>()
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "user information is featched",
                Data = null
            };
        }

        public ResponseModel<TokenResponce> LoginUser(LoginModel loginModel)
        {
            try
            {
                // Retrieve the user by email
                var user = _context.Users.FirstOrDefault(u => u.Email.Equals(loginModel.Email));
                if (user == null)
                {
                    return new ResponseModel<TokenResponce>()
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = "Invalid Email",
                        Success = false,
                        Data = null
                    };
                }

                // Verify the password
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginModel.Password, user.Password);
                if (!isValidPassword)
                {
                    return new ResponseModel<TokenResponce>()
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = "Invalid password",
                        Success = false,
                        Data = null
                    };
                }
                
                // Create claims for the JWT
                var authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Sid, Convert.ToString(user.UserId)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                Console.WriteLine(authClaims);
                // Generate tokens
                var token = _token.GetToken(authClaims);
                var refreshToken = _token.GetRefreshToken();
                
                var userToken = _context.Tokens.FirstOrDefault(token => token.Email.Equals(user.Email));
                if (userToken != null)
                {
                    var info = new TokenInfo()
                    {
                        Email = user.Email,
                        Token = token.TokenString,
                        TokenExpiry = DateTime.Now.AddDays(7)
                    };
                    try
                    {
                        _context.Tokens.Add(info);
                        _context.SaveChanges();
                    }
                    catch (Exception ex) 
                    {
                        throw new Exception("unable to save the token",ex);
                    }
                }
                

                return new ResponseModel<TokenResponce>()
                {
                    StatusCode = (int) HttpStatusCode.OK,
                    Message = "Logged in",
                    Success = true,
                    Data = token
                };
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return new ResponseModel<TokenResponce>()
                {
                    StatusCode = (int) HttpStatusCode.InternalServerError,
                    Message = "An error occurred while logging in: " + ex.Message,
                    Success = false,
                    Data = null
                };
            }
        }

        public void LogoutUser()
        {
            throw new NotImplementedException();
        }

        /*
         * 	
            Response body
            Download
            {
              "statusCode": 201,
              "success": true,
              "message": "user created success fully",
              "data": {
                "userId": 2,
                "firstName": "chandrashekar",
                "lastName": "chimmanchoud",
                "email": "s.k.chandrashekar123@gmail.com",
                "password": "$2a$11$rlIxqKOFux79RNST1fM59.67NXoseBSyOcJ73ZaJ2rXCyDSCMXrsq"
              }
            }
         */

        public ResponseModel<User> RegisterUser(RegisterUserModel registerUser)
        {
            //check if the user exist or not
            var userExist = (User)_context.Users.Where(user => user.Email.Equals(registerUser.Email)).FirstOrDefault();

            if (userExist != null)
            {
                return new ResponseModel<User>((int)HttpStatusCode.Conflict, false, "The Email is already registered", null);
            }
            //bcrypting the password
            registerUser.Password = BCryptPassword.BCryptThePassWord(registerUser.Password);

            var user = new User()
            {
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                Email = registerUser.Email,
                Password = registerUser.Password,
            };
            //create the user hear
            _context.Users.Add(user);
            try
            {
                _context.SaveChanges();
                return new ResponseModel<User>((int)HttpStatusCode.Created, true, "user created success fully", user);
            }
            catch (Exception ex)
            {
                throw new Exception("user creation failed");
            }
        }

        public bool ResetPassword(string email)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProfile(string userId, User updatedUser)
        {
            throw new NotImplementedException();
        }
    }
}
