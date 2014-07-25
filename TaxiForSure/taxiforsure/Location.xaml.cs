using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using TaxiforSure.Model;

namespace TaxiforSure
{
    public partial class Location : PhoneApplicationPage
    {
        List<GeoCoordinate> MyCoordinates = new List<GeoCoordinate>();
        RouteQuery MyQuery = null;
        GeocodeQuery Mygeocodequery = null;
        CurrentPastBookingDetails storesignIndata=null;
        BookingData storedata=null;

        bool isPicked = true;

        public Location()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (Storage.IsLogin)
            {
                try
                {  
                    App myApp = (App)App.Current;
                    int index = int.Parse(NavigationContext.QueryString["index"]);
                    String dataStore = NavigationContext.QueryString["data"];
                    CurrentPastBookingDetails data;
                    if (dataStore == "current")
                    {
                        var defaultBooking = from names in Storage.SignInCurrentBookingIds
                                             orderby names.Time ascending
                                             select names;
                        data = defaultBooking.ElementAt(index);
                    }
                    else
                    {
                        data = Storage.SignInPastBookingIds.ElementAt(index);
                    }

                   // VehicleNumberRun.Text = data.CarName;

                    DriverDetailPanel.Visibility = Visibility.Collapsed;
                    FromBlock.Text = data.From;
                    ToBlock.Text = data.To;

                    var client = new MyClient();
                    var driver = await client.CallDriver(data.BookingId);
                    if (driver != null)
                    {
                        if (driver.Number.Length > 0) DriverDetailPanel.Visibility = Visibility.Visible;
                        PhoneBlock.Text = driver.Number;
                        NameBlock.Text = driver.Name;
                        if (!String.IsNullOrWhiteSpace(driver.VehicleNumber)) VehicleGap.Text = "   ";

                        if (driver.Status == "Assigned")
                        {
                            VehicleNumberRun.Text = driver.VehicleNumber;
                           // CarNameRun.Text = data.CarName;

                            var ts = await client.TrackTaxi(data.BookingId);

                            if (ts != null)
                            {
                                if (ts.StatusId >= 6)
                                {
                                    if (Storage.SignInPastBookingIds == null) Storage.SignInPastBookingIds = new List<CurrentPastBookingDetails>();
                                    if (!Storage.SignInPastBookingIds.Contains(data))
                                        Storage.SignInPastBookingIds.Insert(0, data);
                                    Storage.SignInCurrentBookingIds.Remove(data);
                                }

                                //driver is assigned and current location is there
                                if (dataStore == "current")
                                {
                                    ShowOnMap(ts);
                                }
                                else
                                {
                                    AddOnMap(driver.PickupLocation, LocationOf.Customer, true);
                                }
                                if (!string.IsNullOrEmpty(ts.Message))
                                {
                                    grdMessage.Background = new SolidColorBrush(Color.FromArgb(255, 248, 218, 21));
                                    MessageBlock.Foreground = new SolidColorBrush(Colors.Black);
                                    MessageBlock.FontWeight = FontWeights.Bold;
                                    MessageBlock.Text = ts.Message;
                                }
                            }
                        }
                        else
                        {
                            //driver is not assigned or non vts
                            AddOnMap(driver.PickupLocation, LocationOf.Customer, true);
                            MessageBlock.Text = driver.Message;
                        }
                    }
                    else
                    {
                        //didn't return from call driver API successfully
                        GeoCoordinate geo = new GeoCoordinate();
                        string[] latlong = data.pickup_latlong.Split(',');
                        if (latlong.Length > 0)
                        {
                            geo.Latitude =Convert.ToDouble(latlong[0]);
                            geo.Longitude =Convert.ToDouble(latlong[1].ToString());
                            AddOnMap(geo, LocationOf.Customer, true);
                        }
                        MessageBlock.Text = "Details not available at the moment";
                    }

                    storesignIndata = data;
                }
                catch (Exception exp)
                {
                    Debug.WriteLine(exp.Message);
                }
            }
            else
            {
                pickFavourite.Visibility = Visibility.Collapsed;
                dropFavourite.Visibility = Visibility.Collapsed;

                try
                {
                    App myApp = (App)App.Current;
                    int index = int.Parse(NavigationContext.QueryString["index"]);
                    String dataStore = NavigationContext.QueryString["data"];
                    BookingData data;
                    if (dataStore == "current")
                    {
                        var defaultBooking = from names in Storage.CurrentBookingIds
                                             orderby names.Time ascending
                                             select names;
                        data = defaultBooking.ElementAt(index);
                    }
                    else
                    {
                        data = Storage.PastBookingIds.ElementAt(index);
                    }
                    VehicleNumberRun.Text = data.CarName;

                    DriverDetailPanel.Visibility = Visibility.Collapsed;
                    FromBlock.Text = data.From;
                    ToBlock.Text = data.To;

                    var client = new MyClient();
                    var driver = await client.CallDriver(data.BookingId);
                    if (driver != null)
                    {
                        if (driver.Number.Length > 0) DriverDetailPanel.Visibility = Visibility.Visible;
                        PhoneBlock.Text = driver.Number;
                        NameBlock.Text = driver.Name;
                        if (!String.IsNullOrWhiteSpace(driver.VehicleNumber)) VehicleGap.Text = "   ";

                        if (driver.Status == "Assigned")
                        {
                            VehicleNumberRun.Text = driver.VehicleNumber;
                            CarNameRun.Text = data.CarName;

                            var ts = await client.TrackTaxi(data.BookingId);

                            if (ts != null)
                            {
                                if (ts.StatusId >= 6)
                                {
                                    if (Storage.PastBookingIds == null) Storage.PastBookingIds = new List<BookingData>();
                                    if (!Storage.PastBookingIds.Contains(data))
                                        Storage.PastBookingIds.Insert(0, data);
                                    Storage.CurrentBookingIds.Remove(data);
                                }

                                //driver is assigned and current location is there
                                if (dataStore == "current")
                                {
                                    ShowOnMap(ts);
                                }
                                else
                                {
                                    AddOnMap(driver.PickupLocation, LocationOf.Customer, true);
                                }
                                if (!string.IsNullOrEmpty(ts.Message))
                                {
                                    grdMessage.Background = new SolidColorBrush(Color.FromArgb(255, 248, 218, 21));
                                    MessageBlock.Foreground = new SolidColorBrush(Colors.Black);
                                    MessageBlock.FontWeight = FontWeights.Bold;
                                    MessageBlock.Text = ts.Message;
                                }
                            }
                        }
                        else
                        {
                            //driver is not assigned or non vts
                            AddOnMap(driver.PickupLocation, LocationOf.Customer, true);
                            MessageBlock.Text = driver.Message;
                        }
                    }
                    else
                    {
                        // did n't return from call driver API successfully
                        MessageBlock.Text = "Details not available at the moment";
                    }
                    storedata = data;
                }
                catch (Exception exp)
                {
                    Debug.WriteLine(exp.Message);
                }
            }
        }

        private void ShowOnMap(TaxiStatus ts)
        {
            try
            {
                if (ts.Location.Longitude < 1 || ts.StatusId == 1 || ts.StatusId == 5) //1,5
                {
                    AddOnMap(ts.PickupLocation, LocationOf.Customer, true);
                }
                else if (ts.StatusId == 4)
                {
                    AddOnMap(ts.Location, LocationOf.CabAndCustomer, true);
                }
                else if (ts.StatusId == 2 || ts.StatusId == 6 || ts.StatusId == 3)
                {
                    AddOnMap(ts.PickupLocation, LocationOf.Customer, false);
                    AddOnMap(ts.Location, LocationOf.Cab, false);
                    double north = ts.Location.Latitude > ts.PickupLocation.Latitude
                       ? ts.Location.Latitude
                       : ts.PickupLocation.Latitude;
                    double south = ts.Location.Latitude < ts.PickupLocation.Latitude
                        ? ts.Location.Latitude
                        : ts.PickupLocation.Latitude;
                    double east = ts.Location.Longitude > ts.PickupLocation.Longitude
                        ? ts.Location.Longitude
                        : ts.PickupLocation.Longitude;
                    double west = ts.Location.Longitude < ts.PickupLocation.Longitude
                        ? ts.Location.Longitude
                        : ts.PickupLocation.Longitude;
                    north += 0.10;
                    west -= 0.05;
                    east += 0.05;
                    south -= 0.00;
                    LocationMap.SetView(new LocationRectangle(north, west, south, east));

                    // Mygeocodequery = new GeocodeQuery();
                    // Mygeocodequery.SearchTerm = "Bangalore Airport,India";
                    MyCoordinates.Add(new GeoCoordinate(ts.PickupLocation.Latitude, ts.PickupLocation.Longitude));
                    MyCoordinates.Add(new GeoCoordinate(ts.Location.Latitude, ts.Location.Longitude));
                    // Mygeocodequery.GeoCoordinate = new GeoCoordinate(ts.PickupLocation.Latitude, ts.PickupLocation.Longitude);
                    // Mygeocodequery.QueryCompleted += Mygeocodequery_QueryCompleted;
                    // Mygeocodequery.QueryAsync();

                    MyQuery = new RouteQuery();
                    MyQuery.Waypoints = MyCoordinates;
                    MyQuery.QueryCompleted += MyQuery_QueryCompleted;
                    MyQuery.QueryAsync();
                    Mygeocodequery.Dispose();
                }
                else
                {
                    AddOnMap(ts.PickupLocation, LocationOf.Customer, true);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        void Mygeocodequery_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            if (e.Error == null)
            {
                MyQuery = new RouteQuery();
                MyCoordinates.Add(e.Result[0].GeoCoordinate);
                MyQuery.Waypoints = MyCoordinates;
                MyQuery.QueryCompleted += MyQuery_QueryCompleted;
                MyQuery.QueryAsync();
                Mygeocodequery.Dispose();
            }
        }

        void MyQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            if (e.Error == null)
            {
                Route MyRoute = e.Result;
                MapRoute MyMapRoute = new MapRoute(MyRoute);
                LocationMap.AddRoute(MyMapRoute);
                MyQuery.Dispose();
            }
        }



        /// <summary>
        /// "cutr","cab","cabcust"
        /// </summary>
        /// <param name="gc"></param>
        private void AddOnMap(GeoCoordinate gc, LocationOf who, bool SetZoom)
        {
            var mapOverlay = new MapOverlay();
            mapOverlay.Content = getIcon(who);
            mapOverlay.GeoCoordinate = gc;
            //empirical
            mapOverlay.PositionOrigin = new Point(0.5, 1);
            MapLayer MyLayer = new MapLayer();
            MyLayer.Add(mapOverlay);
            LocationMap.Layers.Add(MyLayer);
            
            if (SetZoom)
            {
                LocationMap.ZoomLevel = 14;
                LocationMap.CartographicMode = MapCartographicMode.Road;
                LocationMap.Center = gc;
            }
        }

        enum LocationOf
        {
            Cab,
            Customer,
            CabAndCustomer
        }
        Grid getIcon(LocationOf who)
        {
            Grid MyGrid = new Grid();
            Image img = new Image();
            String path = "/Assets/driver_cab.png";
            switch (who)
            {
                case LocationOf.Customer:
                    path = "/Assets/customer.png";
                    break;
                case LocationOf.CabAndCustomer:
                    path = "/Assets/cabcust.png";
                    break;
            }
            img.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            MyGrid.Children.Add(img);
            //Ellipse e = new Ellipse();
            //e.Height = 10;
            //e.Width = 10;
            //e.Fill = new SolidColorBrush(Colors.Black);
            //MyGrid.Children.Add(e);
            return MyGrid;
        }

        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (PhoneBlock.Text.Length > 0)
            {
                PhoneCallTask pt = new PhoneCallTask();
                pt.DisplayName = NameBlock.Text;
                pt.PhoneNumber = "+91" + PhoneBlock.Text;
                pt.Show();
            }

        }

        private void pickFavourite_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Image img = (Image)sender;
            img.Opacity = 0.5;
        }

        private void dropFavourite_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Image img = (Image)sender;
            img.Opacity = 1;
        }

        private async void pickFavourite_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isPicked = true;
            popup.IsOpen = true;
            gridHolder.IsEnabled = false;
            Work.IsChecked = true;
            txtOther.Visibility = Visibility.Collapsed;
        }

        private async void dropFavourite_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isPicked = false;
            popup.IsOpen = true;
            gridHolder.IsEnabled = false;
            Work.IsChecked = true;
            txtOther.Visibility = Visibility.Collapsed;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
            gridHolder.IsEnabled = true;
            pickFavourite.Opacity = 1;
            dropFavourite.Opacity = 1;
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            string title=string.Empty;
            if(Other.IsChecked==true && string.IsNullOrEmpty(txtOther.Text.Trim()))
            {
                MessageBox.Show("Please enter other field.", "TaxiForSure", MessageBoxButton.OK);
                txtOther.Focus();
                return;
            }

            if (Work.IsChecked == true)
                title = "Work";
            else if (Office.IsChecked == true)
                title = "Office";
            else if (Hangout.IsChecked == true)
                title = "Hangout";
            else if (Other.IsChecked == true)
                title = txtOther.Text;
            if (isPicked)
            {
                MyClient client = new MyClient();
                if (Storage.IsLogin)
                {
                    List<FavouriteDetails> fav = new List<FavouriteDetails>();
                    FavouriteDetails favPicks = new FavouriteDetails();
                    favPicks.city = storesignIndata.pickup_city;
                    favPicks.address = storesignIndata.From;
                    favPicks.landmark = storesignIndata.pickup_area;
                    string[] latlong = storesignIndata.pickup_latlong.Split(',');
                    favPicks.latitude = latlong[0];
                    favPicks.longitude = latlong[1];
                    favPicks.title = title;
                    favPicks.type = "office";
                    fav.Add(favPicks);
                    FavouriteConfirmation status = await client.AddFavourites(SignInCustomer.UserId, fav, "5.2");
                    if (status!=null)
                    {
                        MessageBox.Show("Successfully added in favourite list.", "TaxiForSure", MessageBoxButton.OK);
                        pickFavourite.Source = new BitmapImage(new Uri("Assets/Icons/favourite.png", UriKind.RelativeOrAbsolute));
                        pickFavourite.MouseLeftButtonUp -= new System.Windows.Input.MouseButtonEventHandler(pickFavourite_MouseLeftButtonUp);
                        popup.IsOpen = false;
                        gridHolder.IsEnabled = true;
                        pickFavourite.Opacity = 1;
                    }
                    else
                    {
                        MessageBox.Show("Failed.", "TaxiForSure", MessageBoxButton.OK);
                    }
                }
            }
            else
            {
                MyClient client = new MyClient();
                if (Storage.IsLogin)
                {
                    List<FavouriteDetails> fav = new List<FavouriteDetails>();
                    FavouriteDetails favDrops = new FavouriteDetails();
                    favDrops.city = storesignIndata.pickup_city;
                    favDrops.address = storesignIndata.To;
                    favDrops.landmark = storesignIndata.drop_area;
                    string[] latlong = storesignIndata.pickup_latlong.Split(',');
                    favDrops.latitude = latlong[0];
                    favDrops.longitude = latlong[1];
                    favDrops.title = title;
                    favDrops.type = "office";
                    fav.Add(favDrops);
                    FavouriteConfirmation status = await client.AddFavourites(SignInCustomer.UserId, fav, "5.2");
                    if (status!=null)
                    {
                        if (status.Status == true)
                        {
                            MessageBox.Show("Successfully added in favourite list.", "TaxiForSure", MessageBoxButton.OK);
                            dropFavourite.Source = new BitmapImage(new Uri("Assets/Icons/favourite.png", UriKind.RelativeOrAbsolute));
                            dropFavourite.MouseLeftButtonUp -= new System.Windows.Input.MouseButtonEventHandler(dropFavourite_MouseLeftButtonUp);
                            popup.IsOpen = false;
                            gridHolder.IsEnabled = true;
                            dropFavourite.Opacity = 1;
                            txtOther.Text = string.Empty;
                        }
                        else
                        {
                            MessageBox.Show(status.Message, "TaxiForSure", MessageBoxButton.OK);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed.", "TaxiForSure", MessageBoxButton.OK);
                    }
                }
            }
        }

        private void Other_Checked(object sender, RoutedEventArgs e)
        {
            txtOther.Visibility = Visibility.Visible;
            txtOther.Focus();
        }

        private void Other_Unchecked(object sender, RoutedEventArgs e)
        {
            txtOther.Text = string.Empty;
            txtOther.Visibility = Visibility.Collapsed;
        }
    }
}