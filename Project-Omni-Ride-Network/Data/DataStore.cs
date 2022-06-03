using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Project_Omni_Ride_Network.Data {
    public class DataStore {

        #region Protected Members

        protected ApplicationDbContext dbContext;

        #endregion

        #region Constructor

        public DataStore(ApplicationDbContext ctx) {
            dbContext = ctx;
        }
        #endregion


        #region Database functions
        // TODO : Add Parameters and Return Values to Database functions;   + Create the APIModels or Datamodels to communicate

        public async Task EnsureDataStoreAsync() {
            await dbContext.Database.EnsureCreatedAsync();
        }

        #region Login

        public async Task CheckLoginAsync() {
            // TODO
        }

        public async Task AddNewUserAsync() {
            // TODO
        }

        public async Task GetCustomersAsync() {
            // TODO
        }

        #endregion

        #region Vehicles

        public async Task AddVehicleAsync() {
            // TODO
        }

        public async Task RemoveVehicleAsync() {
            // TODO
        }

        public async Task GetAllVehiclesAsync() {
            // TODO
        }

        #endregion

        #region Orders

        public async Task AddOrderAsync() {
            //TODO
        }

        public async Task GetOrdersAsync() {
            // TODO
        }

        #endregion

        #region Ratings

        public async Task AddRatingAsync() {
            // TODO
        }

        public async Task GetRatingsAsync() {
            // TODO
        }

        #endregion

        #endregion

        #region Helpers 

        private string HashString(string s) {
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create()) {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(s));

                foreach (byte b in result) {
                    sb.Append(b.ToString("x2"));
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
