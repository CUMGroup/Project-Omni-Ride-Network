using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Project_Omni_Ride_Network {
    // Attribute to use for endpoints that need token authorization
    public class AuthorizeTokenAttribute : AuthorizeAttribute {

        public AuthorizeTokenAttribute() {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
