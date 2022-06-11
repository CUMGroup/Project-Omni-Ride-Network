using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Omni_Ride_Network {
    public static class Routes {

        public const string DEBUG_TESTDATA = "addtestdata";

        public const string ERROR_404 = "error/404";
        public const string ERROR_GENERIC = "error/{code:int}";

        public const string OVERVIEW = "overview";
        public const string BOOKING = "booking/{id}";

        public const string LOGIN = "login";
        public const string REGISTER = "register";
        public const string PROFILE = "profile";
        public const string RATING = "rating";

        public const string CONTACT = "contact";
        public const string KARRIERE = "karriere";
        public const string DATENSCHUTZ = "datenschutz";
        public const string PARTNER = "partner";
        public const string IMPRESSUM = "impressum";
        public const string AGB = "agb";


        public const string VEHICLE_API = "addvehicle";
        public const string FILTERED_VEHICLES = "filteredvehicles";
        public const string REGISTER_ADMIN = "registeradmin";
    }
}
