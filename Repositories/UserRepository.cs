using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Web_App.Data;
using Web_App.Models;
using Web_App.Repositories.Interfaces;

namespace Web_App.Repositories
{
    public class UserRepository : IUserRepository
    {
        static class ConfigurationManager
        {
            public static IConfiguration AppSetting { get; }
            static ConfigurationManager()
            {
                AppSetting = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
            }
        }

        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public User Authenticate(string username, string password)
        {
            var userInDb = _context.Users.FirstOrDefault(u=>u.UserName == username && u.Password == password);
            if (userInDb == null)
            {
                
                throw new GraphQLException(new Error("not found", null, null));
                /*return null;*/
                /*return  new NoContentResult();*/
                /*return HttpStatusCode.BadRequest;*/
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Key"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
         
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim("Id", userInDb.Id.ToString())
                }),
                Issuer = ConfigurationManager.AppSetting["JWT:Issuer"],
                Audience = ConfigurationManager.AppSetting["JWT:Audience"],
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = signinCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.CreateToken(tokenDescriptor);
            userInDb.Token = tokenHandler.WriteToken(tokenString);
            userInDb.Password = "";
            return userInDb;

        }

        public User Register(string username, string password)
        {
            User user = new User()
            {
                UserName = username,
                Password = password
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public bool UniqueUser(string username)
        {
            var user = _context.Users.FirstOrDefault(u=>u.UserName == username);
            if(user == null)
            {
                return true;
            }

            return false;
        }
    }
}
