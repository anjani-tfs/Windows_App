using System;

namespace TaxiforSure.Model
{
    public class AutoCompleteResult
    {
        public String Description { get; set; }
        public String Reference { get; set; }
        public override string ToString()
        {
            return Description;
        }
    }
}
