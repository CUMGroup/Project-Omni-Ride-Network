using System.ComponentModel.DataAnnotations;

namespace Project_Omni_Ride_Network {
    public class LoginApiModel {

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
