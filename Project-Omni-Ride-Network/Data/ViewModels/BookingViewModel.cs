namespace Project_Omni_Ride_Network {
    public class BookingViewModel : BaseViewModel {

        public Vehicle Vehicle { get; set; }

        public BookingViewModel(Vehicle v) {
            Vehicle = v;
        }

        public BookingViewModel(BaseViewModel b, Vehicle v) {
            Vehicle = v;
            Authorized = b.Authorized;
            UserName = b.UserName;
        }

    }
}
