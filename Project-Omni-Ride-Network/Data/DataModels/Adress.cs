using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Project_Omni_Ride_Network.Data.DataModels {
    public class Adress {

        //[Required]
        //public string UserId { get; set; }
        // Querverweis auf Objekt

        public int KdPLZ { get; set; }

        public string KdOrt { get; set; }

        public string KdStr { get; set; }

        public string KdStrNr { get; set; }
    }
}
