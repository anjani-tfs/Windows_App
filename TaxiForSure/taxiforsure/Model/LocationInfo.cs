using System;
using System.Device.Location;

namespace TaxiforSure.Model
{
    public class LocationInfo
    {
        public String Name { get; set; }
        public String Area { get; set; }
        public String Landmark { get; set; }
        public GeoCoordinate Location { get; set; }
        public String City { get; set; }
    }
}
