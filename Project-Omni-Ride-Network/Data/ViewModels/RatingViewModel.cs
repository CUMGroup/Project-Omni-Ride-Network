using System.Linq;


namespace Project_Omni_Ride_Network {
    public class RatingViewModel : BaseViewModel {
        // Rating Distribution for the rating bar diagram
        // [0] -> 1 Star, [1] -> 2 Star ...
        public int[] RatingDistribution { get; set; }
        // Rating counts for the rating bar diagram
        // [0] -> 1 Star, ...
        public int[] RatingCounts { get; set; }

        public int TotalRatings { get => RatingCounts.Sum(); }

        // Is the user able to give a rating or has he already reviewed? (only one review per user possible)
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
