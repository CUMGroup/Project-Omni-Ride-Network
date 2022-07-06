
namespace Project_Omni_Ride_Network {
    /// <summary>
    /// Base Viewmodel given to every View
    /// </summary>
    public class BaseViewModel {

        // Authorized and Username are important to know, to display the profile dropdown in the Layout correctly
        public bool Authorized { get; set; } = false;
        public string UserName { get; set; }

    }
}
