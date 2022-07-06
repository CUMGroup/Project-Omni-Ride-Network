using System;

namespace Project_Omni_Ride_Network {
    // Exception thrown by the Datastore on invalid database operations
    public class DatabaseAPIException : Exception {

        public DatabaseAPIException(string message) : base(message) {

        }

    }
}
