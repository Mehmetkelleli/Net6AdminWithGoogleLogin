using Backend.Domain.EntityClass;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Backend.Application.ViewModel
{
    public class RoleViewModel
    {
        //list role
        public IQueryable<IdentityRole> Roles { get; set; }
        
        //Add Role
        public string AddRole { get; set; }
        
        //Role Update Details
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public List<User> RoleUsers { get; set; }
        public List<User> NonUsers { get; set; }

    }
}
