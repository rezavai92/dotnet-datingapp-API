using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace API.Entities
{
    public class LoginResponse
    {
         public string UserName { get; set; }
         public string Token{ get; set; }

         public LoginResponse(string userName){
            this.UserName = userName;
            this.Token = "token";
        }
    }
}