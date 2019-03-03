using System.Diagnostics;
using System.Linq;
using AspNetCoreMultitenant.Web.Data;
using AspNetCoreMultitenant.Web.Extensions;
using AspNetCoreMultitenant.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            _context.Database.EnsureCreated();

            var model = _context.Products
                                .Include(b => b.Category)
                                .ToList();

            return View("ProductList", model);
        }

        public IActionResult Category(int id)
        {
            var model = _context.Categories
                                   .Include(c => c.Products)
                                   .First(c => c.Id == id)
                                   .Products;
            
            return View("ProductList", model);
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
