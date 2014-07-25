using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TaxiforSure.Model
{
    [DataContract]
    public class LoginDetails
    {
        [DataMember]
        public String UserId { get; set; }

        [DataMember]
        public String Email { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public String Number { get; set; }

        [DataMember]
        public String Referral_Code { get; set; }

        [DataMember]
        public String Referral_Url { get; set; }

        [DataMember]
        public String Username { get; set; }

        [DataMember]
        public String Password { get; set; }
    }
}
