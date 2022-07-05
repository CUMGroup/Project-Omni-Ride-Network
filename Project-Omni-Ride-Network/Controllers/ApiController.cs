using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Project_Omni_Ride_Network {
    // endpoints:  /api/<ENDPOINT>
    [Route("api")]
    [ApiController]
    public class ApiController : Controller {

        #region Init
        // Authentication Manager
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        // App Config
        private readonly IConfiguration _configuration;
        // Database store
        private readonly DataStore dbStore;
        // Email functions
        private readonly Mailer mailer;
        private readonly MailTxt mailTxt;

        public ApiController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, 
            IConfiguration config, DataStore dbStore, Mailer mailer, MailTxt mailTxt) {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = config;
            this.dbStore = dbStore;
            this.mailer = mailer;
            this.mailTxt = mailTxt;   
        }

        #endregion

        #region Endpoints

        #region Authentication

        /// <summary>
        /// Login Endpoint for Api clients
        /// </summary>
        /// <param name="model">Login Credentials</param>
        /// <returns>Logintoken</returns>
        [HttpPost]
        [Route(Routes.LOGIN)]
        public async Task<IActionResult> Login([FromBody] LoginApiModel model) {
            var user = await userManager.FindByEmailAsync(model.Email);
            // Check user password if exists
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password)) {

                var userRoles = await userManager.GetRolesAsync(user);
                // Default claims
                var authClaims = new List<Claim> {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                // Custom role claims -> "User", "Admin" roles
                foreach (var userRole in userRoles) {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                // create JWT Token
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });
            }

            return Unauthorized();
        }

        /// <summary>
        /// Register Endpoint for API Clients
        /// </summary>
        /// <param name="model">Register Data</param>
        /// <returns>Ok Response if successful</returns>
        [HttpPost]
        [Route(Routes.REGISTER)]
        public async Task<IActionResult> Register([FromBody] RegisterApiModel model) {
            // User already exists?
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = "User already exists!" });

            // Create new ApplicationUser
            ApplicationUser user = new ApplicationUser() {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email
            };
            {
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = "User creation failed! Please check user details and try again." });
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
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = "Error creating the User" });
            }

            // Send Register mail
            mailer.MailerAsync(_configuration.GetValue<string>("MailCredentials:Email"), model.Email, MailTxt.REGISTRY_SUBJ, 
                mailTxt.CreateRegistryResponse(model.KdTitle, model.KdSurname));
            return Ok(new ApiResponse { Status = "Success", Message = "User created successfully!" });

        }

        /// <summary>
        /// Register Endpoint for API Clients to register an Admin.
        /// Can only be called with an admin authorization Token
        /// </summary>
        /// <param name="model">Register data</param>
        /// <returns>Ok status if successful</returns>
        [HttpPost]
        [Route(Routes.REGISTER_ADMIN)]
        [AuthorizeToken(Roles = UserRoles.Admin)]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterApiModel model) {
            // User already exists?
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = "User creation failed: User already exists" });

            // Create Application User
            ApplicationUser user = new ApplicationUser {
                Email = model.Email,
                UserName = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            {
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = "User creation failed! Please check user details and try again." });

                // Manage roles
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                if (await roleManager.RoleExistsAsync(UserRoles.Admin)) {
                    await userManager.AddToRoleAsync(user, UserRoles.Admin);
                }

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
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = "Error creating the User" });
            }

            // Send register mail
            mailer.MailerAsync(_configuration.GetValue<string>("MailCredentials:Email"), model.Email, MailTxt.REGISTRY_SUBJ, 
                mailTxt.CreateRegistryResponse(model.KdTitle, model.KdSurname));
            return Ok(new ApiResponse { Status = "Success", Message = "User created successfully!" });
        }

        /// <summary>
        /// Endpoint to remove a user for API Clients.
        /// Must have the authorization token of the to be deleted user!
        /// </summary>
        /// <returns>Ok status if successful</returns>
        [HttpDelete]
        [Route(Routes.DELETE_USER)]
        [AuthorizeToken]
        public async Task<IActionResult> RemoveUser() {
            try {
                var a = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
                if (a == null) {
                    return Unauthorized();
                }
                await dbStore.RemoveCustomerAsync(a);
            } catch (DatabaseAPIException) {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = "Error on deleting customer" });
            }

            return Ok(new ApiResponse { Status = "Success", Message = "User deleted successfully!" });
        }

        #endregion

        #region Vehicles
        /// <summary>
        /// Endpoint to remove a vehicle from the database.
        /// Must have authorization token with admin role
        /// </summary>
        /// <param name="v">Vehicle to remove</param>
        /// <returns>Ok status if successful</returns>
        [HttpDelete]
        [Route(Routes.VEHICLE_REMOVE)]
        [AuthorizeToken(Roles = UserRoles.Admin)]
        public async Task<IActionResult> RemoveVehicle([FromBody] Vehicle v) {
            try {
                await dbStore.RemoveVehicleAsync(v);
            } catch (DatabaseAPIException) {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = "Error on deleting Vehicle" });
            }

            return Ok(new ApiResponse { Status = "Success", Message = "Vehicle deleted successfully!" });
        }

        /// <summary>
        /// Endpoint to add a vehicle to the database.
        /// Must have authorization token with admin role
        /// </summary>
        /// <param name="v">Vehicle to add</param>
        /// <returns>Ok if successful</returns>
        [HttpPost]
        [Route(Routes.VEHICLE_ADD)]
        [AuthorizeToken(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddVehicle([FromBody] Vehicle v) {
            try {
                await dbStore.AddVehicleAsync(v);
            } catch (DatabaseAPIException) {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = "Error on creating Vehicle" });
            }

            return Ok(new ApiResponse { Status = "Success", Message = "Vehicle created successfully!" });
        }

        /// <summary>
        /// Get Endpoint to retrieve a filtered, 20 element page from all vehicles
        /// </summary>
        /// <param name="page">Page to get</param>
        /// <param name="searchTxt">Search filter by model and brand</param>
        /// <param name="categoryFilter">Category filter by sharing and renting</param>
        /// <param name="brandFilter">Filter by a specific brand</param>
        /// <param name="modelFilter">Filter by a specific model</param>
        /// <param name="typeFilter">Filter by a specific vehicle type</param>
        /// <param name="minPrice">Price minimum filter</param>
        /// <param name="maxPrice">Price maximum filter</param>
        /// <returns>Partial Html View, that contains the maximum 20 element, filtered page. You can embed the result via Javascript</returns>
        [HttpGet]
        [Route(Routes.FILTERED_VEHICLES)]
        public async Task<PartialViewResult> GetVehicleListView(int? page, string searchTxt, int? categoryFilter, 
            string brandFilter, string modelFilter, int? typeFilter, float? minPrice, float? maxPrice) {
            // Get all vehicles
            IEnumerable<Vehicle> veh = await dbStore.GetAllVehiclesAsync();

            // if search filter is present -> filter
            if (!String.IsNullOrWhiteSpace(searchTxt))
                veh = veh.Where(e => e.Brand.ToLower().Contains(searchTxt)
                || e.Model.ToLower().Contains(searchTxt)
                || e.Firm.ToLower().Contains(searchTxt));

            // if filter is present -> filter
            if (categoryFilter != null)
                veh = veh.Where(e => e.Category == categoryFilter);
            if (!String.IsNullOrWhiteSpace(brandFilter))
                veh = veh.Where(e => e.Brand.Equals(brandFilter));
            if (!String.IsNullOrWhiteSpace(modelFilter))
                veh = veh.Where(e => e.Model.Equals(modelFilter));
            if (typeFilter != null)
                veh = veh.Where(e => e.Type == typeFilter);
            if (minPrice != null)
                veh = veh.Where(e => e.BasicPrice >= minPrice);
            if (maxPrice != null)
                veh = veh.Where(e => e.BasicPrice <= maxPrice);

            // Calculate the page
            int itemsPerPage = 20;
            int currentPage = page ?? 1;
            if (currentPage <= 0) currentPage = 1;
            int totalItems = veh == null ? 0 : veh.Count();

            int pageCount = totalItems > 0 ? (int)Math.Ceiling(totalItems / (double)itemsPerPage) : 0;
            if (currentPage > pageCount) currentPage = pageCount;

            // Get filtered page view
            if (veh != null & totalItems > 0)
                veh = veh.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

            // Set metadata for view
            ViewData["MaxPage"] = pageCount;
            ViewData["CurrentPage"] = currentPage;

            return PartialView("_overviewList", veh.ToList());
        }
        #endregion

        #region Orders
        /// <summary>
        /// Endpoint to delete an order.
        /// Must have authorization token with admin role
        /// </summary>
        /// <param name="id">orderid to remove</param>
        /// <returns></returns>
        [HttpDelete]
        [Route(Routes.ORDER_DEL)]
        [AuthorizeToken(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteOrder([FromBody]string id) {
            try {
                await dbStore.RemoveOrderAsync(id);
            } catch (DatabaseAPIException) {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = "Error on deleting order"});
            }
            return Ok(new ApiResponse { Status = "Success", Message = "Order deleted successfully!" });
        }

        #endregion

        #region Rating

        /// <summary>
        /// Get Endpoint to retrieve a filtered, 20 element page from all ratings
        /// </summary>
        /// <param name="page">Page to get</param>
        /// <param name="starFilter">Filter by amount of stars</param>
        /// <param name="sortNewest">Sort result by Newest; Null: no sorting; true: Sort by Newest; false: Sort by Oldest</param>
        /// <param name="sortByHighestStars">Sort result by Highest rating; Null: no sorting; true: Sort by best ratings; false: Sort by worst ratings</param>
        /// <returns>Partial Html View, that contains the maximum 20 element, filtered page. You can embed the result via Javascript</returns>
        [HttpGet]
        [Route(Routes.FILTERED_RATINGS)]
        public async Task<PartialViewResult> GetRatingListView(int? page, int? starFilter, bool? sortNewest, bool? sortByHighestStars) {
            // Retrieve all ratings from db
            IEnumerable<Rating> rating = await dbStore.GetRatingsAsync();

            // if filter is not null: filter
            if (starFilter != null && starFilter > 0 && starFilter < 6) {
                rating = rating.Where(e => e.Stars == starFilter);
            }

            // No Ratings? -> Return special view
            if (rating == null || !rating.Any()) {
                ViewData["MaxPage"] = 0;
                ViewData["CurrentPage"] = 0;
                return PartialView("_noRatings");
            }

            // Sort the result set according to the params
            if(sortNewest != null && sortNewest == true) {
                if (sortNewest.Value)
                    rating.OrderByDescending(e => e.CmntTime);
                else
                    rating.OrderBy(e => e.CmntTime);
            }
            if(sortByHighestStars != null) {
                if (sortByHighestStars.Value)
                    rating.OrderByDescending(e => e.Stars);
                else
                    rating.OrderBy(e => e.Stars);
            }

            // Calculate the page
            int itemsPerPage = 20;
            int currentPage = page ?? 1;
            if (currentPage <= 0) currentPage = 1;
            int totalItems = rating == null ? 0 : rating.Count();

            int pageCount = totalItems > 0 ? (int)Math.Ceiling(totalItems / (double)itemsPerPage) : 0;
            if (currentPage > pageCount) currentPage = pageCount;

            // Get filtered page view
            if (rating != null & totalItems > 0)
                rating = rating.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

            // Set metadata for view
            ViewData["MaxPage"] = pageCount;
            ViewData["CurrentPage"] = currentPage;

            return PartialView("_ratingList", rating.ToList());
        }

        #endregion

        #endregion
    }
}
