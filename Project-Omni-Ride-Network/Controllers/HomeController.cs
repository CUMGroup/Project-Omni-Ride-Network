using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace Project_Omni_Ride_Network {

    //endpoints: /<ENDPOINT>
    public class HomeController : Controller {

        #region init
        // Database Store
        private readonly DataStore dbStore;
        // Authentication Manager
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        // App Config
        private readonly IConfiguration configuration;
        // Email functions
        private readonly Mailer mailer;

        public HomeController(DataStore dbStore, SignInManager<ApplicationUser> signInManager, 
                UserManager<ApplicationUser> userManager, Mailer mailer, IConfiguration configuration) {
            this.dbStore = dbStore;
            dbStore.EnsureDataStore();

            this.signInManager = signInManager;
            this.userManager = userManager;
            this.mailer = mailer;
            this.configuration = configuration;
        }

        #endregion

        /// <summary>
        /// Preperation method for BaseViewModel
        /// </summary>
        /// <returns> BaseViewModel </returns>
        public async Task<BaseViewModel> PrepareBaseViewModel() {
            bool authorized = User.Identity.IsAuthenticated;
            string name = "";
            // check if user is authenticated
            if (authorized) {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                if(user == null) {
                    authorized = false;
                } else {
                    var customer = (await dbStore.GetCustomersAsync()).Where(e => e.UserId.Equals(user.Id));
                    if (customer.Count() > 0)
                        name = customer.First().KdName + " " + customer.First().KdSurname;
                }
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
            return View(await PrepareBaseViewModel());
        }

        #endregion

        #region Vehicle Information Routes

        /// <summary>
        /// Endpoint to Overview for all vehicles
        /// </summary>
        /// <returns> Overviewviewmodel </returns>
        [Route(Routes.OVERVIEW)]
        public async Task<IActionResult> Overview() {
            // Get all Vehicles from Database and hand over to View
            List<Vehicle> vehicles = await dbStore.GetAllVehiclesAsync();
            var modelList = vehicles.Select(m => m.Model).Distinct().OrderBy(e => e).ToList();
            var brandList = vehicles.Select(b => b.Brand).Distinct().OrderBy(e => e).ToList();
            return View(new OverviewViewModel(await PrepareBaseViewModel()) {
                Vehicles = vehicles,
                ModelFilterList = modelList,
                BrandFilterList = brandList
            });
        }

        /// <summary>
        /// Endpoint to the bookingview of a vehicle with specific VehicleId
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Bookingviewmodel for specific vehicle </returns>
        [Route(Routes.BOOKING)]
        public async Task<IActionResult> Booking(string id) {

            //Get Vehicle from id and hand over to view
            List<Vehicle> vehicles = await dbStore.GetAllVehiclesAsync();
            var veh = vehicles.Where(e => id.Equals(e.VehicleId));
            if (veh.Any())
                return View(new BookingViewModel(await PrepareBaseViewModel(), veh.First()));
            else
                return NotFound(await PrepareBaseViewModel());
        }

        /// <summary>
        /// Endpoint to logical booking action 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orderModel"></param>
        /// <returns> Bookingdoneview </returns>
        [Route(Routes.BOOKING + "/bookingaction")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlaceOrder(string id, [FromForm] Order orderModel) {

            // if user is not logged in -> Loginpage
            if (!User.Identity.IsAuthenticated)
                return RedirectToRoute("Login", "Home");

            // Get user from Email
            var user = await userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null)
                return RedirectToRoute("Login", "Home");
            var customer = (await dbStore.GetCustomersAsync()).Where(e => e.UserId.Equals(user.Id));
            if (customer == null || !customer.Any())
                return RedirectToRoute("Login", "Home");
            orderModel.User = customer.First();

            // Get vehicle from VehicleId
            var vehicle = (await dbStore.GetAllVehiclesAsync()).Where(e => e.VehicleId.Equals(id));
            if (vehicle == null || !vehicle.Any())
                return NotFound();
            orderModel.Vehicle = vehicle.First();

            // Calculate totalprice
            orderModel.Totalprice = PriceCalc.CalculateTotalprice(orderModel);

            // Create corresponding Order entry
            try {
                await dbStore.AddOrderAsync(orderModel);
            } catch (DatabaseAPIException e) {
                return await Error(418);
            }

            // send confirmation mail
            _ = mailer.MailerAsync(configuration.GetValue<string>("MailCredentials:Email"), user.Email, MailTxt.CreateOrderSubject(orderModel),
                MailTxt.CreateOrderResponse(orderModel));
            return RedirectToAction("BookingDone", "Home");
        }

        [Route(Routes.BOOKING_DONE)]
        public async Task<IActionResult> BookingDone() {
            return View(await PrepareBaseViewModel());
        }

        #endregion

        #region User Specific Routes

        /// <summary>
        /// Endpoint to loginpage
        /// </summary>
        /// <param name="response"></param>
        /// <param name="ReturnUrl"></param>
        /// <returns> View </returns>
        [Route(Routes.LOGIN)]
        public async Task<IActionResult> Login(ApiResponse response, string ReturnUrl) {
            // resets the user to the access page after login
            ViewData["ReturnUrl"] = ReturnUrl ?? "";
            if (response == null || response.Status == null || response.Message == null)
                response = new ApiResponse { Status = "100", Message = "" };
            ViewData["ApiStatus"] = response.Status;
            ViewData["ApiMessage"] = response.Message;
            return View(await PrepareBaseViewModel());
        }

        /// <summary>
        /// Endpoint to loginaction
        /// </summary>
        /// <param name="loginModel"></param>
        /// <param name="ReturnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(Routes.LOGIN + Routes.ACTION_SUFFIX)]
        public async Task<IActionResult> LoginAction([FromForm] LoginApiModel loginModel, string ReturnUrl) {

            // delete sessioncookie
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            // sign in via mail and password
            var res = await signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, false, false);

            // get returnurl
            string decodedUrl = "";
            if (!string.IsNullOrWhiteSpace(ReturnUrl))
                decodedUrl = HttpUtility.UrlDecode(ReturnUrl);

            // if returnurl equals bookingaction-URL redirect to booking of vehicle
            // else redirect to indexpage
            if (res.Succeeded) {
                if (Url.IsLocalUrl(decodedUrl)) {
                    var u = decodedUrl.Split('/');
                    if (u != null && u.Length == 4 && u[1].ToLower().Equals("booking") && u[3].ToLower().Equals("bookingaction"))
                        decodedUrl = "/" + u[1] + "/" + u[2];
                    return Redirect(decodedUrl);
                } else
                    return RedirectToAction("Index", "Home");
            }
            // login failed -> redirect to home
            return RedirectToAction("Login", "Home", new ApiResponse { Status = "Error", Message = "User information incorrect" });
        }

        [Route(Routes.LOGOUT + Routes.ACTION_SUFFIX)]
        public async Task<IActionResult> LogoutAction() {
            // delete session cookie
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        [Route(Routes.REGISTER)]
        public async Task<IActionResult> Register(ApiResponse response) {
            // check if http status is okay or ready do continue
            if (response == null || response.Status == null || response.Message == null)
                response = new ApiResponse { Status = "100", Message = "" };
            ViewData["ApiStatus"] = response.Status;
            ViewData["ApiMessage"] = response.Message;
            return View(await PrepareBaseViewModel());
        }

        /// <summary>
        /// Endpoint to Registeraction
        /// </summary>
        /// <param name="model"></param>
        /// <returns>  </returns>
        [HttpPost]
        [Route(Routes.REGISTER + Routes.ACTION_SUFFIX)]
        public async Task<IActionResult> RegisterAction([FromForm] RegisterApiModel model) {

            // check if user already exists
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return RedirectToAction("Register", "Home", new ApiResponse { Status = "Error", Message = "User already exists!" });

            // Create Application User
            ApplicationUser user = new ApplicationUser() {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email
            };
            {   
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return RedirectToAction("Register", "Home", 
                        new ApiResponse { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            }

            // Create corresponding Customer entry
            Customer customer = new Customer {
                KdBirth = model.KdBirth,
                KdName = model.KdName,
                KdSurname = model.KdSurname,
                KdTitle = model.KdTitle,
                Paymnt = model.Paymnt,
                User = user,
                UserId = await userManager.GetUserIdAsync(user)
            };
            try {
                await dbStore.AddCustomerAsync(customer);
            } catch (DatabaseAPIException) {
                return RedirectToAction("Register", "Home", new ApiResponse { Status = "Error", Message = "Error creating the User" });
            }

            // send register mail
            _ = mailer.MailerAsync(configuration.GetValue<string>("MailCredentials:Email"), model.Email, MailTxt.REGISTRY_SUBJ,
                MailTxt.CreateRegistryResponse(model.KdTitle, model.KdSurname));
            return await LoginAction(new LoginApiModel { Email = model.Email, Password = model.Password }, null);
        }

        /// <summary>
        /// Endpoint Profile
        /// </summary>
        /// <returns>  </returns>
        [Route(Routes.PROFILE)]
        [Authorize]
        public async Task<IActionResult> Profile() {
            // User not authentiated -> Indexpage
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            var user = await userManager.FindByEmailAsync(User.Identity.Name);
            // user email couldn't find -> Indexpage
            if (user == null) {
                return RedirectToAction("Index", "Home");
            }
            // no customer could find -> Indexpage
            var customer = (await dbStore.GetCustomersAsync()).Where(e => e.UserId.Equals(user.Id));
            if (customer == null || customer.Count() == 0) {
                return RedirectToAction("Index", "Home");
            }
            return View(new ProfileViewModel(await PrepareBaseViewModel(), customer.First()));
        }

        /// <summary>
        /// Endpoint to remove a user for API Clients.
        /// Must have the authorization of the to be deleted user!
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(Routes.DELETE_USER)]
        [Authorize]
        public async Task<IActionResult> RemoveUser() {
            // try to delete user, if not possible -> unautorized
            try {
                var a = await userManager.FindByEmailAsync(User.Identity.Name);
                if (a == null) {
                    return Unauthorized();
                }
                // remove customer as well as corresponding AspNetUser and all Orders of these costumer
                await dbStore.RemoveCustomerAsync(a);
            } catch (DatabaseAPIException) {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = "Error on deleting customer" });
            }
            // delete session cookie
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Endpoint to ratingsview
        /// </summary>
        /// <returns></returns>
        [Route(Routes.RATING)]
        public async Task<IActionResult> Rating() {
            // Get ratings and calculate ratingbars
            List<Rating> ratings = await dbStore.GetRatingsAsync();
            int[] count = { ratings.Where(e => e.Stars == 1).Count(), ratings.Where(e => e.Stars == 2).Count(), ratings.Where(e => e.Stars == 3).Count(), 
                ratings.Where(e => e.Stars == 4).Count(), ratings.Where(e => e.Stars == 5).Count() };
            int sumCount = count[0] + count[1] + count[2] + count[3] + count[4];
            if(sumCount == 0) sumCount = 1;
            int[] dist = { CalcRatingDistribution(count[0], sumCount), CalcRatingDistribution(count[1], sumCount), 
                CalcRatingDistribution(count[2], sumCount), CalcRatingDistribution(count[3], sumCount), CalcRatingDistribution(count[4], sumCount) };

            // check if user is authenticated -> only authenticated users have permission to rate
            bool userAlreadyReviewed = false;
            if(User.Identity.IsAuthenticated) {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);
                if (user != null)
                    userAlreadyReviewed = ratings.Where(e => e.UserId.Equals(user.Id)).Any();
            }

            return View(new RatingViewModel(await PrepareBaseViewModel()) { RatingCounts = count, RatingDistribution = dist, UserAlreadyReviewed=userAlreadyReviewed});
        }

        //Helpermethod for calculating ratingbar
        private int CalcRatingDistribution(int starCount, int totalSum) {
            return (int)Math.Ceiling((starCount / (double)totalSum) * 100);
        }

        /// <summary>
        /// Endpoint to ratingaction
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route(Routes.RATING)]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RatingAction([FromForm]RatingApiModel model) {
            // check if user is authenticated -> only authenticated users have permission to rate
            var user = await userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null)
                return Unauthorized();
            var customer = (await dbStore.GetCustomersAsync()).Where(e => e.UserId.Equals(user.Id)).FirstOrDefault();
            if (customer == null)
                return Unauthorized();
            // check if user already reviewed -> only one review per user
            if ((await dbStore.GetRatingsAsync()).Where(e => e.UserId.Equals(user.Id)).Any())
                return Unauthorized();

            // create rating entry
            var r = new Rating() { CmntTime = DateTime.Now, Comment = model.Comment, Stars = model.Stars % 6, User = customer, UserId = user.Id };
            try {
                await dbStore.AddRatingAsync(r);
            } catch (DatabaseAPIException) { 
                return Unauthorized();
            }
            return Ok();
        }

        #endregion

        #region Page Information Routes

        [Route(Routes.KARRIERE)]
        public async Task<IActionResult> Karriere() {
            return View(await PrepareBaseViewModel());
        }

        [Route(Routes.DATENSCHUTZ)]
        public async Task<IActionResult> Datenschutz() {
            return View(await PrepareBaseViewModel());
        }

        [Route(Routes.PARTNER)]
        public async Task<IActionResult> Partner() {
            return View(await PrepareBaseViewModel());
        }

        [Route(Routes.IMPRESSUM)]
        public async Task<IActionResult> Impressum() {
            return View(await PrepareBaseViewModel());
        }

        [Route(Routes.AGB)]
        public async Task<IActionResult> AGB() {
            return View(await PrepareBaseViewModel());
        }

        #endregion

        #region Kontakt

        [Route(Routes.CONTACT)]
        public async Task<IActionResult> Contact() {
            return View(await PrepareBaseViewModel());
        }

        /// <summary>
        /// Endpoint to contactaction
        /// get mail from customer and send response mail
        /// </summary>
        /// <param name="contact"></param>
        /// <returns> ContactDone View </returns>
        [HttpPost]
        [Route(Routes.CONTACT + Routes.ACTION_SUFFIX)]
        public IActionResult ContactAction([FromForm] ContactModel contact) {
            // validate contact model 
            if (ModelState.IsValid) {
                var ourMail = configuration.GetValue<String>("MailCredentials:Email");
                var senderMail = contact.SenderEmail;
                var subject = contact.Subject;
                var mailText = ("<html><body><p>" + "Name: " + contact.SenderName + "<br>" + "E-Mail: " 
                    + contact.SenderEmail + "<br>" + contact.Message + "</p></body></html>");

                // try to send mail to us and responsemail to customer
                // if success -> ContactDone view
                try {
                    _ = mailer.MailerAsync(ourMail, ourMail, subject, mailText.ToString());
                    _ = mailer.MailerAsync(ourMail, senderMail, "Ihr Anliegen: " + subject, MailTxt.CreateServiceResponse(contact.SenderName));
                } catch (Exception) {
                    return RedirectToAction("Contact", "Home", new ApiResponse { Status = "Error", Message = "Send failed"});
                }
                return RedirectToAction("ContactDone", "Home");
            }
            return RedirectToAction("Contact","Home", new ApiResponse { Status = "Error", Message = "Bad Request" });
        }

        [Route(Routes.CONTACT_DONE)]
        public async Task<IActionResult> ContactDone() {
            return View(await PrepareBaseViewModel());
        }

        #endregion

    }
}