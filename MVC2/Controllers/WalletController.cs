using Microsoft.AspNetCore.Mvc;

namespace MVC2.Controllers
{
    public class WalletController : Controller
    {
        [ActionName("Index")]
        public IActionResult WalletList()
        {
            return View("WalletList");
        }
    }
}
