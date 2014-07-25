using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Windows.Navigation;
using TaxiforSure.Model;

namespace TaxiforSure
{
    public partial class SelectCity : PhoneApplicationPage
    {
        public SelectCity()
        {
            InitializeComponent();
        }

        private void CityListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ListBoxItem)CityListBox.SelectedItem;
            if (!Storage.City.Equals(item.Content.ToString()))
            {
                Storage.City = item.Content.ToString();
                Query.Pickup = null;
                Query.Drop = null;
                App myApp = (App)App.Current;
            }
            NavigationService.GoBack();
        }
    }
}