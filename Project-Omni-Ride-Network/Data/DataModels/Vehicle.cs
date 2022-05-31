using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Project_Omni_Ride_Network {

    [Table("Vehicle")]
    public class Vehicle {

        [Key]
        public string VehicleId { get; set; }

        [Required]
        public int Category { get; set; }

        [Required]
        [MaxLength(20)]
        public string Firm { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        [MaxLength(15)]
        public string Brand { get; set; }

        [Required]
        [MaxLength(20)]
        public string Model { get; set; }

        [Required]
        [MaxLength(10)]
        public string Color { get; set; }

        [Required]
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


    }
}
