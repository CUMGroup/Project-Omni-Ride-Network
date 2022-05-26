using Microsoft.AspNetCore.Mvc;

namespace Project_Omni_Ride_Network {

    public class HomeController : Controller {

        public HomeController() {

        }

        public IActionResult Index() {
            return View();
        }
    }
}