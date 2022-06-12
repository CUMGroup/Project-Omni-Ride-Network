﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api")]
    [ApiController]
    public class ApiController : Controller {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly DataStore dbStore;

        public ApiController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config, DataStore dbStore) {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._configuration = config;
            this.dbStore = dbStore;
        }

        #region Authentication

        [HttpPost]
        [Route(Routes.LOGIN)]
        public async Task<IActionResult> Login([FromBody]LoginApiModel model) {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password)) {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim> {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach(var userRole in userRoles) {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token),expiration = token.ValidTo}) ;
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route(Routes.REGISTER)]
        public async Task<IActionResult> Register([FromBody] RegisterApiModel model) {
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = "User already exists!" });

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
            }catch (DatabaseAPIException) {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = "Error creating the User" });
            }

            return Ok(new ApiResponse { Status = "Success", Message = "User created successfully!" });

        }

        [HttpPost]
        [Route(Routes.REGISTER_ADMIN)]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterApiModel model) {
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = "User creation failed: User already exists" });
            ApplicationUser user = new ApplicationUser {
                Email = model.Email,
                UserName = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            {
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = "User creation failed! Please check user details and try again." });

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                if (await roleManager.RoleExistsAsync(UserRoles.Admin)) {
                    await userManager.AddToRoleAsync(user, UserRoles.Admin);
                }

            }
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

            return Ok(new ApiResponse { Status = "Success", Message = "User created successfully!" });
        }

        #endregion

        #region Vehicles

        [HttpPost]
        [Route(Routes.VEHICLE_API)]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddVehicle([FromBody] Vehicle v) {
            try {
                await dbStore.AddVehicleAsync(v);
            } catch (DatabaseAPIException) {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = "Error on creating Vehicle" });
            }

            return Ok(new ApiResponse { Status = "Success", Message = "Vehicle created successfully!" });
        }


        [HttpGet]
        [Route(Routes.FILTERED_VEHICLES)]
        public async Task<PartialViewResult> GetVehicleListView(int? page, string searchTxt, int? categoryFilter, string brandFilter, string modelFilter, int? typeFilter, float? minPrice, float? maxPrice) {

            IEnumerable<Vehicle> veh = await dbStore.GetAllVehiclesAsync();

            if(!String.IsNullOrWhiteSpace(searchTxt))
                veh = veh.Where(e => e.Brand.ToLower().Contains(searchTxt)
                || e.Model.ToLower().Contains(searchTxt)
                || e.Firm.ToLower().Contains(searchTxt));

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

            int itemsPerPage = 20;
            int currentPage = page ?? 1;
            if (currentPage <= 0) currentPage = 1;
            int totalItems = veh == null ? 0 : veh.Count();

            int pageCount = totalItems > 0 ? (int)Math.Ceiling(totalItems / (double)itemsPerPage) : 0;
            if (currentPage > pageCount) currentPage = pageCount;

            if(veh != null & totalItems > 0)
                veh = veh.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

            return PartialView("_overviewList", veh.ToList());
        }
        #endregion

    }
}