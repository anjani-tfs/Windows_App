using System;
using System.ComponentModel;

namespace TaxiforSure.Model
{
    public class CarDetails : INotifyPropertyChanged
    {
        public String CarName { get; set; }
        public String CarType { get; set; }
        public String CarModel { get; set; }
        public int FareId { get; set; }
        public int BaseKm { get; set; }
        public double BaseFare { get; set; }
        public double ExtraKmFare { get; set; }
        public bool HasAc { get; set; }
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
