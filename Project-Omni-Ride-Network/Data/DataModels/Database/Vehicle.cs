using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Project_Omni_Ride_Network {

    [Table("Vehicle")]
    public class Vehicle {

        [Key]
        public string VehicleId { get; set; }

        // Sharing, Rental, ...
        [Required]
        public int Category { get; set; }
        // Leihe = 1
        // Sharing = 2

        [Required]
        [MaxLength(20)]
        public string Firm { get; set; }

        // Type := Car, Bike, ...
        [Required]
        public int Type { get; set; }
        // PKW = 1
        // Fahrrad = 2
        // E-Scooter = 3
        // Busse/Transporter = 4
        // LKW = 5
        // Panzer = 6

        [Required]
        [MaxLength(15)]
        public string Brand { get; set; }

        [Required]
        [MaxLength(20)]
        public string Model { get; set; }

        [Required]
        [MaxLength(20)]
        public string Color { get; set; }

        [MaxLength(10)]
        public string Plate { get; set; }

        [Required]
        public float BasicPrice { get; set; }

        [Required]
        public float PriceHD { get; set; }

        [Required]
        public float PriceInsu { get; set; }

        [Required]
        [MaxLength(256)]
        public string PathToImg { get; set; }


        public string GetCategoryAsString() => 
            Category switch {
                1 => "Leihe",
                2 => "Sharing",
                _ => "",
            };

    }
}
