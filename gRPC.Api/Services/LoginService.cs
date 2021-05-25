using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using gRPC.Api.Data;
using gRPC.Api.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace gRPC.Api.Services
{
    public class LoginService : Login.LoginBase
    {
        private readonly DataContext _dataContext;
        private readonly byte[] jwtSecret;

        public LoginService(DataContext dataContext, IConfiguration configuration)
        {
            _dataContext = dataContext;
            jwtSecret = Encoding.ASCII.GetBytes(configuration.GetValue<string>("JwtSecret"));
        }

        [AllowAnonymous]
        public override async Task<LoginReplay> Login(LoginRequest request, ServerCallContext context)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(v => v.Username == request.Username && v.Password == request.Password);

            if (user is null)
                return new LoginReplay { AccessToken = string.Empty };


            var token = new JwtSecurityToken(signingCredentials: new SigningCredentials(new SymmetricSecurityKey(jwtSecret), SecurityAlgorithms.HmacSha256Signature),
                claims: new Claim[]  { new Claim(ClaimTypes.Name, user.Username) }
                );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginReplay() { AccessToken = accessToken };
        }
    }
}
