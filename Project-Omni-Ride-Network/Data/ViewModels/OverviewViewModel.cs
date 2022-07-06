using System.Collections.Generic;

namespace Project_Omni_Ride_Network {
    public class OverviewViewModel : BaseViewModel {

        public List<Vehicle> Vehicles { get; set; }

        // Filter lists to populate the filter combo boxes with the right car names
        public List<string> BrandFilterList { get; set; }
        public List<string> ModelFilterList { get; set; }


        public OverviewViewModel() {
            init();
        }

        public OverviewViewModel(BaseViewModel b) {
            Authorized = b.Authorized;
            UserName = b.UserName;

            init();
        }

        private void init() {
            Vehicles = new List<Vehicle>();
            BrandFilterList = new List<string>();
            ModelFilterList = new List<string>();
        }
    }
}
