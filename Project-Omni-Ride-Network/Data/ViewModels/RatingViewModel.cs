using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Omni_Ride_Network {
    public class RatingViewModel : BaseViewModel {

        public List<Rating> Ratings { get; set; }

        public RatingViewModel() {
            init();
        }

        public RatingViewModel(BaseViewModel b) {
            Authorized = b.Authorized;
            UserName = b.UserName;
            init();
        }

        private void init() {
            Ratings = new List<Rating>();
        }
    }
}
