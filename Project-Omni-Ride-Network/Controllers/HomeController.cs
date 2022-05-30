using Microsoft.AspNetCore.Mvc;

namespace Project_Omni_Ride_Network {

    public class HomeController : Controller {

        public HomeController() {

        }

        public IActionResult Index() {
            return View();
        }

        #region Vehicle Information Routes

        [Route("overview")]
        public IActionResult Overview() {
            return View();
        }

        [Route("booking/{id}")]
        public IActionResult Booking(string id) {
            //TODO Handle id -> Give to View Model
            return View();
        }

        #endregion

        #region User Specific Routes

        [Route("login")]
        public IActionResult Login() {
            return View();
        }

        [Route("register")]
        public IActionResult Register() {
            return View();
        }

        [Route("profile")]
        public IActionResult Profile() {
            return View();
        }

        [Route("rating")]
        public IActionResult Rating() {
            return View();
        }

        #endregion

        #region Page Information Routes

        [Route("contact")]
        public IActionResult Contact() {
            return View();
        }

        [Route("partner")]
        public IActionResult Partner() {
            return View();
        }

        [Route("impressum")]
        public IActionResult Impressum() {
            return View();
        }

        [Route("agb")]
        public IActionResult AGB() {
            return View();
        }

        #endregion
    }
}