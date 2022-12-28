using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Data.Migrations;
using API.DTOS;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private DataContext _context;
        public AccountController(DataContext dataContext)
        {
            this._context = dataContext;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto payload)
        {

         
                if (await this.IsUserExists(payload.UserName))
                    {
                        return BadRequest("username already exists");
                    }
                    using var hmac = new HMACSHA512();
                    AppUser user = new AppUser
                    {
                        PasswordSalt = hmac.Key,
                        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload.Password)),
                        UserName = payload.UserName.ToLower(),

                    };
                    this._context.Add(user);
                    await this._context.SaveChangesAsync();

                    return user;
          



        }

[HttpGet("Login")]
 public async Task<ActionResult<LoginResponse>> Login(LoginDto  payload){
            var user = await this._context.Users.SingleOrDefaultAsync(x => x.UserName == payload.UserName);
            if(user == null){
                return Unauthorized("invalid username");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload.Password));

            for (int i = 0; i < computedHash.Length;i++){
                if(computedHash[i] != user.PasswordHash[i]){
                    return Unauthorized("invalid password");
                }
            }

            return new LoginResponse(user.UserName);



        }

        public async Task<Boolean> IsUserExists(string userName)
        {
            return await this._context.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }


   
}