using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TaxiforSure.Model
{ 
    [DataContract]

    public class EmailConfirmation
    {
        [DataMember]
        public String Message { get; set; }
    }


    [DataContract]

    public class FavouriteConfirmation
    {
        [DataMember]
        public String Message { get; set; }

        [DataMember]
        public bool  Status { get; set; }
    }
}
