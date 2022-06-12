using System.Collections.Generic;

namespace Project_Omni_Ride_Network {
    public class OverviewViewModel {

        public List<Vehicle> Vehicles { get; set; }

        public List<string> BrandFilterList { get; set; }
        public List<string> ModelFilterList { get; set; }


        public OverviewViewModel() {
            Vehicles = new List<Vehicle>();
            BrandFilterList = new List<string>();
            ModelFilterList = new List<string>();
        }

    }
}
