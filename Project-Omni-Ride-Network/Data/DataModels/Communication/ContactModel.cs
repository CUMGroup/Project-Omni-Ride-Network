using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Project_Omni_Ride_Network {

    public class ContactModel {

        [Required, Display(Name = "SenderName")]
        public string SenderName { get; set; }

        [Required, Display(Name = "SenderEmail"), EmailAddress]
        public string SenderEmail { get; set; }

        [Required, Display(Name = "Subject") ]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
    }

}