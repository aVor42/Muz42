using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AudioShmaudio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace AudioShmaudio.Controllers
{
    public class AccountController : Controller
    {

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _context;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, ApplicationContext context)
        {

            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        // Возвращает форму для регистрации пользователя
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        // Принимает регистрационные данные (логин, имя, пароль)
        // Создаёт пользователя с ролью user
        [HttpPost]
        public async Task<IActionResult> Registration(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User { Login = model.Login, UserName = model.UserName };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }
        }

        // Возвращает форму для авторизации пользователя
        [HttpGet]
        public IActionResult LogIn(string returnUrl)
        {
            return View(new LogInViewModel { ReturnUrl = returnUrl});
        }

        // Принимает логин и пароль (в модели)
        // Аутентифицирует пользователя, если данные верны
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userManager.Users.FirstOrDefault(user => user.Login == model.Login);
            if(user is null)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        // Позволяет выйти из учётной записи
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Возращает представление списка пользователей сервиса
        // с возможностью задать модераторов
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UsersList()
        {
            if (Request.Headers["x-requested-with"] != "XMLHttpRequest")
                return View("Index");

            var users = await _context.Users.ToListAsync();
            var usersRoles = new List<UsersRoles>();

            foreach(var user in users)
            {
                usersRoles.Add(new UsersRoles {
                    User = user,
                    Roles = new List<string>(await _userManager.GetRolesAsync(user))
                });
            }

            return PartialView("Partials/_UsersList", usersRoles);
            
        }

        // Принимает код пользователя
        // Добавляет пользователю роль модератора
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddModer(string userId)
        {
            var user = _context.Users.Find(userId);
            var isModer = await _userManager.IsInRoleAsync(user, "moderator");
            var isAdmin = await _userManager.IsInRoleAsync(user, "admin");
            if (user is null || isModer || isAdmin)
                return BadRequest();

            await _userManager.RemoveFromRoleAsync(user, "user");
            await _userManager.AddToRoleAsync(user, "moderator");
            return Ok();
        }

        // Принимает код пользователя
        // Удаляет у пользователя роль модератора
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RemoveModer(string moderId)
        {
            var moder = _context.Users.Find(moderId);
            var isUser= await _userManager.IsInRoleAsync(moder, "user");
            var isAdmin = await _userManager.IsInRoleAsync(moder, "admin");
            if (moder is null || isUser || isAdmin)
                return BadRequest();

            await _userManager.RemoveFromRoleAsync(moder, "moderator");
            await _userManager.AddToRoleAsync(moder, "user");
            return Ok();
        }

    }
}
