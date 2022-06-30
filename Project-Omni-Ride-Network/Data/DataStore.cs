using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Project_Omni_Ride_Network {
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

        public async Task EnsureDataStoreAsync() {
            await dbContext.Database.EnsureCreatedAsync();
        }

        public void EnsureDataStore() {
            dbContext.Database.EnsureCreated();
        }

        #region Customer

        public async Task AddCustomerAsync(Customer c) {
            if (c == null || c.User == null)
                throw new DatabaseAPIException("Cannot add undefinded Customer to Database");

            dbContext.Customers.Add(c);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<Customer>> GetCustomersAsync() {
            return dbContext.Customers.ToList();
        }


        public async Task<bool> RemoveCustomerAsync(ApplicationUser user) {
            if (String.IsNullOrWhiteSpace(user?.Id)) {
                throw new DatabaseAPIException("UserId can't be null when removing");
            }

            var applUser = dbContext.Users.Where(e => e.Id.Equals(user.Id));
            if (applUser.Any()) {
                dbContext.Users.Remove(applUser.First()); 
                await dbContext.SaveChangesAsync();
            }
            var rating = dbContext.Rating.Where(r => r.UserId.Equals(user.Id));
            if (rating.Any()) {
                dbContext.Rating.Remove(rating.First());
                await dbContext.SaveChangesAsync();
            }
            var customer = dbContext.Customers.Where(e => e.UserId.Equals(user.Id));
            if (customer.Any()) {
                dbContext.Customers.Remove(customer.First());
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
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

        public async Task<bool> RemoveOrderAsync(string id) {
            if (String.IsNullOrEmpty(id)) {
                throw new DatabaseAPIException("OrderId can't be null when removing");
            }

            var order = dbContext.Orders.Where(e => e.OrderId.Equals(id));
            if (order.Any()) {
                dbContext.Orders.Remove(order.First());
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        #endregion

        #region Ratings

        public async Task<Rating> AddRatingAsync(Rating r) {
            if (r == null || r.User == null)
                throw new DatabaseAPIException("User can't be null");

            r.UserId = r.User.UserId;

            if(dbContext.Rating.Where(e => e.UserId.Equals(r.UserId)).Any()) {
                throw new DatabaseAPIException("User already made a Review");
            }

            dbContext.Rating.Add(r);
            await dbContext.SaveChangesAsync();
            return r;
        }

        public async Task<List<Rating>> GetRatingsAsync() {
            return dbContext.Rating.Include(e => e.User).ToList();
        }

        #endregion

        #endregion
    }
}
