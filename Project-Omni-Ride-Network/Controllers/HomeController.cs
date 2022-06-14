using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Project_Omni_Ride_Network {

    public class HomeController : Controller {

        private readonly DataStore dbStore;

        public HomeController(DataStore dbStore, IConfiguration conf) {
            this.dbStore = dbStore;
            dbStore.EnsureDataStore();

        }

        public IActionResult Index() {
            return View();
        }


        #region Error Routes

        [Route(Routes.ERROR_404)]
        public IActionResult Error404() {
            return View();
        }

        [Route(Routes.ERROR_GENERIC)]
        public IActionResult Error(int code) {
            TempData["ErrorCode"] = code;
            return View();
        }

        #endregion

        #region Vehicle Information Routes

        [Route(Routes.OVERVIEW)]
        public async Task<IActionResult> Overview() {
            List<Vehicle> vehicles = await dbStore.GetAllVehiclesAsync();
            return View(new OverviewViewModel() { Vehicles = vehicles });
        }

        [Route(Routes.BOOKING)]
        public async Task<IActionResult> Booking(string id) {
            List<Vehicle> vehicles = await dbStore.GetAllVehiclesAsync();
            var veh = vehicles.Where(e => id.Equals(e.VehicleId));
            if (veh.Any())
                return View(new BookingViewModel(veh.First()));
            else
                return NotFound();
        }

        [Route(Routes.BOOKING)]
        [HttpPost]
        public IActionResult PlaceOrder(string id) {
            // TODO check form and place order in db
            return Ok();
        }

        #endregion

        #region User Specific Routes

        [Route(Routes.LOGIN)]
        public IActionResult Login() {
            return View();
        }

        [Route(Routes.REGISTER)]
        public IActionResult Register() {
            return View();
        }

        [Route(Routes.PROFILE)]
        public IActionResult Profile() {
            return View();
        }

        [Route(Routes.RATING)]
        public async Task<IActionResult> Rating() {
            List<Rating> ratings = await dbStore.GetRatingsAsync();
            return View(new RatingViewModel() { Ratings = ratings });
        }

        [Route(Routes.RATING)]
        [HttpPost]
        public IActionResult AddRating() {
            // TODO check form and add rating to db
            return Ok();
        }

        #endregion

        #region Page Information Routes


        [Route(Routes.KARRIERE)]
        public IActionResult Karriere() {
            return View();
        }

        [Route(Routes.DATENSCHUTZ)]
        public IActionResult Datenschutz() {
            return View();
        }

        [Route(Routes.PARTNER)]
        public IActionResult Partner() {
            return View();
        }

        [Route(Routes.IMPRESSUM)]
        public IActionResult Impressum() {
            return View();
        }

        [Route(Routes.AGB)]
        public IActionResult AGB() {
            return View();
        }

        #endregion

        #region Contact


        [Route(Routes.CONTACT)]
        public IActionResult Contact() {
            return View();
        }

        #endregion

    }
}