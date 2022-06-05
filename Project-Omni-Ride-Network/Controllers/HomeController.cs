using Microsoft.AspNetCore.Mvc;

namespace Project_Omni_Ride_Network {

    public class HomeController : Controller {

        public HomeController() {

        }

        public IActionResult Index() {
            return View();
        }

        #region Error Routes

        [Route("error/404")]
        public IActionResult Error404() {
            return View();
        }

        [Route("error/{code:int}")]
        public IActionResult Error(int code) {
            // TODO maybe handle different error codes
            return View();
        }

        #endregion

        #region Vehicle Information Routes

        [Route("overview")]
        public IActionResult Overview() {
            return View();
        }

        [Route("booking/{id}")]
        public IActionResult Booking(string id) {
            //TODO Handle id -> Give to View Model
            BookingViewModel test = new BookingViewModel(new Vehicle() {
                VehicleId = id,
                Type = 1,
                BasicPrice = 50,
                Brand = "Subuwu",
                Model = "WRX STI",
                Category = 1,
                Color = "Korallblau 2",
                Firm = "Bubatz SE",
                Plate = "SUB-UWU",
                PriceHD = 2.71828f,
                PriceInsu = 3.1415f,
                PathToImg = "~/images/icon.png"
                
            }) ;
            return View(test);
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

        [Route("karriere")]
        public IActionResult Karriere() {
            return View();
        }

        [Route("datenschutz")]
        public IActionResult Datenschutz() {
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