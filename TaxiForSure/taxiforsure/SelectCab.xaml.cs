using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using TaxiforSure.Model;

namespace TaxiforSure
{
    public partial class SelectCab : PhoneApplicationPage
    {
        ObservableCollection<CarDetails> carTab1, carTab2;
        public static int selectedIndex = 0;
        public static bool isFixed = false;

        public SelectCab()
        {
            InitializeComponent();
            ApplicationBar.IsVisible = false;
            optionStack.Visibility = System.Windows.Visibility.Collapsed;
            carTab1 = new ObservableCollection<CarDetails>();
            carTab2 = new ObservableCollection<CarDetails>();
            tab1.Tap += tab1_Tap;
            tab2.Tap += tab2_Tap;
            isFixed = false;
        }

        void tab2_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (tab2.Foreground != new SolidColorBrush(Colors.Black))
            {
                isFixed = true;
                _cars = carTab2;

                CarListSelector.ItemsSource = carTab2;

                CarListSelector.UpdateLayout();

                Query.Car = carTab2.ElementAt(0);
                carTab2.ElementAt(0).IsSelected = true;
                selectedIndex = 0;
                tab2.Foreground = new SolidColorBrush(Colors.Black);
                tab1.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        void tab1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (tab1.Foreground != new SolidColorBrush(Colors.Black))
            {
                isFixed = false;
                _cars = carTab1;

                CarListSelector.ItemsSource = carTab1;

                CarListSelector.UpdateLayout();

                Query.Car = carTab1.ElementAt(0);
                carTab1.ElementAt(0).IsSelected = true;

                selectedIndex = 0;
                tab2.Foreground = new SolidColorBrush(Colors.White);
                tab1.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private ObservableCollection<CarDetails> _cars;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (Query.Car == null && _cars != null)
            {
                Query.Car = (from c in _cars where c.IsSelected == true select c).First();

            }
        }

        private async void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            App myApp = App.Current as App;

            try
            {
                //if (CarListSelector.ItemsSource == null)
                //{
                //    if (Query.Car == null)
                //    {
                        var client = new MyClient();
                        LoadingGrid.Visibility = Visibility.Visible;
                       
                        if (myApp.IsSourceAirport || myApp.IsTargetAirport) myApp.BookingType = "at";
                        else
                        {
                            myApp.BookingType = "p2p";
                        }
                        _cars = await client.Fare();

                        if ((_cars == null) && (myApp.IsSourceAirport || myApp.IsTargetAirport))
                        {

                        }


                        if (myApp.IsSourceAirport || myApp.IsTargetAirport)
                        {
                            optionStack.Visibility = System.Windows.Visibility.Visible;

                            if (myApp.IsSourceAirport)
                            {

                                if (myApp.cars_from_at_perKM.Count > 0)
                                {
                                    tab1.Text = "Per KM";
                                    carTab1 = myApp.cars_from_at_perKM;
                                    carTab1.ElementAt(0).IsSelected = true;
                                    _cars = myApp.cars_from_at_perKM;
                                }
                                if (myApp.cars_from_at_perKM.Count == 0)
                                {
                                    tab1.Visibility = System.Windows.Visibility.Collapsed;
                                    tab2.Foreground = new SolidColorBrush(Colors.Black);
                                    tab_between.Visibility = System.Windows.Visibility.Collapsed;
                                }
                                if (myApp.cars_from_at_fixed.Count > 0)
                                {
                                    tab2.Text = "Fixed Fare";
                                    carTab2 = myApp.cars_from_at_fixed;
                                    carTab2.ElementAt(0).IsSelected = true;
                                    if (myApp.cars_from_at_perKM.Count == 0)
                                        _cars = myApp.cars_from_at_fixed;
                                }
                                if (myApp.cars_from_at_fixed.Count == 0)
                                {
                                    tab1.Foreground = new SolidColorBrush(Colors.Black);
                                    tab2.Visibility = System.Windows.Visibility.Collapsed;
                                    tab_between.Visibility = System.Windows.Visibility.Collapsed;
                                }

                            }
                            else
                            {

                                if (myApp.cars_at_km.Count > 0)
                                {
                                    tab1.Text = "Per KM";
                                    carTab1 = myApp.cars_at_km;
                                    carTab1.ElementAt(0).IsSelected = true;
                                    _cars = myApp.cars_at_km;
                                }
                                if (myApp.cars_at_km.Count == 0)
                                {
                                    tab2.Foreground = new SolidColorBrush(Colors.Black);
                                    tab1.Visibility = System.Windows.Visibility.Collapsed;
                                    tab_between.Visibility = System.Windows.Visibility.Collapsed;
                                }
                                if (myApp.cars_at.Count > 0)
                                {
                                    carTab2 = myApp.cars_at;
                                    carTab2.ElementAt(0).IsSelected = true;
                                    tab2.Text = "Fixed Fare";
                                    if (myApp.cars_at_km.Count == 0)
                                        _cars = myApp.cars_at;
                                }
                                if (myApp.cars_at.Count == 0)
                                {
                                    tab1.Foreground = new SolidColorBrush(Colors.Black);
                                    tab2.Visibility = System.Windows.Visibility.Collapsed;
                                    tab_between.Visibility = System.Windows.Visibility.Collapsed;
                                }
                            }
                            if (tab1.Visibility == System.Windows.Visibility.Visible)
                            {
                                tab1.Foreground = new SolidColorBrush(Colors.Black);
                                if (tab2.Visibility == System.Windows.Visibility.Visible)
                                {
                                    tab_between.Visibility = System.Windows.Visibility.Visible;
                                    tab2.Foreground = new SolidColorBrush(Colors.White);
                                }
                            }

                            if (Storage.City == "Delhi")
                            {
                                tab1.Visibility = Visibility.Collapsed;
                            }
                            if (Storage.City == "Chennai")
                            {
                                tab1.Visibility = Visibility.Collapsed;
                            }
                        }
                        else
                        {

                        }

                        CarListSelector.ItemsSource = _cars;

                        if (_cars.Count > 0)
                        {
                            ApplicationBar.IsVisible = true;
                            //select first elemt
                            Query.Car = _cars.ElementAt(0);
                            _cars.ElementAt(0).IsSelected = true;
                        }
                        else
                        {
                            MessageBox.Show("Cannot find cabs for this route.", "TaxiForSure", MessageBoxButton.OK);
                            NavigationService.Navigate(new Uri("/HomePage.xaml", UriKind.RelativeOrAbsolute));
                        }
                //    }
                //}
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }

            if (myApp.IsSourceAirport || myApp.IsTargetAirport)
            {
                if (isFixed == false)
                {
                    _cars = carTab1;

                    CarListSelector.ItemsSource = carTab1;

                    tab2.Foreground = new SolidColorBrush(Colors.White);
                    tab1.Foreground = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    _cars = carTab2;

                    CarListSelector.ItemsSource = carTab2;

                    tab2.Foreground = new SolidColorBrush(Colors.Black);
                    tab1.Foreground = new SolidColorBrush(Colors.White);
                }
            }
            if (_cars.Count > 0)
            {
                Query.Car = _cars.ElementAt(selectedIndex);
                _cars.ElementAt(selectedIndex).IsSelected = true;
            }

            LoadingGrid.Visibility = Visibility.Collapsed;
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            Query.Car = null;
            base.OnBackKeyPress(e);
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            //    var r = (RadioButton)sender;
            //    if (r.Tag != null)
            //        Query.Car = (CarDetails)r.Tag;
            //    else
            //    {
            //        Debug.WriteLine("Tag is null");
            //    }
        }

        private void Proceed_Click(object sender, EventArgs e)
        {
            if (Query.Car != null)
                NavigationService.Navigate(new Uri("/PassengerDetail.xaml", UriKind.RelativeOrAbsolute));
        }

        private void CarListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CarListSelector.SelectedItem != null)
            {
                if (Query.Car != null) Query.Car.IsSelected = false;
                Query.Car = (CarDetails)CarListSelector.SelectedItem;
                Query.Car.IsSelected = true;
                for (int i = 0; i < _cars.Count; i++)
                {
                    if (_cars[i].CarName == Query.Car.CarName)
                    {
                        
                        selectedIndex = i;
                        _cars.ElementAt(i).IsSelected = true;
                        Query.Car.BaseFare = _cars[i].BaseFare;
                        Query.Car.BaseKm = _cars[i].BaseKm;
                        Query.Car.ExtraKmFare = _cars[i].ExtraKmFare;
                        //break;
                    }
                }
                 
                //  App myApp = (App)App.Current;

            }
        }
    }
}