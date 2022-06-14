namespace Project_Omni_Ride_Network {
    public class ProfileViewModel : BaseViewModel {

        public Customer Customer { get; set; }

        public ProfileViewModel(Customer c) {
            Customer = c;
        }

        public ProfileViewModel(BaseViewModel b, Customer c) {
            Authorized = b.Authorized;
            UserName = b.UserName;
            Customer = c;
        }

    }
}
