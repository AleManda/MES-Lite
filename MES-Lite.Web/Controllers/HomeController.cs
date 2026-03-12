using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MES_Lite.Web.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MES_Lite.Web.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger,UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
        _logger = logger;
    }


    //
    //______________________________________________________________________________________________
    public async Task<IActionResult> Index()
    {
        //per ora in home page faccio vedere il ruolo dell'utente se loggato
        var user = await _userManager.GetUserAsync(HttpContext.User);

        if (user != null)
        {
            List<string> roles = (List<string>)await _userManager.GetRolesAsync(user);

            if (roles.Count > 0)
                return View("Index",roles[0]);
        }

        return View();
    }

    //
    //______________________________________________________________________________________________
    public IActionResult Privacy()
    {
        return View();
    }

    //
    //______________________________________________________________________________________________
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
