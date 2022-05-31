using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

using System;

namespace Project_Omni_Ride_Network.Data.DataModels {
    public class Order {

        [Required]
        public int OrderId { get; set; }

        //public string UserId { get; set; }
        // Querverweis auf Objekt

        //public string VehicleId { get; set; }
        // Querverweis auf Objekt

        public DateTime DateTimePickUp { get; set; }

        public DateTime DateTimeReturn { get; set; }

        public int optAdd { get; set; }


    }
}
