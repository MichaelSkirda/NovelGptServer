using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace RenSharpServer.Controllers
{
	public class AccountController : Controller
	{
		private AppDbContext db;
		public AccountController(AppDbContext context)
		{
			db = context;
		}
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(string email, string password)
		{
			User? user = await db.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
			if (user != null)
			{
				await Authenticate(email); // аутентификация
				return RedirectToAction("Index", "Home");
			}

			return View(null);
		}

		private async Task Authenticate(string userName)
		{
			// создаем один claim
			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
			};
			// создаем объект ClaimsIdentity
			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
			// установка аутентификационных куки
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login", "Account");
		}
	}
}
