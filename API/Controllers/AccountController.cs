using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Data.Migrations;
using API.DTOS;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
          private DataContext _context;
        public AccountController(DataContext  dataContext){
            this._context = dataContext;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto payload){

            if(await this.IsUserExists(payload.UserName)){
                return BadRequest("username already exists");
            }
            using var  hmac = new HMACSHA512();

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


        public async Task<Boolean> IsUserExists(string userName){
            return await this._context.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
}