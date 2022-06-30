using System.Linq;


namespace Project_Omni_Ride_Network {
    public class RatingViewModel : BaseViewModel {
        public int[] RatingDistribution { get; set; }
        public int[] RatingCounts { get; set; }

        public int TotalRatings { get => RatingCounts.Sum(); }

        public bool UserAlreadyReviewed { get; set; }

        public RatingViewModel() {
            init();
        }

        public RatingViewModel(BaseViewModel b) {
            Authorized = b.Authorized;
            UserName = b.UserName;
            init();
        }

        private void init() {
            RatingDistribution = new int[5];
            RatingCounts = new int[5];
        }
    }
}
