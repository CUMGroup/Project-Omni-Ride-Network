
namespace Project_Omni_Ride_Network {
    public static class Routes {

        public const string ACTION_SUFFIX = "action";

        #region Error Routes

        public const string ERROR_404 = "error/404";
        public const string ERROR_GENERIC = "error/{code:int}";
        #endregion

        #region Vehicle routes

        public const string OVERVIEW = "overview";
        public const string BOOKING = "booking/{id}";
        public const string BOOKING_DONE = "bookingdone";

        #region API Routes
        public const string FILTERED_VEHICLES = "filteredvehicles";
        public const string VEHICLE_ADD = "addvehicle";
        public const string VEHICLE_REMOVE = "removevehicle";
        public const string ORDER_DEL = "deleteorder";
        #endregion
        #endregion

        #region User Routes

        public const string LOGIN = "login";
        public const string LOGOUT = "logout";
        public const string REGISTER = "register";

        public const string PROFILE = "profile";
        public const string RATING = "rating";

        public const string DELETE_USER = "deleteuser";

        #region API Routes
        public const string FILTERED_RATINGS = "filteredratings";
        public const string REGISTER_ADMIN = "registeradmin";
        #endregion
        #endregion

        #region Other Routes

        public const string CONTACT = "contact";
        public const string CONTACT_DONE = "mailsent";
        public const string KARRIERE = "karriere";
        public const string DATENSCHUTZ = "datenschutz";
        public const string PARTNER = "partner";
        public const string IMPRESSUM = "impressum";
        public const string AGB = "agb";
        #endregion
    }
}
