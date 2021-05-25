using gRPC.Api.Data;
using gRPC.Api.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace gRPC.Api.Services
{
    public class UsersService : Users.UsersBase
    {
        private readonly DataContext _context;

        public UsersService(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        public override async Task<Replay> AddUser(AddRequest request, ServerCallContext context)
        {
            var userExist = await _context.Users.AsNoTracking().FirstOrDefaultAsync(v => v.Username.ToLower() == request.Username.ToLower());
            if(userExist is not null)
                return new Replay() { Success = false, Message = $"Username: '{request.Username} exist'" };

            _context.Users.Add(new Data.Entity.UserEntity { Username = request.Username, Password = request.Password });
            var success = await _context.SaveChangesAsync();

            if (success != 0)
                return new Replay() { Success = true };
            else
                return new Replay() { Success = false, Message = "Error while adding new user " };
        }

        [Authorize]
        public override async Task<Replay> EditUser(EditRequest request, ServerCallContext context)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(v => v.Id == request.Id);
            if (user is null)
                return new Replay() { Success = false, Message = $"Cant find user '{request.Username}'" };

            user.Username = request.Username;
            user.Password = request.Password;
            _context.Users.Update(user);
            var success = await _context.SaveChangesAsync();

            if (success != 0)
                return new Replay() { Success = true };
            else
                return new Replay() { Success = false, Message = "Error while adding new user " };
        }

        [Authorize]
        public override async Task<Replay> DeleteUser(RemoveRequest request, ServerCallContext context)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(v => v.Id == request.Id);
            if (user is null)
                return new Replay() { Success = false, Message = $"Cant find user with id '{request.Id}'" };

            _context.Users.Remove(user);

            var success = await _context.SaveChangesAsync();

            if (success != 0)
                return new Replay() { Success = true };
            else
                return new Replay() { Success = false, Message = "Error while adding new user " };
        }

        [Authorize]
        public override async Task<UsersReplay> GetUsers(Empty request, ServerCallContext context)
        {
            var users = await _context.Users.ToListAsync();
            var mappedUsers = new UsersReplay();
            mappedUsers.Users.AddRange(users.Select(x => new UsersReplay.Types.User { Id = x.Id, Username = x.Username, Password = x.Password }));
            return mappedUsers;
        }
    }
}
