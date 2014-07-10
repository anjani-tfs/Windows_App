using System;
using System.Runtime.Serialization;

namespace TaxiforSure.Model
{
    [DataContract]
    public class BookingData
    {
        [DataMember]
        public String BookingId { get; set; }

        [DataMember]
        public String From { get; set; }

        [DataMember]
        public String To { get; set; }

        [DataMember]
        public String CarName { get; set; }

        [DataMember]
        public String Time { get; set; }

        [DataMember]
        public String Fare
        {
            get;
            set;
        }

        [DataMember]
        public String BookingStatus
        {
            get;
            set;
        }
    }
}
