using Bilet14.Models;
using Bilet14.ViewModels.AccountVMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bilet14.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    #region Register
    public async Task<IActionResult> Register()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (!ModelState.IsValid) return View();

        ApplicationUser newUser = new ApplicationUser()
        {
            FullName = registerVM.FullName,
            Email = registerVM.Email,
            UserName = registerVM.UserName,
        };

        IdentityResult result = await _userManager.CreateAsync(newUser, registerVM.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
                return View();
            }
        }

        await _userManager.AddToRoleAsync(newUser, "Admin");
        await _signInManager.SignInAsync(newUser, isPersistent: false);
        return RedirectToAction("Index", "Home");
    }
    #endregion

    #region Login
    public async Task<IActionResult> Login()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (!ModelState.IsValid) return View();

        if (loginVM.UserNameOrEmail.Contains('@'))
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Email or Password is wrong!");
                    return View();
                }
            return RedirectToAction("Index", "Home");
            }

        }

        else
        {
            ApplicationUser user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Username or Password is wrong!");
                    return View();
                }
            return RedirectToAction("Index", "Home");
            }


        }
        return View();

     

    }
    #endregion


    #region SignOut
    public async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    #endregion


    public async Task<IActionResult> CreateRole()
    {
        await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
        await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });

       return NoContent();

    }




}
