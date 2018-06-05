using System.Diagnostics;
using AspNetCoreMultitenant.Web.Data;
using AspNetCoreMultitenant.Web.Extensions;
using AspNetCoreMultitenant.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMultitenant.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public HomeController(ApplicationDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            _context.Users.Add(new IdentityUser());
            ViewData["Message"] = _context.TenantId + ";" + _tenantProvider.GetTenant().Id;
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = HttpContext.Request.Host.ToString();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
