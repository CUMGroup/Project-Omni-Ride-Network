using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

using System;

namespace Project_Omni_Ride_Network.Data.DataModels {
    public class Costumer {

        //[Required]
        //public int CostumerId { get; set; }
        // Querverweis auf Objekt

        public string KdTitle { get; set; }

        public string KdSurname { get; set; }

        public string KdName { get; set; }

        public DateTime KdBirth { get; set; }

        public int Paymnt { get; set; }

    }
}
