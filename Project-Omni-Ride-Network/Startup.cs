using Dna;
using Dna.AspNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Project_Omni_Ride_Network {
    public class Startup {

        #region Public Properties

        public IConfiguration Configuration { get; }

        #endregion

        #region Constructor

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        #endregion

        #region Configuration

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            // Add Services for the dependency injection
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(Framework.Construction.Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<DataStore>(provider => new DataStore(provider.GetService<ApplicationDbContext>()));
            services.AddScoped<Mailer>(provider => new Mailer(Configuration));

            // AddIdentity adds cookie based authentication
            // Adds scoped classes for things like UserManager, SignInManager, PasswordHashers...
            // Automatically adds the validated user from a cookie to the HttpContext.User
            // https://github.com/aspnet/Identity/blob/master/src/Identity/IdentityServiceCollectionExtensions.cs
            services.AddIdentity<ApplicationUser, IdentityRole>()

                // Adds UserStore and RoleStore
                // https://github.com/aspnet/Identity/blob/master/src/EF/IdentityEntityFrameworkBuilderExtensions.cs
                .AddEntityFrameworkStores<ApplicationDbContext>()

                //Adds a provider for random keys and hashes
                .AddDefaultTokenProviders();

            // Setup token authentication
            services.AddAuthentication()
            .AddJwtBearer(options => {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters() {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:Audience"],
                    ValidIssuer = Configuration["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            // Setup login cookie
            services.ConfigureApplicationCookie(options => {
                options.LoginPath = $"/{Routes.LOGIN}"; 
                options.ExpireTimeSpan = TimeSpan.FromHours(3); 
            });

            services.AddControllersWithViews();
            services.AddMvc();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider) {

            app.UseDnaFramework();

            app.UseAuthentication();

            // Redirect errors
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Map default endpoints to the home controller
            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        #endregion
    }
}
