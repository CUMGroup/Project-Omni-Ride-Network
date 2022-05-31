using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Project_Omni_Ride_Network {

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {

        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        #region Public Properties
        
        //TODO Add tables

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor, expecting database options passed in
        /// </summary>
        /// <param name="options">The Database context options</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

        }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
    }
}