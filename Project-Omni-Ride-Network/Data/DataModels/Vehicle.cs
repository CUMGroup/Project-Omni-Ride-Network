using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Project_Omni_Ride_Network.Data.DataModels {

    [Table("Vehicle")]
    public class Vehicle {

        //[Required]
        //public string VehicleId { get; set; }
        // Querverweis auf Objekt

        public int Category { get; set; }

        public string Firm { get; set; }

        public int Type { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

        public string Plate { get; set; }

        public float BasicPrice { get; set; }

        public float PriceHD { get; set; }

        public string PriceInsu { get; set; }


        public string PathToImg { get; set; }


    }
}
