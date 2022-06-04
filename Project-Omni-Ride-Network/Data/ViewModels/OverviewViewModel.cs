using System.Collections.Generic;

namespace Project_Omni_Ride_Network {
    public class OverviewViewModel {

        public List<Vehicle> Vehicles { get; set; }

        public OverviewViewModel() {
            Vehicles = new List<Vehicle>();
        }

    }
}
