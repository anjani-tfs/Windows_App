using Microsoft.Phone.Controls;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Tasks;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using TaxiforSure.Model;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace TaxiforSure
{
    public partial class PanoramaPage1 : PhoneApplicationPage
    {
        // Constructor
        public PanoramaPage1()
        {
            InitializeComponent();
            //onlyOnce = true;
            //onlyTwice = true;
            FromButton1.TextWrapping = TextWrapping.Wrap;
            ToButton1.TextWrapping = TextWrapping.Wrap;
            FromButton2.TextWrapping = TextWrapping.Wrap;
            ToButton2.TextWrapping = TextWrapping.Wrap;
            FromButton2.FontSize = 20;
            ToButton2.FontSize = 20;
            BookDatePicker.Value = RoundUp(DateTime.Now.Add(TimeSpan.FromHours(1)), TimeSpan.FromMinutes(15));
            BookTimePicker.Value = RoundUp(DateTime.Now.Add(TimeSpan.FromHours(1)), TimeSpan.FromMinutes(15));
            BookDatePicker.ValueChanged += BookDatePicker_ValueChanged;
            BookTimePicker.ValueChanged += BookTimePicker_ValueChanged;
            // Set the data context of the listbox control to the sample data

            if (((App)App.Current).GPSOnlyOnce == true)
            {
                ((App)App.Current).GPSOnlyOnce = false;
                getGPSActivatedForPrefil();
            }
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            // if (Storage.IsFirstLogin)
            {

            }
        }

        async private void getGPSActivatedForPrefil()
        {
            if (Storage.AskForGps())
            {
                try
                {

                    var c = new MyClient();
                    await checkLocation(c, await c.ReverseGeoCode(await (App.Current as App).GetLocation()));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }


        string _city1, _city2;

        private async Task checkLocation(MyClient c, LocationInfo l)
        {
            try
            {
                String city = await c.IsinCity(l.Location.Latitude, l.Location.Longitude);

                if (string.IsNullOrEmpty(city))
                {

                    MessageBox.Show("Thanks for checking us out! We are yet to launch our Services in your chosen location.", "TaxiForSure", MessageBoxButton.OK);
                }
                else
                {
                    App myApp = (App)App.Current;
                    myApp.pickLatitudeParam = l.Location.Latitude.ToString();
                    myApp.pickLongitudeParam = l.Location.Longitude.ToString();

                    Query.Pickup = l;
                    Query.Pickup.City = city;
                    Storage.LastLocation = l;



                    MainPanorama.Title = Storage.City;

                    if (Query.Pickup != null)
                    {
                        FromButton1.Text = Query.Pickup.Name;
                        FromButton2.Text = Query.Pickup.Name;
                        bool isCityB, isCityC, isCityD;
                        isCityB = FromButton2.Text.Contains("Bangalore");
                        isCityC = FromButton2.Text.Contains("Chennai");
                        isCityD = FromButton2.Text.Contains("Delhi");
                        if (isCityB && MainPanorama.Title.ToString() != "Bangalore")
                        {
                            MessageBoxResult result = MessageBox.Show("Selected city is different. Choose ok to change the default selected city or please change your choose city in dashboard screen!", "TaxiForSure", MessageBoxButton.OKCancel);
                            if (result == MessageBoxResult.OK)
                            {
                                MainPanorama.Title = "Bangalore";
                                Storage.City = "Bangalore";
                            }
                            else
                            {

                                this.NavigationService.Navigate(new Uri("/LocationSelect.xaml", UriKind.Relative));
                            }
                        }
                        else if (isCityC && MainPanorama.Title.ToString() != "Chennai")
                        {
                            MessageBoxResult result = MessageBox.Show("Selected city is different. Choose ok to change the default selected city or please change your choose city in dashboard screen!", "TaxiForSure", MessageBoxButton.OKCancel);
                            if (result == MessageBoxResult.OK)
                            {
                                MainPanorama.Title = "Chennai";
                                Storage.City = "Chennai";
                            }
                            else
                            {

                                this.NavigationService.Navigate(new Uri("/LocationSelect.xaml", UriKind.Relative));
                            }
                        }
                        else if (isCityD && MainPanorama.Title.ToString() != "Delhi")
                        {
                            MessageBoxResult result = MessageBox.Show("Selected city is different. Choose ok to change the default selected city or please change your choose city in dashboard screen!", "TaxiForSure", MessageBoxButton.OKCancel);
                            if (result == MessageBoxResult.OK)
                            {
                                MainPanorama.Title = "Delhi";
                                Storage.City = "Delhi";
                            }
                            else
                            {

                                this.NavigationService.Navigate(new Uri("/LocationSelect.xaml", UriKind.Relative));
                            }
                        }
                    }
                    else
                    {
                        FromButton1.Text = "Choose Pickup Location";
                        FromButton2.Text = "Choose Pickup Location";

                    }


                    //MessageBox.Show("executed");
                }




            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }

        }

        //bool onlyOnce;
        //bool onlyTwice;

        void BookTimePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {

            //DateTime changedTime = (DateTime)e.NewDateTime;
            //App myApp = (App)App.Current;

            //if (((changedTime.Hour - DateTime.Now.Hour < 1) || ((changedTime.Hour - DateTime.Now.Hour == 1) && (changedTime.Minute - DateTime.Now.Minute <= 0))) && (changedTime.Date == DateTime.Now.Date))
            //{
            //    if (onlyOnce)
            //    {
            //        MessageBox.Show("Sorry, please try Pick Up Now.");

            //        onlyOnce = false;

            //        myApp.dateChecker = false;
            //    }


            //}
            //else
            //{


            //}
        }

        void BookDatePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            //DateTime changedTime = (DateTime)e.NewDateTime;
            //if (changedTime < DateTime.Now)
            //{
            //    if (onlyTwice)
            //    {
            //        MessageBox.Show("You cannot make a booking in past time.");
            //        DatePicker picker = (DatePicker)sender;
            //        picker.Value = DateTime.Now;
            //        onlyTwice = false;
            //    }
            //}

        }



        DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }
        private async Task GetGps()
        {
            if (Storage.AskForGps())
            {
                var cd = await (App.Current as App).GetLocation();
                //var client = new MyClient();
                //String city = await client.IsinCity(cd.Latitude, cd.Longitude);
                //if (!String.IsNullOrWhiteSpace(city))
                //{
                //    Storage.City = city;
                //    MainPanorama.Title = city;
                //}
            }

        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            

            MainPanorama.Title = Storage.City;

            while (NavigationService.CanGoBack == true)
            {
                NavigationService.RemoveBackEntry();
            }
            if (Query.Drop != null)
            {
                ToButton1.Text = Query.Drop.Name;
                ToButton2.Text = Query.Drop.Name;
            }
            else
            {
                ToButton1.Text = "Choose Drop Location";
                ToButton2.Text = "Choose Drop Location";
            }
            if (Query.Pickup != null)
            {
                FromButton1.Text = Query.Pickup.Name;
                FromButton2.Text = Query.Pickup.Name;
            }
            else
            {
                FromButton1.Text = "Choose Pickup Location";
                FromButton2.Text = "Choose Pickup Location";
            }
            if (!DeviceNetworkInformation.IsNetworkAvailable)
            {
                MessageBox.Show("Please check your internet connection.", "TaxiForSure", MessageBoxButton.OK);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App myApp = (App)App.Current;
            myApp.isPickup = true;
            if (!DeviceNetworkInformation.IsNetworkAvailable)
            {
                MessageBox.Show("Please check your internet connection.", "TaxiForSure", MessageBoxButton.OK);
            }
            else
            {
                NavigationService.Navigate(new Uri("/LocationSelect.xaml?isTargetDestination=false", UriKind.RelativeOrAbsolute));

            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            App myApp = (App)App.Current;
            myApp.isPickup = false;
            if (!DeviceNetworkInformation.IsNetworkAvailable)
            {
                MessageBox.Show("The Application needs internet to operate.", "TaxiForSure", MessageBoxButton.OK);
            }
            else
            {
                NavigationService.Navigate(new Uri("/LocationSelect.xaml?isTargetDestination=true", UriKind.RelativeOrAbsolute));
            }
        }

        private void ProceedBtn_OnClick(object sender, EventArgs e)
        {
            //validate the data

            try
            {
                if (!DeviceNetworkInformation.IsNetworkAvailable)
                {
                    MessageBox.Show("The Application needs internet to operate.", "TaxiForSure", MessageBoxButton.OK);
                    return;
                }
                if (Query.Pickup == null) MessageBox.Show("Select Pickup Location.", "TaxiForSure", MessageBoxButton.OK);
                else if (Query.Drop == null) MessageBox.Show("Select Drop Location.", "TaxiForSure", MessageBoxButton.OK);
                else
                {
                    Query.PickNow = MainPanorama.SelectedIndex == 0 ? true : false;
                    if (!Query.Pickup.City.Equals(Query.Drop.City))
                    {
                        MessageBox.Show("Source and destination are in diferent cities", "TaxiForSure", MessageBoxButton.OK);
                        return;
                    }

                    if (Query.Pickup.Name.Equals(Query.Drop.Name))
                    {
                        MessageBox.Show("Pickup and Drop Locations are the same.", "TaxiForSure", MessageBoxButton.OK);
                        return;
                    }

                    MyClient tempClient = new MyClient();
                   bool isNear = tempClient.IsInRadius(Query.Pickup.Location.Latitude, Query.Pickup.Location.Longitude, Query.Drop.Location.Latitude, Query.Drop.Location.Longitude, 1000);
                    if(isNear == true)
                    {
                        MessageBox.Show("Cabs cannot be booked for distance less than 1 km.", "TaxiForSure", MessageBoxButton.OK);
                        return;
                    }

                    DateTime datepart = BookDatePicker.Value.Value;
                    DateTime timePart = BookTimePicker.Value.Value;

                    ((App)App.Current).bookingDateSelected = datepart.Day + "/" + datepart.Month + "/" + datepart.Year;

                    Query.PickupTime = new DateTime(datepart.Year, datepart.Month, datepart.Day,
                        timePart.Hour, timePart.Minute, timePart.Second);
                    //Query.PickupTime = new DateTime(timePart.Hour, timePart.Minute, timePart.Second);
                    Query.PickupTime = RoundUp(Query.PickupTime, TimeSpan.FromMinutes(15));
                    App myApp = (App)App.Current;
                    myApp.pickupParam = Query.PickupTime.ToString();
                    myApp.pickUpParamForPickLater = timePart.Hour.ToString() + ":" + timePart.Minute.ToString();
                    if ((!Query.PickNow && Query.PickupTime < DateTime.Now.Add(TimeSpan.FromHours(1.25))) && (Query.PickNow && Query.PickupTime > DateTime.Now))
                    {
                        MessageBox.Show("Please choose Pick Now.", "TaxiForSure", MessageBoxButton.OK);
                        return;
                    }
                    else if (!Query.PickNow && Query.PickupTime <= DateTime.Now)
                    {
                        MessageBox.Show("Bookings cannot be made in past.", "TaxiForSure", MessageBoxButton.OK);
                        return;
                    }
                    else if (!Query.PickNow && RoundUp(Query.PickupTime,TimeSpan.FromMinutes(15)) < RoundUp(DateTime.Now.Add(TimeSpan.FromHours(1)), TimeSpan.FromMinutes(15)))
                    {
                        MessageBox.Show("Please choose Pick Now.", "TaxiForSure", MessageBoxButton.OK);
                        return;
                    }

                    if (Query.PickupTime > DateTime.Now.Add(TimeSpan.FromDays(60)))
                    {
                        MessageBox.Show("Date cannot be set after 2 months from now.","TaxiForSure",MessageBoxButton.OK);
                        return;

                    }

                    NavigationService.Navigate(new Uri("/SelectCab.xaml", UriKind.RelativeOrAbsolute));
                }

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }



        private void ApplicationBarMenuItem_OnClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PricePage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ApplicationBarMenuItem_OnClick_2(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/TrackMyTaxi.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ApplicationBarMenuItem_OnClick_3(object sender, EventArgs e)
        {
            var review = new MarketplaceReviewTask();
            review.Show();
        }

        private void GiveFeedback(object sender, EventArgs e)
        {
            var et = new EmailComposeTask();
            et.To = "app-feedback@taxiforsure.com";
            et.Subject = "Windows Phone App Feedback";
            et.Body = "\n\n\n WP App-2.8.3";
            et.Show();
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ChangeCity(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SelectCity.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ApplicationBarMenuItem_OnClick4(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SelectCity.xaml", UriKind.RelativeOrAbsolute));
        }

        private void MainPanorama_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (MainPanorama.SelectedIndex == 0)
            {
                PickName.Text = ("NOW");
                PickTime.Text = ("before");
                App myApp = (App)App.Current;
                myApp.isPickNow = true;
            }
            else
            {
                PickName.Text = ("LATER");
                PickTime.Text = ("after");
                App myApp = (App)App.Current;
                myApp.isPickNow = false;

            }
            BookTimePicker.Value = RoundUp(DateTime.Now.Add(TimeSpan.FromHours(1)), TimeSpan.FromMinutes(15));
            PickTime2.Text = BookTimePicker.Value.Value.ToShortTimeString();
        }
        //protected override void OnNavigatedFrom(NavigationEventArgs e)
        //{
        //    App myApp = (App)App.Current;
        //    if (myApp.isPickNow == true)
        //        myApp.bookLater_pickupDate = BookDatePicker.Value.Value.Day.ToString() + "/" + BookDatePicker.Value.Value.Month.ToString() + "/" + BookDatePicker.Value.Value.Year.ToString();
        //    base.OnNavigatedFrom(e);
        //}

        private void TextBlock_Tap_1(object sender, GestureEventArgs e)
        {
            App myApp = (App)App.Current;
            myApp.isPickup = true;
            if (!DeviceNetworkInformation.IsNetworkAvailable)
            {
                MessageBox.Show("Please check your internet connection.", "TaxiForSure", MessageBoxButton.OK);
            }
            else
            {
                NavigationService.Navigate(new Uri("/LocationSelect.xaml?isTargetDestination=false", UriKind.RelativeOrAbsolute));

            }
        }

        private void TextBlock_Tap_2(object sender, GestureEventArgs e)
        {
            App myApp = (App)App.Current;
            myApp.isPickup = false;
            if (!DeviceNetworkInformation.IsNetworkAvailable)
            {
                MessageBox.Show("Please check your internet connection.", "TaxiForSure", MessageBoxButton.OK);
            }
            else
            {
                NavigationService.Navigate(new Uri("/LocationSelect.xaml?isTargetDestination=true", UriKind.RelativeOrAbsolute));
            }
        }

        private void FromButton1_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var button = (TextBlock)sender;
            button.Foreground = new SolidColorBrush(Color.FromArgb(255, 248, 218, 21));
        }

        private void FromButton1_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var button = (TextBlock)sender;
            button.Foreground = new SolidColorBrush(Colors.Black);
            
        }
    }
}