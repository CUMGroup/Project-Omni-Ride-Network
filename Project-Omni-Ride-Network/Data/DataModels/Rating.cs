using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

using System;

namespace Project_Omni_Ride_Network {
    public class Rating {

        public Customer User { get; set; }
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Comment { get; set; }

        [Required]
        public int Stars { get; set; }

        [Required]
        public DateTime CmntTime { get; set; }

    }
}
