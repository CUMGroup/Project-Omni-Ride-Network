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

        public async Task RemoveUserAsync() {
            //TODO
        }

        #endregion

        #region Vehicles

        public async Task<Vehicle> AddVehicleAsync(Vehicle v) {

            if (v == null)
                throw new DatabaseAPIException("Cannot add undefined Vehicle to Database");

            string vehID = Guid.NewGuid().ToString("N");
            bool unique;
            do {
                unique = true;
                if (dbContext.Vehicles.Where(e => e.VehicleId.Equals(vehID)).Any()) {
                    vehID = Guid.NewGuid().ToString("N");
                    unique = false;
                }
            } while (!unique);
            v.VehicleId = vehID;
            dbContext.Vehicles.Add(v);
            await dbContext.SaveChangesAsync();
            return v;
        }

        public async Task<bool> RemoveVehicleAsync(Vehicle v) {
            if(String.IsNullOrWhiteSpace(v?.VehicleId)) {
                throw new DatabaseAPIException("VehicleID can't be null when removing");
            }

            var veh = dbContext.Vehicles.Where(e => e.VehicleId.Equals(v.VehicleId));
            if (veh.Any()) {
                dbContext.Vehicles.Remove(veh.First());
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Vehicle>> GetAllVehiclesAsync() {
            return dbContext.Vehicles.ToList();
        }

        #endregion

        #region Orders

        public async Task<Order> AddOrderAsync(Order o) {
            if(o == null || o.Vehicle == null || o.User == null) {
                throw new DatabaseAPIException("Order is not complete!");
            }

            string orderID = Guid.NewGuid().ToString("N");
            bool unique;
            do {
                unique = true;
                if (dbContext.Orders.Where(e => e.OrderId.Equals(orderID)).Any()) {
                    orderID = Guid.NewGuid().ToString("N");
                    unique = false;
                }
            } while (!unique);
            o.OrderId = orderID;
            dbContext.Orders.Add(o);
            await dbContext.SaveChangesAsync();
            return o;

        }

        public async Task<List<Order>> GetOrdersAsync() {
            return dbContext.Orders.ToList();
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
