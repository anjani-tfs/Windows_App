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

        public Location()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);
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
                            ShowOnMap(ts);
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
                    MessageBlock.Text = "Details not available at the moment";
                }
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.Message);
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

    }
}