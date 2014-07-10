using System;
using System.Device.Location;

namespace TaxiforSure.Model
{
    public class TaxiStatus
    {/*
      * "status": "Assigned",
    "booking_fare": 200.00,
    "pickup_area_longitude": 77.6264100,
    "latitude": 12.8899750,
    "status_message": "Thanks for booking with us! Please pay as per meter reading!",
    "pickup_area_latitude": 12.9277600,
    "booking_status": 6,
    "longitude": 77.6207450
      * */
        public String Status { get; set; }
        public String Fare { get; set; }
        public GeoCoordinate Location { get; set; }
        public GeoCoordinate PickupLocation { get; set; }
        public String Message { get; set; }
        public int StatusId { get; set; }
    }
}
