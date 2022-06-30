using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Omni_Ride_Network {
    public class Adress {

        public Customer User { get; set; }
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public int KdPLZ { get; set; }

        [Required]
        [MaxLength(15)]
        public string KdOrt { get; set; }

        [Required]
        [MaxLength(15)]
        public string KdStr { get; set; }

        [Required]
        [MaxLength(3)]
        public string KdStrNr { get; set; }
    }
}
