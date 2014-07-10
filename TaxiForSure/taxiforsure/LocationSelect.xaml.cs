using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using TaxiforSure.Model;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace TaxiforSure
{
    public partial class LocationSelect : PhoneApplicationPage
    {
        String previousValue = "";
        bool isTargetDestination;
        ProgressIndicator prog = new ProgressIndicator();
        private MyClient c = new MyClient();

        public bool IsBackPressed = false;

        LocationInfo locationInfo = null;

        public LocationSelect()
        {
            InitializeComponent();
            inputBox.TextChanged += inputBox_TextChanged;
            inputBox.TextInputUpdate += inputBox_TextInputUpdate;
            App myApp = (App)App.Current;
            
            //   dt.Start();
        }
        

       

        void inputBox_TextInputUpdate(object sender, TextCompositionEventArgs e)
        {

        }

        async void inputBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                if (String.IsNullOrWhiteSpace(inputBox.Text) || inputBox.Text == "")
                {
                    resultBox.ItemsSource = null;
                    resultBox.Items.Clear();
                    previousValue = inputBox.Text;
                    OptionsPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    OptionsPanel.Visibility = Visibility.Collapsed;
                }

                if (!inputBox.Text.Equals(previousValue) && LoadinGrid.Visibility == Visibility.Collapsed)
                {
                    // cancel previous requests
                    c.CancelPending();
                    previousValue = inputBox.Text;
                    prog.IsIndeterminate = true;
                    prog.IsVisible = true;
                    SystemTray.SetProgressIndicator(this, prog);
                    List<AutoCompleteResult> results = await c.GetAutoComplete(previousValue, locationInfo.Location);// await ((App.Current as App).GetLocation()));
                    prog.IsVisible = false;
                    if (results != null)
                    {
                        //                results.Insert(0, new AutoCompleteResult { Description = "Current Location", Reference = "" });

                        resultBox.ItemsSource = results;
                    }

                    if (inputBox.Text == "")
                    {
                        resultBox.ItemsSource = null;
                        resultBox.Items.Clear();
                        previousValue = "";
                    }
                }
            }
            catch
            {
            }
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            IsBackPressed = false;
            locationInfo = new LocationInfo();

            try
            {
                if (Storage.City == "Delhi")
                {
                    locationInfo.City = "Delhi";
                    try
                    {
                        locationInfo.Location = new GeoCoordinate();
                        locationInfo.Location.Latitude = ((App)App.Current).Delhi3T.Lat;
                        locationInfo.Location.Longitude = ((App)App.Current).Delhi3T.Lng;
                    }
                    catch
                    {
                        locationInfo.Location = new GeoCoordinate();
                        locationInfo.Location.Latitude = 28.55754;
                        locationInfo.Location.Longitude = 77.08814;

                    }
                }
                else if (Storage.City == "Chennai")
                {
                    locationInfo.City = "Chennai";
                    try
                    {
                        locationInfo.Location = new GeoCoordinate();
                        locationInfo.Location.Latitude = ((App)App.Current).ChennaiPort.Lat;
                        locationInfo.Location.Longitude = ((App)App.Current).ChennaiPort.Lng;
                    }
                    catch
                    {
                        locationInfo.Location = new GeoCoordinate(12.9844860, 80.1626730);

                    }

                }
                else if (Storage.City == "Bangalore")
                {
                    locationInfo.City = "Bangalore";
                    try
                    {
                        locationInfo.Location = new GeoCoordinate();
                        locationInfo.Location.Latitude = ((App)App.Current).BanglorePort.Lat;
                        locationInfo.Location.Longitude = ((App)App.Current).BanglorePort.Lng;
                    }
                    catch
                    {
                        locationInfo.Location = new GeoCoordinate(13.20318, 77.70445);
                    }
                }
                else if (Storage.City == "Hyderabad")
                {
                    locationInfo.City = "Hyderabad";
                    try
                    {
                        locationInfo.Location = new GeoCoordinate();
                        locationInfo.Location.Latitude = ((App)App.Current).HyderabadPort.Lat;
                        locationInfo.Location.Longitude = ((App)App.Current).HyderabadPort.Lng;
                    }
                    catch
                    {
                        locationInfo.Location = new GeoCoordinate(17.240785, 78.42939);
                    }
                }
            }
            catch
            {
            }


            try
            {
                this.isTargetDestination = (bool)bool.Parse(NavigationContext.QueryString["isTargetDestination"]);
                if (Storage.City == "Delhi")
                {
                    AirportTextBlock.Text = "Airport Terminal 3";
                    DelhiAirBlock.Visibility = Visibility.Visible;
                }
                else if (Storage.City == "Chennai")
                {
                    AirportTextBlock.Text = "Chennai Airport";

                }
                else if (Storage.City == "Bangalore")
                {
                    AirportTextBlock.Text = "Bangalore Airport";

                }
                else if (Storage.City == "Hyderabad")
                {
                    AirportTextBlock.Text = "Rajiv Gandhi International Airport";
                }
            }
            catch
            { }
        }

        private async void resultBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (resultBox.SelectedIndex != -1)
                {

                    LoadinGrid.Visibility = Visibility.Visible;
                    var select = (AutoCompleteResult)resultBox.SelectedItem;
                    inputBox.Text = select.Description;
                    var location = await c.GeoCode(select);
                    await checkLocation(location);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                LoadinGrid.Visibility = Visibility.Collapsed;
            }
        }

        //private void checkCurrentCity()
        //{
        //     isCityB = ToButton2.Text.ToString().Contains("Bangalore");
        //        isCityC = ToButton2.Text.ToString().Contains("Chennai");
        //        isCityD = ToButton2.Text.ToString().Contains("Delhi");
        //        if (isCityB && MainPanorama.Title.ToString() != "Bangalore")
        //        {
        //            MessageBoxResult result = MessageBox.Show("Your location is not in the selected city. Choose ok to change the default selected city.", "TaxiForSure", MessageBoxButton.OKCancel);
        //            if (result == MessageBoxResult.OK)
        //            {
        //                MainPanorama.Title = "Bangalore";
        //                Storage.City = "Bangalore";
        //            }
        //            else
        //            {

        //                this.NavigationService.Navigate(new Uri("/LocationSelect.xaml", UriKind.Relative));
        //            }
        //        }
        //        else if (isCityC && MainPanorama.Title.ToString() != "Chennai")
        //        {
        //            MessageBoxResult result = MessageBox.Show("Your location is not in the selected city. Choose ok to change the default selected city.", "TaxiForSure", MessageBoxButton.OKCancel);
        //            if (result == MessageBoxResult.OK)
        //            {
        //                MainPanorama.Title = "Chennai";
        //                Storage.City = "Chennai";
        //            }
        //            else
        //            {

        //                this.NavigationService.Navigate(new Uri("/LocationSelect.xaml", UriKind.Relative));
        //            }
        //        }
        //        else if (isCityD && MainPanorama.Title.ToString() != "Delhi")
        //        {
        //            MessageBoxResult result = MessageBox.Show("Your location is not in the selected city. Choose ok to change the default selected city.", "TaxiForSure", MessageBoxButton.OKCancel);
        //            if (result == MessageBoxResult.OK)
        //            {
        //                MainPanorama.Title = "Delhi";
        //                Storage.City = "Delhi";
        //            }
        //            else
        //            {

        //                this.NavigationService.Navigate(new Uri("/LocationSelect.xaml", UriKind.Relative));
        //            }
        //        }
        //}
        private async Task checkLocation(LocationInfo l)
        {
            try
            {
                String city = await c.IsinCity(l.Location.Latitude, l.Location.Longitude);
                if (string.IsNullOrEmpty(city))
                {
                    inputBox.Text = "";
                    MessageBox.Show("Thanks for checking us out! We are yet to launch our Services in your chosen location.", "TaxiForSure", MessageBoxButton.OK);
                    LoadinGrid.Visibility = Visibility.Collapsed;
                    return;
                }
                if (city.ToLower() != Storage.City.ToLower())
                {
                    MessageBoxResult result = MessageBox.Show("Selected city is different. Please change your choose city in dashboard screen!", "TaxiForSure", MessageBoxButton.OK);
                    if (result == MessageBoxResult.OK)
                    {
                        //Storage.City = city;

                        inputBox.Text = "";
                        LoadinGrid.Visibility = Visibility.Collapsed;
                        return;
                    }
                    //else
                    //{
                    //    inputBox.Text = "";
                    //    LoadinGrid.Visibility = Visibility.Collapsed;
                    //    return;
                    //}
                }
                if (IsBackPressed == false)
                {
                    if (isTargetDestination)
                    {
                        App myApp = (App)App.Current;
                        myApp.dropLatParam = l.Location.Latitude.ToString();
                        myApp.dropLongParam = l.Location.Longitude.ToString();
                        Query.Drop = l;
                        Query.Drop.City = city;
                        myApp.IsTargetAirport = await c.IsInAirportVicinity(l.Location.Latitude, l.Location.Longitude, city);
                    }

                    else
                    {
                        App myApp = (App)App.Current;
                        myApp.pickLatitudeParam = l.Location.Latitude.ToString();
                        myApp.pickLongitudeParam = l.Location.Longitude.ToString();

                        Query.Pickup = l;
                        Query.Pickup.City = city;
                        Storage.LastLocation = l;
                        myApp.IsSourceAirport = await c.IsInAirportVicinity(l.Location.Latitude, l.Location.Longitude, city);
                    }

                    NavigationService.GoBack();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
            LoadinGrid.Visibility = Visibility.Collapsed;
        }

        private async void CurrentLocationSelect(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (Storage.AskForGps())
            {
                try
                {
                    LoadinGrid.Visibility = Visibility.Visible;
                    await checkLocation(await c.ReverseGeoCode(await (App.Current as App).GetLocation()));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                LoadinGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void InputBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
            }
        }

        private void AirportTextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            #region airport co-ords
            #endregion
            LoadinGrid.Visibility = System.Windows.Visibility.Visible;
            
            var l = new LocationInfo();
            

                if (Storage.City == "Delhi")
                {
                    l.Name = l.Area = l.Landmark = "Airport Terminal 3";
                    l.City = "Delhi";
                    try
                    {
                        l.Location = new GeoCoordinate();
                        l.Location.Latitude = ((App)App.Current).Delhi3T.Lat;
                        l.Location.Longitude = ((App)App.Current).Delhi3T.Lng;
                    }
                    catch
                    {
                        l.Location = new GeoCoordinate();
                        l.Location.Latitude = 28.55754;
                        l.Location.Longitude = 77.08814;
                        
                    }
                }
                else if (Storage.City == "Chennai")
                {
                    l.Name = l.Area = l.Landmark = "Chennai Airport";
                    l.City = "Chennai";
                    try
                    {
                        l.Location = new GeoCoordinate();   
                        l.Location.Latitude = ((App)App.Current).ChennaiPort.Lat;
                        l.Location.Longitude = ((App)App.Current).ChennaiPort.Lng;
                    }
                    catch
                    {
                        l.Location = new GeoCoordinate(12.9844860, 80.1626730);
                        
                    }

                }
                else if (Storage.City == "Bangalore")
                {
                    l.Name = l.Area = l.Landmark = "Bangalore Airport";
                    l.City = "Bangalore";
                    try
                    {
                        l.Location = new GeoCoordinate();
                        l.Location.Latitude = ((App)App.Current).BanglorePort.Lat;
                        l.Location.Longitude = ((App)App.Current).BanglorePort.Lng;
                    }
                    catch
                    {
                        l.Location = new GeoCoordinate(13.20318, 77.70445);
                        
                        
                    }

                }
                else if(Storage.City == "Hyderabad")
                {

                    l.Name = l.Area = l.Landmark = "Rajiv Gandhi International Airport";
                    l.City = "Hyderabad";
                    try
                    {
                        l.Location = new GeoCoordinate();
                        l.Location.Latitude = ((App)App.Current).HyderabadPort.Lat;
                        l.Location.Longitude = ((App)App.Current).HyderabadPort.Lng;
                    }
                    catch
                    {
                        l.Location = new GeoCoordinate(17.240785,78.429398);

                    }


                }

                if (isTargetDestination)
                {
                    App myApp = (App)App.Current;
                    myApp.dropLatParam = l.Location.Latitude.ToString();
                    myApp.dropLongParam = l.Location.Longitude.ToString();
                    Query.Drop = l;
                    myApp.IsTargetAirport = true;
                }

                else
                {
                    App myApp = (App)App.Current;
                    myApp.pickLatitudeParam = l.Location.Latitude.ToString();
                    myApp.pickLongitudeParam = l.Location.Longitude.ToString();

                    Query.Pickup = l;
                    Storage.LastLocation = l;
                    myApp.IsSourceAirport = true;
                }
                LoadinGrid.Visibility = System.Windows.Visibility.Collapsed;
                NavigationService.GoBack();
            
            
        }

        private void DelhiAirBlock_OnTap(object sender, GestureEventArgs e)
        {
            var l = new LocationInfo();
            l.Name = l.Area = l.Landmark = "Airport Terminal 1";
            l.City = "Delhi";
            try
            {
                l.Location = new GeoCoordinate();
                l.Location.Latitude = ((App)App.Current).Delhi1D.Lat;
                l.Location.Longitude = ((App)App.Current).Delhi1D.Lng;
            }
            catch
            {
                
                l.Location = new GeoCoordinate(28.56168, 77.119);
            }
            
            
            if (isTargetDestination)
            {
                App myApp = (App)App.Current;
                myApp.dropLatParam = l.Location.Latitude.ToString();
                myApp.dropLongParam = l.Location.Longitude.ToString();
                Query.Drop = l;
                myApp.IsTargetAirport = true;
            }

            else
            {
                App myApp = (App)App.Current;
                myApp.pickLatitudeParam = l.Location.Latitude.ToString();
                myApp.pickLongitudeParam = l.Location.Longitude.ToString();

                Query.Pickup = l;
                Storage.LastLocation = l;
                myApp.IsSourceAirport = true;
            }

            NavigationService.GoBack();
        }

        private void UIElement_OnTap(object sender, GestureEventArgs e)
        {
            var t = (TextBlock)sender;
            t.FontSize += 0.20 * t.FontSize;

          // Prosenjit - Modified on 27-03-2014
            t.Foreground =new SolidColorBrush(Color.FromArgb(255, 248, 218, 21));
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            var button = (TextBlock)sender;
            button.Foreground = new SolidColorBrush(Color.FromArgb(255, 248, 218, 21));
        }

        private void TextBlock_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            var button = (TextBlock)sender;
            if (button.Text.Contains("Current Location") || button.Text.Contains("Airport"))
            {
                button.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TextBlock_MouseLeave_1(object sender, MouseEventArgs e)
        {
            var button = (TextBlock)sender;
            if (button.Text.Contains("Current Location") || button.Text.Contains("Airport"))
            {
                button.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void PhoneApplicationPage_BackKeyPress_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (LoadinGrid.Visibility == Visibility.Visible)
            {
                IsBackPressed = true;
            }
        }
    }
}