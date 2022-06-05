using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Omni_Ride_Network {
    public class RatingViewModel {

        public List<Rating> Ratings { get; set; }

        public RatingViewModel() {
            Ratings = new List<Rating>();
        }
    }
}
