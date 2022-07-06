using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Ensures that the Database is created
        /// </summary>
        /// <returns>Task to await</returns>
        public async Task EnsureDataStoreAsync() {
            await dbContext.Database.EnsureCreatedAsync();
        }

        /// <summary>
        /// Ensures that the Database is created
        /// </summary>
        public void EnsureDataStore() {
            dbContext.Database.EnsureCreated();
        }

        #region Customer

        /// <summary>
        /// Adds a Customer to the database
        /// </summary>
        /// <param name="c">Customer to add</param>
        /// <returns>Task to await</returns>
        /// <exception cref="DatabaseAPIException">Thrown if the Customer information is insufficient</exception>
        public async Task AddCustomerAsync(Customer c) {
            if (c == null || c.User == null)
                throw new DatabaseAPIException("Cannot add undefinded Customer to Database");

            dbContext.Customers.Add(c);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieve all Customers
        /// </summary>
        /// <returns>Task to await. Task result is a list of Customer Datamodels</returns>
        public async Task<List<Customer>> GetCustomersAsync() {
            return await dbContext.Customers.ToListAsync();
        }

        /// <summary>
        /// Remove a User from the Database
        /// </summary>
        /// <param name="user">User to remove</param>
        /// <returns>Task to await. Task result is a bool if the operation was successful</returns>
        /// <exception cref="DatabaseAPIException">Thrown if the specified userid is null</exception>
        public async Task<bool> RemoveCustomerAsync(ApplicationUser user) {
            // Valid to delete?
            if (String.IsNullOrWhiteSpace(user?.Id)) {
                throw new DatabaseAPIException("UserId can't be null when removing");
            }

            // Delete all occurrences of the customer/user
            var rating = dbContext.Rating.Where(r => r.UserId.Equals(user.Id));
            if (rating.Any()) {
                dbContext.Rating.Remove(rating.First());
            }

            var customer = dbContext.Customers.Where(e => e.UserId.Equals(user.Id));
            if (customer.Any()) {
                dbContext.Customers.Remove(customer.First());
            }

            var order = dbContext.Orders.Where(r => r.User.UserId.Equals(user.Id));
            foreach(var o in order) {
                dbContext.Orders.Remove(o);
            }

            var applUser = dbContext.Users.Where(e => e.Id.Equals(user.Id));
            if (applUser.Any()) {
                dbContext.Users.Remove(applUser.First()); 
            }
            await dbContext.SaveChangesAsync();
            
            
            return true;
        }


        #endregion

        #region Vehicles

        /// <summary>
        /// Adds a vehicle to the Database
        /// </summary>
        /// <param name="v">Vehicle to add</param>
        /// <returns>Task to await. Task result is the added Vehicle populated with its vehicleId</returns>
        /// <exception cref="DatabaseAPIException"></exception>
        public async Task<Vehicle> AddVehicleAsync(Vehicle v) {
            // Can't add empty vehicle
            if (v == null)
                throw new DatabaseAPIException("Cannot add undefined Vehicle to Database");

            // Generate new GUID, vehicleID
            string vehID = Guid.NewGuid().ToString("N");
            // Check for the tiiiiny chance for a double GUID that already exists and correct it
            bool unique;
            do {
                unique = true;
                if (dbContext.Vehicles.Where(e => e.VehicleId.Equals(vehID)).Any()) {
                    vehID = Guid.NewGuid().ToString("N");
                    unique = false;
                }
            } while (!unique);

            // Add the vehicle
            v.VehicleId = vehID;
            dbContext.Vehicles.Add(v);
            await dbContext.SaveChangesAsync();
            return v;
        }

        /// <summary>
        /// Removes a Vehicle from the database
        /// </summary>
        /// <param name="v">Vehicle to remove</param>
        /// <returns>Task to await. Task result is a bool if the operation was successful</returns>
        /// <exception cref="DatabaseAPIException">Thrown if there is no vehicleId specified</exception>
        public async Task<bool> RemoveVehicleAsync(Vehicle v) {
            if(String.IsNullOrWhiteSpace(v?.VehicleId)) {
                throw new DatabaseAPIException("VehicleID can't be null when removing");
            }

            // Retrieve the Vehicle and delete it (if it was found)
            var veh = dbContext.Vehicles.Where(e => e.VehicleId.Equals(v.VehicleId));
            if (veh.Any()) {
                dbContext.Vehicles.Remove(veh.First());
                await dbContext.SaveChangesAsync();
                return true;
            }
            // No vehicle got found/deleted -> false
            return false;
        }

        /// <summary>
        /// Retrieve all vehicles
        /// </summary>
        /// <returns>Task to await. Task result is a list of Vehicle Datamodels</returns>
        public async Task<List<Vehicle>> GetAllVehiclesAsync() {
            return await dbContext.Vehicles.ToListAsync();
        }

        #endregion

        #region Orders

        /// <summary>
        /// Adds an order to the database
        /// </summary>
        /// <param name="o">Order to add</param>
        /// <returns>Task to await. Task result is the added Order, populated with its orderId</returns>
        /// <exception cref="DatabaseAPIException">Thrown if the order to add or its content is null</exception>
        public async Task<Order> AddOrderAsync(Order o) {
            if(o == null || o.Vehicle == null || o.User == null) {
                throw new DatabaseAPIException("Order is not complete!");
            }

            // Generate GUID, orderId
            string orderID = Guid.NewGuid().ToString("N");
            // Check for the tiiiiny chance for a double GUID that already exists and correct it
            bool unique;
            do {
                unique = true;
                if (dbContext.Orders.Where(e => e.OrderId.Equals(orderID)).Any()) {
                    orderID = Guid.NewGuid().ToString("N");
                    unique = false;
                }
            } while (!unique);

            // add the order
            o.OrderId = orderID;
            dbContext.Orders.Add(o);
            await dbContext.SaveChangesAsync();
            return o;

        }

        /// <summary>
        /// Retrieve all orders
        /// </summary>
        /// <returns>Task to await. Task result is a list of Order Datamodels</returns>
        public async Task<List<Order>> GetOrdersAsync() {
            return await dbContext.Orders.ToListAsync();
        }

        /// <summary>
        /// Removes an order from the database
        /// </summary>
        /// <param name="id">orderId to remove</param>
        /// <returns>Task to await. Task result is a bool if the operation was successful</returns>
        /// <exception cref="DatabaseAPIException">Thrown if the specified orderId is empty</exception>
        public async Task<bool> RemoveOrderAsync(string id) {
            if (String.IsNullOrEmpty(id)) {
                throw new DatabaseAPIException("OrderId can't be null when removing");
            }

            // Retrieve Order and delete it (if it was there)
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

        /// <summary>
        /// Adds a rating to the database
        /// </summary>
        /// <param name="r">Rating to add</param>
        /// <returns>Task to await. Task result is the added Rating</returns>
        /// <exception cref="DatabaseAPIException">Thrown if the User is null or already made a review</exception>
        public async Task<Rating> AddRatingAsync(Rating r) {
            if (r == null || r.User == null)
                throw new DatabaseAPIException("User can't be null");

            r.UserId = r.User.UserId;

            if(dbContext.Rating.Where(e => e.UserId.Equals(r.UserId)).Any()) {
                throw new DatabaseAPIException("User already made a Review");
            }

            // Add Rating
            dbContext.Rating.Add(r);
            await dbContext.SaveChangesAsync();
            return r;
        }

        /// <summary>
        /// Retrieve all Ratings
        /// </summary>
        /// <returns>Task to await. Task result is a list of Rating Datamodels</returns>
        public async Task<List<Rating>> GetRatingsAsync() {
            return await dbContext.Rating.Include(e => e.User).ToListAsync();
        }

        #endregion

        #endregion
    }
}
