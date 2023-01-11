using Backend.Application.ViewModel;
using Backend.Domain.EntityClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ui.ViewComponents
{
    public class NavbarViewComponent:ViewComponent
    {
        private UserManager<User> _User;
        public NavbarViewComponent(UserManager<User> User)
        {
            _User = User;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _User.FindByNameAsync(User.Identity.Name);
            NavbarViewModel Model = new NavbarViewModel()
            {
                User = user,
                Role = await _User.GetRolesAsync(user)
            };
            return View(Model);
        }
    }
}
