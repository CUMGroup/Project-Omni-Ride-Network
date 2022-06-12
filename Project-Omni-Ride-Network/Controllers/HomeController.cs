using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Project_Omni_Ride_Network {

    public class HomeController : Controller {

        private readonly DataStore dbStore;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(DataStore dbStore, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) {
            this.dbStore = dbStore;
            dbStore.EnsureDataStore();

            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<BaseViewModel> PrepareBaseViewModel() {
            bool authorized = User.Identity.IsAuthenticated;
            string name = "";
            if(authorized) {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);
                var customer = (await dbStore.GetCustomersAsync()).Where(e => e.UserId.Equals(user.Id));
                if(customer.Count() > 0)
                    name = customer.First().KdName + " " + customer.First().KdSurname;
            }
            return new BaseViewModel { Authorized = authorized, UserName = name };
        }

        public async Task<IActionResult> Index() {
            return View(await PrepareBaseViewModel());
        }


        #region Error Routes

        [Route(Routes.ERROR_404)]
        public async Task<IActionResult> Error404() {
            return View(await PrepareBaseViewModel());
        }

        [Route(Routes.ERROR_GENERIC)]
        public async Task<IActionResult> Error(int code) {
            TempData["ErrorCode"] = code;
            return View(await PrepareBaseViewModel ());
        }

        #endregion

        #region Vehicle Information Routes

        [Route(Routes.OVERVIEW)]
        public async Task<IActionResult> Overview() {
            List<Vehicle> vehicles = await dbStore.GetAllVehiclesAsync();
            var modelList = vehicles.Select(m => m.Model).Distinct().OrderBy(e => e).ToList();
            var brandList = vehicles.Select(b => b.Brand).Distinct().OrderBy(e => e).ToList();
            return View(new OverviewViewModel(await PrepareBaseViewModel()) { 
                Vehicles = vehicles,
                ModelFilterList = modelList,
                BrandFilterList = brandList
            });
        }

        [Route(Routes.BOOKING)]
        public async Task<IActionResult> Booking(string id) {
            List<Vehicle> vehicles = await dbStore.GetAllVehiclesAsync();
            var veh = vehicles.Where(e => id.Equals(e.VehicleId));
            if (veh.Any())
                return View(new BookingViewModel(await PrepareBaseViewModel (), veh.First()));
            else
                return NotFound(await PrepareBaseViewModel ());
        }

        [Route(Routes.BOOKING)]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string id) {
            // TODO check form and place order in db
            return Ok(await PrepareBaseViewModel ());
        }

        #endregion

        #region User Specific Routes

        [Route(Routes.LOGIN)]
        public async Task<IActionResult> Login(string ReturnUrl) {
            ViewData["ReturnUrl"] = ReturnUrl ?? "";
            return View(await PrepareBaseViewModel ());
        }

        [HttpPost]
        [Route(Routes.LOGIN + Routes.ACTION_SUFFIX)]
        public async Task<IActionResult> LoginAction([FromForm] LoginApiModel loginModel, string ReturnUrl) {

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            var res = await signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, false, false);

            string decodedUrl = "";
            if (!string.IsNullOrWhiteSpace(ReturnUrl))
                decodedUrl = HttpUtility.UrlDecode(ReturnUrl);

            if (res.Succeeded) {
                if (Url.IsLocalUrl(decodedUrl))
                    return Redirect(decodedUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            return Unauthorized(await PrepareBaseViewModel ());
         }

        [Route(Routes.REGISTER)]
        public async Task<IActionResult> Register() {
            return View(await PrepareBaseViewModel ());
        }

        [Route(Routes.PROFILE)]
        public async Task<IActionResult> Profile() {
            return View(await PrepareBaseViewModel ());
        }

        [Route(Routes.RATING)]
        public async Task<IActionResult> Rating() {
            List<Rating> ratings = await dbStore.GetRatingsAsync();
            return View(new RatingViewModel(await PrepareBaseViewModel()) { Ratings = ratings });
        }

        [Route(Routes.RATING)]
        [HttpPost]
        public async Task<IActionResult> AddRating() {
            // TODO check form and add rating to db
            return Ok(await PrepareBaseViewModel());
        }

        #endregion

        #region Page Information Routes

        [Route(Routes.CONTACT)]
        public async Task<IActionResult> Contact() {
            return View(await PrepareBaseViewModel ());
        }

        [Route(Routes.KARRIERE)]
        public async Task<IActionResult> Karriere() {
            return View(await PrepareBaseViewModel ());
        }

        [Route(Routes.DATENSCHUTZ)]
        public async Task<IActionResult> Datenschutz() {
            return View(await PrepareBaseViewModel ());
        }

        [Route(Routes.PARTNER)]
        public async Task<IActionResult> Partner() {
            return View(await PrepareBaseViewModel ());
        }

        [Route(Routes.IMPRESSUM)]
        public async Task<IActionResult> Impressum() {
            return View(await PrepareBaseViewModel ());
        }

        [Route(Routes.AGB)]
        public async Task<IActionResult> AGB() {
            return View(await PrepareBaseViewModel ());
        }

        #endregion
    }
}