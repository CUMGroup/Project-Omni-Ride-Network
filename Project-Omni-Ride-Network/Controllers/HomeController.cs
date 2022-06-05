using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Omni_Ride_Network {

    public class HomeController : Controller {

        private readonly DataStore dbStore;

        public HomeController(DataStore dbStore) {
            this.dbStore = dbStore;
            dbStore.EnsureDataStore();
        }

        public IActionResult Index() {
            return View();
        }

        #region DEBUG ROUTES

        [ConditionalAttribute("DEBUG")]
        [Route("addtestdata")]
        public async void AddTestData() {
            await dbStore.AddVehicleAsync(new Vehicle() {
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
            });
        }

        #endregion

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
        public async Task<IActionResult> Overview() {
            List<Vehicle> vehicles = await dbStore.GetAllVehiclesAsync();
            return View(new OverviewViewModel() { Vehicles = vehicles });
        }

        [Route("booking/{id}")]
        public async Task<IActionResult> Booking(string id) {
            List<Vehicle> vehicles = await dbStore.GetAllVehiclesAsync();
            var veh = vehicles.Where(e => id.Equals(e.VehicleId));
            if (veh.Any())
                return View(new BookingViewModel(veh.First()));
            else
                return NotFound();
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