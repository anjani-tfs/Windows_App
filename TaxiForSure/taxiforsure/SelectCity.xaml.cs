using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Windows.Navigation;
using TaxiforSure.Model;
using System.Collections.Generic;



namespace TaxiforSure
{
    public partial class SelectCity : PhoneApplicationPage
    {

        private List<ListBoxItem> PopulateCityTable()
        {


            List<string> cityStringList = new List<string>();
            cityStringList = Storage.cityList;
           /*
            cityStringList.Add("a");
            cityStringList.Add("b");
            cityStringList.Add("c");
            cityStringList.Add("d");
            cityStringList.Add("e");
            cityStringList.Add("f");
            cityStringList.Add("g");
            cityStringList.Add("h");
            cityStringList.Add("i");
            cityStringList.Add("j");
            cityStringList.Add("k");
            * */
            List<ListBoxItem> MyList = new List<ListBoxItem>();

            foreach (string value in cityStringList)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = value;
                item.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                item.Margin = new System.Windows.Thickness(0, 20, 0, 0);
                MyList.Add(item);
            }

            return MyList;
            //CityListBox.Items = MyList;
            

        }

        public void adjustHeightOfCityListBox(){

            int maxRows = 8 < Storage.cityList.Count ? 8 : Storage.cityList.Count;
            //maxRows = 8;
            int heightOfMaxRows = maxRows * 68;
            CityListBox.Height = heightOfMaxRows;

        }

        public SelectCity()
        {
            InitializeComponent();
            //adjustHeightOfCityListBox();
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