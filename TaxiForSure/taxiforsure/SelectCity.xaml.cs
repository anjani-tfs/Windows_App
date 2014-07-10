using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Windows.Navigation;
using TaxiforSure.Model;
using System.Collections.Generic;


namespace TaxiforSure
{
    public partial class SelectCity : PhoneApplicationPage
    {

        private List<string> PopulateCityTable()
        {
            List<string> MyList = new List<string>();
            MyList.Add("HELLO");
            MyList.Add("WORLD");
            MyList.Add("hi");
            MyList.Add("yo");

            return MyList;
            //CityListBox.Items = MyList;
            

        }


        public SelectCity()
        {
            InitializeComponent();
            //PopulateCityTable();
            CityListBox.ItemsSource = PopulateCityTable();
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