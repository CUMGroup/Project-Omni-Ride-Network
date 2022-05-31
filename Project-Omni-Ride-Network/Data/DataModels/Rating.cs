using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

using System;

namespace Project_Omni_Ride_Network.Data.DataModels {
    public class Rating {

        //[Required]
        //public string UserId { get; set; }
        // Querverweis auf Objekt

        public string Comment { get; set; }

        public int Stars { get; set; }

        public DateTime CmntTime { get; set; }

    }
}
