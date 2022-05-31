using Dna;
using Microsoft.Extensions.DependencyInjection;

namespace Project_Omni_Ride_Network {

    /// <summary>
    /// Shortcut for the DI Services
    /// </summary>
    public static class DI {

        #region Scoped Instances
        /// <summary>
        /// Scoped instance of the ApplicationDbContext
        /// </summary>
        public static ApplicationDbContext DbContext => Framework.Provider.GetService<ApplicationDbContext>();
        #endregion
    }
}