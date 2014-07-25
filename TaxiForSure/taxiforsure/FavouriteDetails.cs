using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TaxiforSure
{
    [DataContract]

    public class FavouriteDetails
    {
        [DataMember]
        public String landmark { get; set; }

        [DataMember]
        public String locality_name { get; set; }

        [DataMember]
        public String type { get; set; }

        [DataMember]
        public String city { get; set; }

        [DataMember]
        public String address { get; set; }

        [DataMember]
        public String latitude { get; set; }

        [DataMember]
        public String longitude { get; set; }

        [DataMember]
        public String title { get; set; }
    }
}
