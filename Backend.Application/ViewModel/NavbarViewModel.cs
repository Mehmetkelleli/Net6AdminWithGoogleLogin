
using Backend.Domain.EntityClass;

namespace Backend.Application.ViewModel
{
    public class NavbarViewModel
    {
        public User User { get; set; }
        public IList<string> Role { get; set; }
    }
}
