using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TaxiforSure.Model
{
    [DataContract]
    public class CurrentPastBookingDetails
    {
        private string booking_status = string.Empty;
        //[DataMember]
        //public String num_past_bookings { get; set; }
        //[DataMember]
        //public String num_pending_bookings { get; set; }
        [DataMember]
        public String BookingId { get; set; }
        [DataMember]
        public String BookingStatus { get; set; }
        [DataMember]
        public String booking_type { get; set; }
        [DataMember]
        public String To { get; set; }
        [DataMember]
        public String drop_area { get; set; }
        [DataMember]
        public String Fare { get; set; }
        [DataMember]
        public String has_feedback { get; set; }
        [DataMember]
        public String is_outstation { get; set; }
        [DataMember]
        public String From { get; set; }
        [DataMember]
        public String pickup_area { get; set; }
        [DataMember]
        public String pickup_city { get; set; }
        [DataMember]
        public String Time { get; set; }
        [DataMember]
        public String pickup_latlong { get; set; }
        [DataMember]
        public String pickup_time { get; set; }
        [DataMember]
        public String service_status { get; set; }   
    }
}
