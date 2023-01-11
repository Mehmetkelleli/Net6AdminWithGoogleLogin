using Backend.Application.Abstract;
using Backend.Domain.EntityClass;
using Backend.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Persistence.Concrete
{
    public class UserRepository:IUserRepository
    {
        private UserManager<User> _User;
        public UserRepository(UserManager<User> User)
        {
            _User = User;
        }
    }
}
