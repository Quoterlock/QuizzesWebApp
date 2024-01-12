using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.BusinessLogic.Interfaces
{
    public interface IAuthorizer
    {
        IdentityUser Authenticate(string username, string password);
        string GenerateToken(IdentityUser identity);

    }
}
