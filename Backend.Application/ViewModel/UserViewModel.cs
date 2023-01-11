using Backend.Domain.EntityClass;
using Microsoft.AspNetCore.Identity;

namespace Backend.Application.ViewModel
{
    public class UserViewModel
    {
        //list user
        public User User { get; set; }
        public IList<string> Roles { get; set; }
        public IQueryable<IdentityRole> AllRoles { get; set; }
        public string SelectedRole { get; set; }
        public string NewPassword { get; set; }

    }
}
