using System;
using System.Device.Location;

namespace TaxiforSure.Model
{
    public class DriverDetails
    {
        public String Number { get; set; }

        public String Name { get; set; }

        public String VehicleNumber { get; set; }

        public String Message { get; set; }

        public String Status { get; set; }

        public GeoCoordinate PickupLocation { get; set; }

    }



    public class BookingInfo
    {
        public String Fare { get; set; }
    }
}
