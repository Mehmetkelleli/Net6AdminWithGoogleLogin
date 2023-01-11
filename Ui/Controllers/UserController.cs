using AutoMapper;
using Backend.Application.Abstract;
using Backend.Application.ViewModel;
using Backend.Domain.EntityClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ui.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        public UserController()
        {

        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Icon()
        {
            return View();
        }
    }
}
