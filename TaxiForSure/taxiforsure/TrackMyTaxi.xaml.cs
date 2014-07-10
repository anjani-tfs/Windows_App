using Microsoft.Phone.Controls;
using Microsoft.Phone.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using TaxiforSure.Model;
using System.Linq;
using System.Xml.Linq;
namespace TaxiforSure
{
    public partial class TrackMyTaxi : PhoneApplicationPage
    {
        public TrackMyTaxi()
        {
            InitializeComponent();
            MainPivot.Title = "View & Track";
            //          PastBookingList.ItemsSource = GetPastBooking();
        }

        void DoneBtn_Click(object sender, RoutedEventArgs e)
        {
            //       NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        void TrackMyTaxi_Loaded(object sender, RoutedEventArgs e)
        {
            if (MainPivot.SelectedIndex > 0)
            {
                PastBooking.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (Storage.CurrentBookingIds!=null && Storage.CurrentBookingIds.Count > 0)
            {
                WaitingBar.Visibility = Visibility.Visible;
            }
            try
            {
                CurrentBookingList.ItemsSource = PastBookingList.ItemsSource = null;
                CurrentBookingList.SelectedIndex = -1;
                PastBookingList.SelectedIndex = -1;

                //TODO
                var client = new MyClient();
                var ts = await client.BookingStatus(Storage.CurrentBookingIds);
                if (ts != null && ts.Count > 0)
                {
                    if (Storage.PastBookingIds == null) Storage.PastBookingIds = new List<BookingData>();
                    foreach (var bookingData in ts)
                    {
                        Storage.PastBookingIds.Insert(0, bookingData);
                        Storage.CurrentBookingIds.Remove(bookingData);
                    }

                    var ModifiedBooking = from names in Storage.CurrentBookingIds
                                          orderby names.Time ascending
                                          select names;

                    try
                    {
                        if (Storage.PastBookingIds.Count > 10)
                        {
                            for (int i = 10; i < Storage.PastBookingIds.Count; i++)
                            {
                                Storage.PastBookingIds.RemoveAt(i);
                            }
                        }
                    }
                    catch
                    {
                    }


                    try
                    {
                        for (int j = 0; j < ts.Count; j++)
                        {
                            if (j < 10)
                            {
                                var bookingInfo = await client.BookingInfo(Storage.PastBookingIds[j].BookingId);
                                if (bookingInfo != null)
                                {
                                    if (bookingInfo.Fare == "0")
                                    {
                                        Storage.PastBookingIds[j].Fare = "₹ " + bookingInfo.Fare;
                                    }
                                    else
                                    {
                                        Storage.PastBookingIds[j].Fare = "₹ " + bookingInfo.Fare + "/-";
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    catch
                    {
                    }

                    CurrentBookingList.ItemsSource = PastBookingList.ItemsSource = null;
                    CurrentBookingList.ItemsSource = ModifiedBooking;
                    PastBookingList.ItemsSource = Storage.PastBookingIds;
                    CurrentBookingList.SelectedIndex = -1;
                    PastBookingList.SelectedIndex = -1;
                }
                else
                {
                    var defaultBooking = from names in Storage.CurrentBookingIds
                                         orderby names.Time ascending
                                         select names;
                    CurrentBookingList.ItemsSource = defaultBooking;
                    PastBookingList.ItemsSource = Storage.PastBookingIds;
                    CurrentBookingList.SelectedIndex = -1;
                    PastBookingList.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                WaitingBar.Visibility = Visibility.Collapsed;
            }
        }

        private void Pivot_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (MainPivot.SelectedIndex >= 0)
            {
                PastBooking.Foreground = new SolidColorBrush(Colors.Black);
            }

        }

        private async void ApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {

            //    var client = new MyClient();
            //    var ts = await client.TrackTaxi(BookingIdBox.Text);
            //    if (ts == null)
            //        MessageBox.Show("There was an error!", "Sorry!", MessageBoxButton.OK);
            //    else
            //        MessageBox.Show(ts.Message, ts.Status, MessageBoxButton.OK);
            //    NavigationService.Navigate(new Uri("/Location.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = (ListBox)sender;
            if (list.SelectedIndex != -1)
            {
                if (!DeviceNetworkInformation.IsNetworkAvailable)
                {
                    MessageBox.Show("The Application needs internet to operate.", "TaxiForSure", MessageBoxButton.OK);
                }
                else
                    NavigationService.Navigate(new Uri("/Location.xaml?data=current&index=" + list.SelectedIndex, UriKind.RelativeOrAbsolute));
            }
        }
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/HomePage.xaml", UriKind.RelativeOrAbsolute));
        }
        private void PastBookingList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = (ListBox)sender;
            if (list.SelectedIndex != -1)
            {
                if (!DeviceNetworkInformation.IsNetworkAvailable)
                {
                    MessageBox.Show("The Application needs internet to operate.", "TaxiForSure", MessageBoxButton.OK);
                }
                else
                    NavigationService.Navigate(new Uri("/Location.xaml?data=past&index=" + list.SelectedIndex, UriKind.RelativeOrAbsolute));
            }
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
                var m = (MenuItem)sender;
                var bookingData = (BookingData)m.Tag;
                (App.Current as App).CancelBookingData = bookingData;


                NavigationService.Navigate(new Uri("/CancelBooking.xaml",
                    UriKind.RelativeOrAbsolute));
        }
    }
}