using Backend.Application.Abstract;
using Backend.Application.ViewModel;
using Backend.Domain.EntityClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ui.Controllers
{
    public class AccountController : BaseController
    {
        private UserManager<User> _User;
        private SignInManager<User> _Sign;
        private RoleManager<IdentityRole> _Role;
        private IFileUpload _FileUpload;
        private IMailSender _Mail;
        private IGenericRepository<SiteSetting> _SiteConfig;
        public AccountController(

            UserManager<User> user,
            SignInManager<User> sign,
            RoleManager<IdentityRole> role,
            IFileUpload fileUpload,
            IMailSender Mail,
            IGenericRepository<SiteSetting> SiteConfig

            )
        {
            _User = user;
            _Sign = sign;
            _Role = role;
            _FileUpload = fileUpload;
            _Mail = Mail;
            _SiteConfig = SiteConfig;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View(new SigninViewModel { });
        }
        [HttpPost]
        public async Task<IActionResult> Login(SigninViewModel Model)
        {
            if (!ModelState.IsValid)
            {
                return View(Model);
            }
            if(await _User.FindByEmailAsync(Model.EMail) == null && await _User.FindByNameAsync(Model.EMail) == null)
            {
                MessageCreate("User Error","User ıs Not Found", Type.error, WindowType.toastr);
                return View(Model);
            }
            var user_Name = await _User.FindByNameAsync(Model.EMail);
            var user_EMail = await _User.FindByEmailAsync(Model.EMail);
            if(user_Name != null)
            {
                if (user_Name.EmailConfirmed != true)
                {
                    MessageCreate("User Error", "User Is Not Confirm Mail", Type.error, WindowType.toastr);
                    return View(Model);
                }
                if(user_Name.Active != true)
                {
                    MessageCreate("User Error", "USer Is Banned",Type.error, WindowType.toastr);
                    return View(Model);
                }
                var result = await _Sign.PasswordSignInAsync(user_Name,Model.Password,Model.RememberMe,false);
                if (result.Succeeded)
                {
                    if (await _User.IsInRoleAsync(user_Name, "Admin"))
                    {
                        MessageCreate($"👋 Welcome {user_Name.Name} {user_Name.LastName}!", "You have successfully logged in to Vuexy. Now you can start to explore!", Type.success, WindowType.toastr);
                        return RedirectToAction("index", "Root");
                    }
                    return RedirectToAction("index", "User");
                }
                MessageCreate("User Error", "UserName or Password Not True", Type.error, WindowType.toastr);
                return View(Model);
            }
            if (user_EMail != null)
            {
                if (user_EMail.EmailConfirmed != true)
                {
                    MessageCreate("User Error", "E Mail Is Not Confirm", Type.error, WindowType.toastr);
                    return View(Model);
                }
                if (user_EMail.Active != true)
                {
                    MessageCreate("User Error", "User Is Banned", Type.error, WindowType.toastr);
                    return View(Model);
                }
                var result = await _Sign.PasswordSignInAsync(user_EMail, Model.Password, Model.RememberMe, false);
                if (result.Succeeded)
                {
                    MessageCreate($"👋 Welcome {user_EMail.Name} {user_EMail.LastName}!", "You have successfully logged in to Vuexy. Now you can start to explore!", Type.success, WindowType.toastr);

                    if (await _User.IsInRoleAsync(user_EMail, "Admin"))
                    {
                        return RedirectToAction("index", "Root");
                    }
                    return RedirectToAction("index", "User");
                }
                MessageCreate("User Error", "UserName or Password Not True", Type.error, WindowType.toastr);
                return View(Model);
            }
            MessageCreate("System Error", "Are You System Attacker Hah ? Write Log", Type.error, WindowType.toastr);
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            return View(new SignUpViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel Model)
        {
            //check process
            if (!ModelState.IsValid)
            {
                return View(Model);
            }
            if (await _User.FindByNameAsync(Model.UserName) != null)
            {

                MessageCreate("User Error", "UserName Is Ready To Use", Type.error, WindowType.toastr);
                return View(Model);
            }
            if (await _User.FindByEmailAsync(Model.EMail) != null)
            {
                MessageCreate("User Error", "E Mail ıs Ready To Use", Type.error, WindowType.toastr);
                return View(Model);
            }
            var random = "default.png";
            //user create process
            if(Model.FileImg != null)
            {
                random = $"{DateTime.UtcNow.Ticks}{Path.GetExtension(Model.FileImg.FileName)}";
                if (!await _FileUpload.Send(Model.FileImg, random, "jpg"))
                {
                    MessageCreate("Img Error", "Picture Is Img File", Type.error, WindowType.toastr);
                    return View(Model);
                }
            }
            var user = new User
            {
                Name = Model.Name,
                LastName = Model.LastName,
                UserName = Model.UserName,
                Email = Model.EMail,
                ImgUrl = random,
                Age = Model.Age,
                Active = true,
            };
            user.SocialLink = new SocialLink() { UserId = user.Id };
            var result = await _User.CreateAsync(user, Model.Password);
            if (result.Succeeded)
            {
                try
                {
                    var config = await _SiteConfig.GetByIdAsync(1);
                    var url = Url.Action("ConfirmMail", "Account", new
                    {
                        UserId = user.Id,
                        Token =await _User.GenerateEmailConfirmationTokenAsync(user)
                    }) ;
                    var html = $"<h1>Confirm Mail </h1><br><p>Click to confirm your account <a href='{config.Host}{url}'>Tıklayınız</a></p>";
                    await _Mail.Send(user.Email, "Hesabınızı Onaylayınız", html);
                    MessageCreate("User Sign Success", "Confirm Mail and Login in Sessions", Type.success, WindowType.toastr);
                    return RedirectToAction("Login");
                }
                catch (Exception)
                {
                    user.EmailConfirmed = true;
                    await _User.UpdateAsync(user);
                    MessageCreate("User Sign Success", "User Created,You Can Now Sign In", Type.success, WindowType.toastr);
                    return RedirectToAction("Login");
                }
            }
            var message = "";
            foreach (var item in result.Errors)
            {
                message += $"{item.Description}<br>";
            }
            MessageCreate("System", message, Type.error, WindowType.sweet);
            return View(Model);
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmMail(string UserId,string Token)
        {
            if (
                string.IsNullOrEmpty(UserId) ||
                string.IsNullOrEmpty(Token) ||
                await _User.FindByIdAsync(UserId) == null)
            {
                MessageCreate("System Error", "Access Denied", Type.error, WindowType.toastr);
                return RedirectToAction("Login");
            }
            var user = await _User.FindByIdAsync(UserId);
            var result =await _User.ConfirmEmailAsync(user, Token);
            if (result.Succeeded)
            {
                var userlist = _User.Users.Count();
                if(await _Role.FindByNameAsync("Admin") == null)
                {
                    await _Role.CreateAsync(new IdentityRole { Name = "Admin" });
                }
                if(userlist == 1)
                {
                    try
                    {
                        await _User.AddToRoleAsync(user, "Admin");
                    }
                    catch (Exception)
                    {
                        MessageCreate("Role Error", "Role Not Addded", Type.error, WindowType.toastr);
                        return RedirectToAction("Login");
                    }
                }
                MessageCreate("User Process", "User ıs Confirmed", Type.success, WindowType.toastr);
                return RedirectToAction("Login");
            }
            var message = "";
            foreach (var item in result.Errors)
            {
                message += $"{item.Description}<br>";
            }
            MessageCreate("System", message, Type.error, WindowType.sweet);
            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string EMail)
        {
            if (string.IsNullOrEmpty(EMail))
            {
                MessageCreate("Process Error","E Mail Is Not Null", Type.error, WindowType.toastr);
                return View();
            }
            var user = await _User.FindByEmailAsync(EMail);
            if(user == null)
            {
                MessageCreate("User Error", "User ıs Not Found", Type.error, WindowType.toastr);
                return View();
            }
            var config =await _SiteConfig.GetByIdAsync(1);
            var token = await _User.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("ResetPassword", "Account", new
            {
                UserId = user.Id,
                Token = token
            });
            var html = $"<h1>Reset Password </h1><br><p>Reset Password Is Link <a href='{config.Host}{url}'>Click</a></p>";
            try
            {
                await _Mail.Send(user.Email, "Reset Password", html);
                MessageCreate("Process Success","Check E Mail Account", Type.success, WindowType.toastr);
                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                MessageCreate("System Error", "Please Report Process To Admin", Type.error, WindowType.toastr);
                return View();
            }

        }
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string UserId,string Token)
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Token) || await _User.FindByIdAsync(UserId) == null)
            {
                MessageCreate("System Error", "Unatherized Access, Acces Denied", Type.error, WindowType.toastr);
                return RedirectToAction("login");
            }
            var user = await _User.FindByIdAsync(UserId);
            var model = new ResetPasswordViewModel
            {
                UserId = user.Id,
                Token = Token
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel Model)
        {
            if (!ModelState.IsValid)
            {
                return View(Model);
            }
            if(await _User.FindByIdAsync(Model.UserId) == null ){
                MessageCreate("User Error","User Is Not Found", Type.error, WindowType.toastr);
                return RedirectToAction("Login");
            }
            var user = await _User.FindByIdAsync(Model.UserId);
            var result = await _User.ResetPasswordAsync(user, Model.Token, Model.Password);
            if (result.Succeeded)
            {
                MessageCreate("User Success", "User Created,You Can Now Sign In", Type.success, WindowType.toastr);
                return RedirectToAction("Login");
            }
            var message = "";
            foreach (var item in result.Errors)
            {
                message += $"{item.Description}<br>";
            }
            MessageCreate("System", message, Type.error, WindowType.sweet);
            return RedirectToAction("Login");
        }
        //google login

        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("ExternalResponse", "Account");
            var properties = _Sign.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        public async Task<IActionResult> ExternalResponse()
        {
            ExternalLoginInfo info = await _Sign.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(Login));

            var result = await _Sign.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
            if (result.Succeeded)
            {
                if (await _Role.FindByNameAsync("Admin") == null)
                {
                    await _Role.CreateAsync(new IdentityRole { Name = "Admin" });
                }
                var user =await _User.FindByEmailAsync(info.Principal.FindFirst(ClaimTypes.Email).Value);
                var userlist = _User.Users.Count();
                if (userlist == 1)
                {
                    try
                    {
                        await _User.AddToRoleAsync(user, "Admin");
                    }
                    catch (Exception)
                    {
                        MessageCreate("System Error","Role IS Not Added", Type.error, WindowType.toastr);
                        return RedirectToAction("Login");
                    }
                }
                if (await _User.IsInRoleAsync(user,"Admin"))
                {
                    MessageCreate($"👋 Welcome {user.Name} {user.LastName}!", "You have successfully logged in to Vuexy. Now you can start to explore!", Type.success, WindowType.toastr);
                    return RedirectToAction("index", "Root");
                }
                return RedirectToAction("index", "User");
            }
            else
            {
                if (await _Role.FindByNameAsync("Admin") == null)
                {
                    await _Role.CreateAsync(new IdentityRole { Name = "Admin" });
                }
                User user = new User
                {
                    Name = info.Principal.FindFirst(ClaimTypes.Name).Value,
                    LastName = info.Principal.FindFirst(ClaimTypes.Surname).Value,
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    Active = true,
                    EmailConfirmed = true,
                    ImgUrl = "default.png",
            };
                user.SocialLink = new SocialLink() { UserId = user.Id };
                IdentityResult identResult = await _User.CreateAsync(user);
                if (identResult.Succeeded)
                {
                    var userlist = _User.Users.Count();
                    if (userlist == 1)
                    {
                        try
                        {
                            await _User.AddToRoleAsync(user, "Admin");
                        }
                        catch (Exception)
                        {
                            MessageCreate("System Error","Role Is Not Added", Type.error, WindowType.toastr);
                            return RedirectToAction("Login");
                        }
                    }
                    identResult = await _User.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {

                        await _Sign.SignInAsync(user, false);
                        if (await _User.IsInRoleAsync(user, "Admin"))
                        {
                            MessageCreate($"👋 Welcome {user.Name} {user.LastName}!", "You have successfully logged in to Vuexy. Now you can start to explore!", Type.success, WindowType.toastr);

                            return RedirectToAction("index", "Root");
                        }
                        return RedirectToAction("index", "User");
                    }
                }
                MessageCreate("System Error","You Are Hacker Hah ? hsahhhfds", Type.error, WindowType.toastr);
                return RedirectToAction("Login");
            }
           
        }
        public async Task<IActionResult> Logout()
        {
            await _Sign.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
