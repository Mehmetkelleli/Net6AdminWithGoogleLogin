using Backend.Application.Abstract;
using Backend.Application.ViewModel;
using Backend.Domain.EntityClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Ui.Controllers.BaseController;

namespace Ui.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RootController : BaseController
    {
        private UserManager<User> _User;
        private RoleManager<IdentityRole> _Role;
        private IFileUpload _FileUpload;
        private IGenericRepository<SiteSetting> _Setting;

        public RootController(
            UserManager<User> User,
            RoleManager<IdentityRole> role,
            IFileUpload FileUpload,
            IGenericRepository<SiteSetting> setting
            )
        {
            _User = User;
            _Role = role;
            _FileUpload = FileUpload;
            _Setting = setting;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> RoleList()
        {
            var model = new RoleViewModel()
            {
                Roles = _Role.Roles,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RoleList(RoleViewModel Model)
        {
            var result = await _Role.CreateAsync(new IdentityRole(Model.AddRole));
            var model = new RoleViewModel()
            {
                Roles = _Role.Roles,
            };
            if (result.Succeeded)
            {
                return View(model);
            }
            var message = "";
            foreach (var item in result.Errors)
            {
                message += $"{item.Description}<br>";
            }
            MessageCreate("System", message, Type.error, WindowType.sweet);
            return View(model);

        }
        [HttpGet]
        public async Task<IActionResult> RoleDelete(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                MessageCreate("Role Error", "Role Id Is Null", Type.error, WindowType.toastr);
                return RedirectToAction("RoleList");
            }
            if(await _Role.FindByIdAsync(id) ==  null)
            {
                MessageCreate("Role Error", "Role Is Null", Type.error, WindowType.toastr);
                return RedirectToAction("RoleList");
            }
            var role = await _Role.FindByIdAsync(id);
            var result =await _Role.DeleteAsync(role);
            if (result.Succeeded)
            {
                MessageCreate("Role Process", "Role Is Deleted", Type.success, WindowType.toastr);
                return RedirectToAction("RoleList");
            }
            var message = "";
            foreach (var item in result.Errors)
            {
                message += $"{item.Description}<br>";
            }
            MessageCreate("System", message, Type.error, WindowType.sweet);
            return RedirectToAction("RoleList");
        }
        [HttpGet]
        public async Task<IActionResult> RoleUpdate(string id)
        {
            if (await _Role.FindByIdAsync(id) == null)
            {
                MessageCreate("Role Error", "Role Is Null", Type.error, WindowType.toastr);
                return RedirectToAction("RoleList");
            }
            var role =await _Role.FindByIdAsync(id);
            var RoleUser = new List<User>();
            var NonRoleUser = new List<User>();
            var users = _User.Users;
            foreach (var item in users)
            {
                if(await _User.IsInRoleAsync(item, role.Name))
                {
                    RoleUser.Add(item);
                }
                else
                {
                    NonRoleUser.Add(item);
                }
            }
            var model = new RoleViewModel()
            {
                NonUsers = NonRoleUser,
                RoleUsers = RoleUser,
                RoleName = role.Name,
                RoleId = role.Id
            };
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleViewModel Model, string[] NonUserAdd, string[] RoleUserAdd)
        {
            if (string.IsNullOrEmpty(Model.RoleName) || await _Role.FindByIdAsync(Model.RoleId) == null)
            {
                MessageCreate("Role Error", "Role Is Not Null", Type.error, WindowType.toastr);
                return RedirectToAction("RoleList");
            }
            var role = await _Role.FindByIdAsync(Model.RoleId);
            role.Name = Model.RoleName;
            var result = await _Role.UpdateAsync(role);
            if (result.Succeeded)
            {
                foreach (var item in NonUserAdd)
                {
                    var user = await _User.FindByIdAsync(item);
                    await _User.RemoveFromRoleAsync(user, role.Name);

                }
                foreach (var item in RoleUserAdd)
                {
                    var user = await _User.FindByIdAsync(item);
                    await _User.AddToRoleAsync(user, role.Name);
                }
                MessageCreate("Role Process", "Role Is Updated", Type.success, WindowType.toastr);
                return RedirectToAction("RoleList");
            }

            MessageCreate("Role Error", "", Type.error, WindowType.toastr);
            return RedirectToAction("");
        }
        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            var list = new List<UserViewModel>();
            foreach (var item in _User.Users)
            {
                var model = new UserViewModel
                {
                    User = item,
                    Roles = await _User.GetRolesAsync(item)
                };
                list.Add(model);
            }
            return View(list);
        }
       [HttpGet]
       public async Task<IActionResult> UserEdit(string id)
       {
            //if string is null redirect your account edit
            if(await _User.FindByNameAsync(User.Identity.Name) == null)
            {
                MessageCreate("User Error", "User Id Is Not Found", Type.error, WindowType.toastr);
                return RedirectToAction("Login", "Account");
            }
            var user = await _User.FindByNameAsync(User.Identity.Name);
            if (!string.IsNullOrEmpty(id))
            {
                if (await _User.FindByIdAsync(id) == null)
                {
                    MessageCreate("User Error", "User Is Not Found", Type.error, WindowType.toastr);
                    return RedirectToAction("UserList");
                }
                user = await _User.FindByIdAsync(id);
            }
            var model = new UserViewModel();
            model.User = user;
            model.Roles = await _User.GetRolesAsync(user);
            ViewBag.Roles = new SelectList(_Role.Roles, "Id", "Name");
            return View(model);
       }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserViewModel Model,IFormFile FileImg)
        {
            var message = "";
            if (await _User.FindByIdAsync(Model.User.Id) == null)
            {
                MessageCreate("User Error", "User Is Not Found", Type.error, WindowType.toastr);
                return Redirect($"/User/{Model.User.Id}");
            }
            //UserName Check
            
            var user = await _User.FindByIdAsync(Model.User.Id);
            var oldUser = new User
            {
                UserName = user.UserName,
            };
            
            var userName = await _User.FindByNameAsync(Model.User.UserName);
            if(user != userName && userName != null)
            {
                if(user.UserName == user.UserName)
                {
                    MessageCreate("User Error", "User Id Is Not Found", Type.error, WindowType.toastr);
                    return Redirect($"/User/{Model.User.Id}");
                }
            }
            user.UserName = Model.User.UserName;
            user.Email = Model.User.Email;
            user.Name = Model.User.Name;
            user.LastName = Model.User.LastName;
            user.Age = Model.User.Age;
            user.ImgUrl = Model.User.ImgUrl;
            if (FileImg != null)
            {
                var random = $"{DateTime.UtcNow.Ticks}{Path.GetExtension(FileImg.FileName)}";
                if (!await _FileUpload.Send(FileImg, random, "jpg"))
                {
                    MessageCreate("Picture Error", "Picture is must jpg file", Type.error, WindowType.toastr);
                    return Redirect($"/User/{Model.User.Id}");
                }
                user.ImgUrl = random;
            }
            if (!string.IsNullOrEmpty(Model.NewPassword))
            {
                var token = await _User.GeneratePasswordResetTokenAsync(user);
                var state = await _User.ResetPasswordAsync(user,token,Model.NewPassword);
                if (!state.Succeeded)
                {
                    message += "";
                    foreach (var item in state.Errors)
                    {
                        message += $"{item.Description}<br>";
                        
                    }
                    MessageCreate("System", message, Type.error, WindowType.sweet);

                    return Redirect($"/User/{Model.User.Id}");
                }
                MessageCreate("User Process", "User Is Updated", Type.success, WindowType.toastr);
                return Redirect($"/User/{Model.User.Id}");
            }
            var result = await _User.UpdateAsync(user);
            if (result.Succeeded)
            {
                if(oldUser.UserName != user.UserName)
                {
                    MessageCreate("User Process", "Please Re Login For Changings", Type.success, WindowType.toastr);
                    return RedirectToAction("Login", "Account");
                }
                MessageCreate("User Process", "User Is Updated", Type.success, WindowType.toastr);
                return Redirect($"/User/{Model.User.Id}");
            }
            foreach (var item in result.Errors)
            {
                message += $"{item.Description}<br>";
            }
            MessageCreate("System", message, Type.error, WindowType.sweet);
            return Redirect($"/User/{Model.User.Id}");
        }
        [HttpPost]
        public async Task<IActionResult> UserDelete(string UserId)
        {
            if(await _User.FindByIdAsync(UserId) == null)
            {
                MessageCreate("User Error", "User Is Not Found", Type.error, WindowType.toastr);
                return Redirect($"/User/");
            }
            var user = await _User.FindByIdAsync(UserId);
            var result = await _User.DeleteAsync(user);
            if (result.Succeeded)
            {
                MessageCreate("User Process", "User Id Deleted", Type.success, WindowType.toastr);
                return RedirectToAction("Login", "Account");
            }
            var message = "";
            foreach (var item in result.Errors)
            {
                message += $"{item.Description}<br>";
            }
            MessageCreate("System", message, Type.error, WindowType.sweet);
            return Redirect($"/User/");
        }
        [HttpGet]
        public async Task<IActionResult> UserImgRemove(string id)
        {
            var user = await _User.FindByIdAsync(id);
            user.ImgUrl = "default.png";
            var result =await _User.UpdateAsync(user);
            if (result.Succeeded)
            {
                MessageCreate("User Process", "Img Is Removed", Type.success, WindowType.toastr);
                return Redirect($"/User/");
            }
            var message = "";
            foreach (var item in result.Errors)
            {
                message += $"{item.Description}<br>";
            }
            MessageCreate("System", message, Type.error, WindowType.sweet);
            return Redirect($"/User/");
        }
        [HttpGet]
        public async Task<IActionResult> SiteConfig()
        {
            var config = await _Setting.GetByIdAsync(1,false) == null? new SiteSetting { }: await _Setting.GetByIdAsync(1, false);
            var model = new SiteConfigViewModel();
            model.SiteLogoUrl = config.SiteLogoUrl;
            model.MetaTags = config.MetaTags;
            model.SiteAdmin = config.SiteAdmin;
            model.SiteDescriptions = config.SiteDescriptions;
            model.SmtpHost = config.SmtpHost;
            model.SmtpPort = config.SmtpPort;
            model.SmtpUser = config.SmtpUser;
            model.Host = config.Host;
            model.SmtpPassword = config.SmtpPassword;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SiteConfig(SiteConfigViewModel Model,IFormFile? SiteLogo)
        {
            if (!ModelState.IsValid)
            {
                return View(Model);
            }
            var config = await _Setting.GetByIdAsync(1);
            config.SmtpHost = Model.SmtpHost;
            config.SmtpUser = Model.SmtpUser;
            config.Host = Model.Host;
            config.MetaTags = Model.MetaTags;
            config.SiteAdmin = Model.SiteAdmin;
            config.SiteDescriptions = Model.SiteDescriptions;
            config.SiteLogoUrl = Model.SiteLogoUrl;
            config.SmtpPassword = Model.SmtpPassword;
            config.SmtpPort = Model.SmtpPort;
            if(SiteLogo != null)
            {
                var random = $"{DateTime.UtcNow.Ticks}{Path.GetExtension(SiteLogo.FileName)}";
                if (!await _FileUpload.Send(SiteLogo, random, "png"))
                {
                    MessageCreate("System","File Is Not Null Is Png", Type.error, WindowType.toastr);
                    return View(Model);
                }
                config.SiteLogoUrl = random;
            }
            if (_Setting.Update(config))
            {
                await _Setting.SaveChangesAsync();
                MessageCreate("System Setting", "Update Setting", Type.success, WindowType.toastr);
                return View(Model);
            }
            MessageCreate("System Error", "Critical Error Fail ", Type.error, WindowType.toastr);
            return View(Model);
        }
    }
}
