using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

using System;

namespace Project_Omni_Ride_Network {
    public class Order {

        [Key]
        public string OrderId { get; set; }

        [Required]
        public Customer User { get; set; }

        [Required]
        public Vehicle Vehicle { get; set; }

        [Required]
        public DateTime DateTimePickUp { get; set; }

        [Required]
        public DateTime DateTimeReturn { get; set; }

        [Required]
        public int optAdd { get; set; }


    }
}
