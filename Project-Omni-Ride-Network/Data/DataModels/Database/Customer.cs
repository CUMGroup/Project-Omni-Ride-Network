using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Project_Omni_Ride_Network {
    public class Customer {

        public ApplicationUser User { get; set; }
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }        

        [Required]
        [MaxLength(4)]
        public string KdTitle { get; set; }

        [Required]
        [MaxLength(20)]
        public string KdSurname { get; set; }

        [Required]
        [MaxLength(15)]
        public string KdName { get; set; }

        [Required]
        public DateTime KdBirth { get; set; }

        [Required]
        public int Paymnt { get; set; }

    }
}
