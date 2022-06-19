using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Project_Omni_Ride_Network {
    public class AuthorizeTokenAttribute : AuthorizeAttribute {

        #region

        public AuthorizeTokenAttribute() {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
        #endregion
    }
}
