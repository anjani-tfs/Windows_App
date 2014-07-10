using System;

namespace TaxiforSure.Model
{
    public static class Query
    {
        public static String CouponCode { get; set; }
        public static LocationInfo Pickup { get; set; }
        public static LocationInfo Drop { get; set; }
        public static bool PickNow { get; set; }
        public static DateTime PickupTime { get; set; }
        public static CarDetails Car { get; set; }
    }
}
