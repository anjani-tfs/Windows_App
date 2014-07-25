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
using System.Text.RegularExpressions;
namespace TaxiforSure
{
    public partial class TrackMyTaxi : PhoneApplicationPage
    {
        MyClient clientglobal = new MyClient();

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

            Pivot2.IsEnabled = true;
            Pivot1.IsEnabled = true;
            MainPivot.IsEnabled = true;
            MainPivot.Opacity = 1;
            Pivot2.Opacity = 1;
            Pivot1.Opacity = 1;
            popup.IsOpen = false;
            popupCurrentBooking.IsOpen = false;

            if (Storage.IsLogin)
            {
                //WaitingBar.Visibility = Visibility.Visible;
                var passBookingDetails1 = await clientglobal.RetryCurrentPastBookings(SignInCustomer.Number.ToString(), SignInCustomer.UserId, "5.2");
                if (passBookingDetails1)
                {
                    if (Storage.SignInCurrentBookingIds != null && Storage.SignInCurrentBookingIds.Count > 0)
                    {
                        WaitingBar.Visibility = Visibility.Visible;
                    }
                    try
                    {
                        CurrentBookingList.ItemsSource = PastBookingList.ItemsSource = null;
                        CurrentBookingList.SelectedIndex = -1;
                        PastBookingList.SelectedIndex = -1;
                        CurrentBookingList.ItemsSource = Storage.SignInCurrentBookingIds;
                        PastBookingList.ItemsSource = Storage.SignInPastBookingIds;
                        //    PastBookingList.ItemsSource = Storage.SignInPastBookingIds;
                        //    CurrentBookingList.SelectedIndex = -1;
                        //    PastBookingList.SelectedIndex = -1;
                        //TODO
                        //var client = new MyClient();
                        //var ts = await client.SignInBookingStatus(Storage.SignInCurrentBookingIds);
                        //if (ts != null && ts.Count > 0)
                        //{
                        //    if (Storage.SignInPastBookingIds == null) Storage.SignInPastBookingIds = new List<CurrentPastBookingDetails>();
                        //    foreach (var bookingData in ts)
                        //    {
                        //        Storage.SignInPastBookingIds.Insert(0, bookingData);
                        //        Storage.SignInCurrentBookingIds.Remove(bookingData);
                        //    }

                        //    var ModifiedBooking = from names in Storage.SignInCurrentBookingIds
                        //                          orderby names.pickup_time ascending
                        //                          select names;

                        //    try
                        //    {
                        //        if (Storage.SignInPastBookingIds.Count > 10)
                        //        {
                        //            for (int i = 10; i < Storage.SignInPastBookingIds.Count; i++)
                        //            {
                        //                Storage.SignInPastBookingIds.RemoveAt(i);
                        //            }
                        //        }
                        //    }
                        //    catch
                        //    {
                        //    }


                        //    try
                        //    {
                        //        for (int j = 0; j < ts.Count; j++)
                        //        {
                        //            if (j < 10)
                        //            {
                        //                var bookingInfo = await client.BookingInfo(Storage.SignInPastBookingIds[j].BookingId);
                        //                if (bookingInfo != null)
                        //                {
                        //                    if (bookingInfo.Fare == "0")
                        //                    {
                        //                        Storage.SignInPastBookingIds[j].Fare = "₹ " + bookingInfo.Fare;
                        //                    }
                        //                    else
                        //                    {
                        //                        Storage.SignInPastBookingIds[j].Fare = "₹ " + bookingInfo.Fare + "/-";
                        //                    }
                        //                }
                        //            }
                        //            else
                        //            {
                        //                break;
                        //            }
                        //        }
                        //    }
                        //    catch
                        //    {
                        //    }

                        //    CurrentBookingList.ItemsSource = PastBookingList.ItemsSource = null;
                        //    CurrentBookingList.ItemsSource = ModifiedBooking;
                        //    PastBookingList.ItemsSource = Storage.SignInPastBookingIds;
                        //    CurrentBookingList.SelectedIndex = -1;
                        //    PastBookingList.SelectedIndex = -1;
                        //}
                        //else
                        //{
                        //    var defaultBooking = from names in Storage.SignInCurrentBookingIds
                        //                         orderby names.pickup_time ascending
                        //                         select names;
                        //    CurrentBookingList.ItemsSource = defaultBooking;
                        //    PastBookingList.ItemsSource = Storage.SignInPastBookingIds;
                        //    CurrentBookingList.SelectedIndex = -1;
                        //    PastBookingList.SelectedIndex = -1;
                        //}
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
            }
            else
            {
                if (Storage.CurrentBookingIds != null && Storage.CurrentBookingIds.Count > 0)
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

        private void OnSelectionChangedPast(object sender, SelectionChangedEventArgs e)
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
            if (Storage.IsLogin)
            {
                 var m = (MenuItem)sender;
                var bookingData = (CurrentPastBookingDetails)m.Tag;
                (App.Current as App).SignInCancelBookingData = bookingData;


                NavigationService.Navigate(new Uri("/CancelBooking.xaml",
                    UriKind.RelativeOrAbsolute));
            }
            else
            {
                var m = (MenuItem)sender;
                var bookingData = (BookingData)m.Tag;
                (App.Current as App).CancelBookingData = bookingData;


                NavigationService.Navigate(new Uri("/CancelBooking.xaml",
                    UriKind.RelativeOrAbsolute));
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Focus();
            Pivot2.IsEnabled = true;
            MainPivot.IsEnabled = true;
            MainPivot.Opacity = 1;
            Pivot2.Opacity = 1;
            popup.IsOpen = false;
        }


        private void btnPastBookingMenu_Click(object sender, RoutedEventArgs e)
        {
            Button text = (Button)sender;
            Pivot2.IsEnabled = false;
            MainPivot.IsEnabled = false;
            MainPivot.Opacity = 0.1;
            Pivot2.Opacity = 0.1;
            popup.IsOpen = true;

            if (Storage.IsLogin)
            {
                SignInCustomer.BookingId = text.Tag.ToString();
            }
            else
            {
                Customer.BookingId = text.Tag.ToString();
            }
        }

        private void btnCurrentBookingMenu_Click(object sender, RoutedEventArgs e)
        {
            Button text = (Button)sender;
            Pivot1.IsEnabled = false;
            MainPivot.IsEnabled = false;
            MainPivot.Opacity = 0.1;
            Pivot1.Opacity = 0.1;
            popupCurrentBooking.IsOpen = true;
            if (Storage.IsLogin)
            {
                SignInCustomer.BookingId = text.Tag.ToString();
            }
            else
            {
                Customer.BookingId = text.Tag.ToString();
            }
        }

        private async void receipt_Click(object sender, RoutedEventArgs e)
        {
            if (Storage.IsLogin)
            {
                txtEmail.Text = SignInCustomer.Email;
            }
            else
            {
                txtEmail.Text = Customer.Email;
            }
            receipt.Visibility = Visibility.Collapsed;
            Cancel.Visibility = Visibility.Collapsed;
            txtEmail.Visibility = Visibility.Visible;
            receiptSend.Visibility = Visibility.Visible;
            receiptCancel.Visibility = Visibility.Visible;
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            this.Focus();
            NavigationService.Navigate(new Uri("/EditBookingDetails.xaml",UriKind.RelativeOrAbsolute));
        }

        private void edit_GotFocus(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            b.Opacity = 0.5;
        }

        private void edit_LostFocus(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            b.Opacity = 1;
        }

        private void edit_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Focus();
        }

        private void edit_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Focus();
        }

        private void CancelBooking_Click(object sender, RoutedEventArgs e)
        {
            if (Storage.IsLogin)
            {
                //var m = (MenuItem)sender;
                //var bookingData = (CurrentPastBookingDetails)m.Tag;
                //(App.Current as App).SignInCancelBookingData = bookingData;

                for (int i = 0; i < Storage.SignInCurrentBookingIds .Count; i++)
                {
                    if (Storage.SignInCurrentBookingIds[i].BookingId == SignInCustomer.BookingId)
                    {
                        (App.Current as App).SignInCancelBookingData = Storage.SignInCurrentBookingIds[i];
                        break;
                    }
                }

                NavigationService.Navigate(new Uri("/CancelBooking.xaml",
                    UriKind.RelativeOrAbsolute));
            }
            else
            {
                for (int i = 0; i < Storage.CurrentBookingIds.Count; i++)
                {
                    if (Storage.CurrentBookingIds[i].BookingId == Customer.BookingId)
                    {
                        (App.Current as App).CancelBookingData = Storage.CurrentBookingIds[i];
                        break;
                    }
                }

                NavigationService.Navigate(new Uri("/CancelBooking.xaml",
                    UriKind.RelativeOrAbsolute));
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Focus();
            Pivot1.IsEnabled = true;
            MainPivot.IsEnabled = true;
            MainPivot.Opacity = 1;
            Pivot1.Opacity = 1;
            popupCurrentBooking.IsOpen = false;
        }

        private async void receiptSend_Click(object sender, RoutedEventArgs e)
        {
            this.Focus();

            if (Storage.IsLogin)
            {
                if (string.IsNullOrEmpty(txtEmail.Text))
                {
                    MessageBox.Show("Please enter your mail id.", "TaxiForSure", MessageBoxButton.OK);
                    return;
                }

                if (!Regex.IsMatch(txtEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    MessageBox.Show("Please enter valid mail id.", "TaxiForSure", MessageBoxButton.OK);
                    return;
                }
                string response = await clientglobal.GetTripReceipt(SignInCustomer.BookingId, SignInCustomer.Email, "5.2");
                if (!string.IsNullOrEmpty(response))
                {
                    MessageBox.Show(response, "TaxiForSure", MessageBoxButton.OK);
                    txtEmail.Visibility = Visibility.Collapsed;
                    receiptSend.Visibility = Visibility.Collapsed;
                    receiptCancel.Visibility = Visibility.Collapsed;
                    receipt.Visibility = Visibility.Visible;
                    Cancel.Visibility = Visibility.Visible;
                    this.Focus();
                    Pivot2.IsEnabled = true;
                    MainPivot.IsEnabled = true;
                    MainPivot.Opacity = 1;
                    Pivot2.Opacity = 1;
                    popup.IsOpen = false;
                    txtEmail.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("Message sending failed.", "TaxiForSure", MessageBoxButton.OK);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtEmail.Text))
                {
                    MessageBox.Show("Please enter your mail id.", "TaxiForSure", MessageBoxButton.OK);
                    return;
                }

                if (!Regex.IsMatch(txtEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    MessageBox.Show("Please enter valid mail id.", "TaxiForSure", MessageBoxButton.OK);
                    return;
                }
                string response = await clientglobal.GetTripReceipt(Customer.BookingId, Customer.Email, "5.2");
                if (!string.IsNullOrEmpty(response))
                {
                    MessageBox.Show(response, "TaxiForSure", MessageBoxButton.OK);
                    txtEmail.Visibility = Visibility.Collapsed;
                    receiptSend.Visibility = Visibility.Collapsed;
                    receiptCancel.Visibility = Visibility.Collapsed;
                    receipt.Visibility = Visibility.Visible;
                    Cancel.Visibility = Visibility.Visible;
                    this.Focus();
                    Pivot2.IsEnabled = true;
                    MainPivot.IsEnabled = true;
                    MainPivot.Opacity = 1;
                    Pivot2.Opacity = 1;
                    popup.IsOpen = false;
                    txtEmail.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("Message sending failed.", "TaxiForSure", MessageBoxButton.OK);
                }
            }
        }

        private void receiptCancel_Click(object sender, RoutedEventArgs e)
        {
            txtEmail.Visibility = Visibility.Collapsed;
            receiptSend.Visibility = Visibility.Collapsed;
            receiptCancel.Visibility = Visibility.Collapsed;
            receipt.Visibility = Visibility.Visible;
            Cancel.Visibility = Visibility.Visible;
            this.Focus();
            Pivot2.IsEnabled = true;
            MainPivot.IsEnabled = true;
            MainPivot.Opacity = 1;
            Pivot2.Opacity = 1;
            popup.IsOpen = false;
        }
    }
}