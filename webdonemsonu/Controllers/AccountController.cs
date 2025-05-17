using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webdonemsonu.Data;
using webdonemsonu.Models;
using webdonemsonu.Models.ViewModels;

namespace webdonemsonu.Controllers
{
	public class AccountController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_context = context;
		}

		[HttpGet] // /Account/Register
		public IActionResult Register()
		{
			return View(); //register.cshtml
		}

		[HttpPost] // /Account/Register
		[ValidateAntiForgeryToken] //Sadece sitenin kendisinden gelen form verileri işlenir.
		public async Task<IActionResult> Register(RegisterVM model)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser
				{
					UserName = model.Email,
					Email = model.Email,
					FirstName = model.FirstName,
					LastName = model.LastName
				};

				var result = await _userManager.CreateAsync(user, model.Password); //Yeni kullanıcı oluşturuldu.
				if (result.Succeeded)
				{
					await _signInManager.SignInAsync(user, isPersistent: true); //kalıcı cookie
					return RedirectToAction("Index", "Home"); //doğru oluştuysa otomatik giriş yapılıp ana sayfaya gider.
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			return View(model);
		}

		[HttpGet] // /Account/Login
		public IActionResult Login(string? returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl; //Eğer giriş yapmadan bir sayfada kalmışsa giriş yapınca oradan devam eder.
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginVM model, string? returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;

			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(
					model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

				if (result.Succeeded)
				{
					var user = await _userManager.FindByEmailAsync(model.Email);

					if (await _userManager.IsInRoleAsync(user, "Admin"))
					{
						// Admin giriş yaptıysa admin paneline yönlendir
						return RedirectToAction("AdminPanel", "Admin");
					}
					else
					{
						// Normal kullanıcıysa geldiği sayfaya git (yoksa anasayfa)
						return RedirectToLocal(returnUrl);
					}
				}

				ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
			}

			return View(model);
		}


		[HttpPost] // /Account/Logout
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		private IActionResult RedirectToLocal(string? returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			return RedirectToAction("Index", "Home");
		}

		[Authorize]
		[HttpGet]
		[Authorize]
[HttpGet]
public async Task<IActionResult> EditProfile()
{
    var user = await _userManager.GetUserAsync(User);
    if (user == null)
        return NotFound();

    var model = new EditProfileVM
    {
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email
    };

    return View(model);
}

[Authorize]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> EditProfile(EditProfileVM model)
{
    if (!ModelState.IsValid)
        return View(model);

    var user = await _userManager.GetUserAsync(User);
    if (user == null)
        return NotFound();

    user.FirstName = model.FirstName;
    user.LastName = model.LastName;
    user.Email = model.Email;

    var result = await _userManager.UpdateAsync(user);
    if (result.Succeeded)
    {
        TempData["Success"] = "Profil güncellendi.";
        return RedirectToAction("Profile"); // or wherever you want to go
    }

    foreach (var error in result.Errors)
    {
        ModelState.AddModelError("", error.Description);
    }

    return View(model);
}

		public async Task<IActionResult> Profile()
		{
			
			var user = await _userManager.GetUserAsync(User);
			if (user == null) return NotFound();

			// Siparişleri getir
			var orders = await _context.Orders
				.Where(o => o.UserId == user.Id)
				.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.Product) 
				.OrderByDescending(o => o.OrderDate)
				.Take(10)
				.ToListAsync();

			// Modeli oluştur
			var model = new UserProfileVM
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				Address = user.Address,
				PhoneNumber = user.PhoneNumber,
				Orders = orders.Select(o => new OrderVM
				{
					Id = o.Id,
					OrderDate = o.OrderDate,
					TotalPrice = o.TotalPrice,
					Status = o.Status,
					OrderItems = o.OrderItems.Select(oi => new OrderItemVM
					{
						ProductName = oi.Product.Name,
						Quantity = oi.Quantity,
						UnitPrice = oi.UnitPrice
					}).ToList()
				}).ToList()
			};

			return View(model);
		}
	}



	}

