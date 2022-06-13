using System;
using System.ComponentModel.DataAnnotations;

namespace Project_Omni_Ride_Network {
    public class RegisterApiModel {

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(4)]
        public string KdTitle { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [MaxLength(20)]
        public string KdSurname { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [MaxLength(15)]
        public string KdName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime KdBirth { get; set; }

        [Required(ErrorMessage = "Payment option is required")]
        public int Paymnt { get; set; }
    }
}
