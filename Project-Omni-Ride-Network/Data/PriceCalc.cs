using System;

namespace Project_Omni_Ride_Network {
    public static class PriceCalc {

        public static float CalculateTotalprice(Order order) {

            float timeprice = CalculateTimePrice(order);
            float optionals = CalculateOptionalPrice(order);

            var totalprice = (float) Math.Round(timeprice + optionals + order.Vehicle.BasicPrice, 2);

            return totalprice;
        }

        private static float CalculateTimePrice(Order order) {

            var duration = order.DateTimeReturn - order.DateTimePickUp;
            
            if (order.Vehicle.Category == 1) {
                var counter = duration.TotalDays + 1;
                var timeprice = counter * order.Vehicle.PriceHD;
                var tp = (float) timeprice;
                return tp;
            }
            if (order.Vehicle.Category == 2) {
                var counter = Math.Ceiling(duration.TotalHours);
                var timeprice = counter * order.Vehicle.PriceHD;
                var tp = (float)timeprice;
                return tp;
            }

            return 0.0f;
        }

        private static float CalculateOptionalPrice(Order order) =>
            order.optAdd switch {
                0 => 0.0f,
                1 => order.Vehicle.PriceInsu,
                2 => 50.0f,
                3 => 35.0f,
                4 => order.Vehicle.PriceInsu + 35.0f,
                5 => 85.0f,
                _ => 0.0f,
            };
        



    }
}
