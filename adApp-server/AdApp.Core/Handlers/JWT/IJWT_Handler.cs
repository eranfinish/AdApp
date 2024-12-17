using AdApp.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdApp.Core.Handlers.JWT
{
    public interface IJWT_Handler
    {
        string GenerateJWT(User usr, IConfiguration config);
    }
}
