using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using TaxiforSure.Model;

namespace TaxiforSure
{
    public partial class CancelBooking : PhoneApplicationPage
    {
        public CancelBooking()
        {
            InitializeComponent();
        }

        private BookingData bookingData;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            bookingData = (App.Current as App).CancelBookingData;
            FromBlock.Text = bookingData.From;
            ToBlock.Text = bookingData.To;
            TimeBlock.Text = bookingData.Time;
            CarBlock.Text = bookingData.CarName;
            try
            {
                string[] bookingFair = bookingData.Fare.Split(',');
                if (bookingFair.Length == 2)
                {
                    CarFare.Text = bookingFair[0].ToString();
                    CarExtraFare.Text = bookingFair[1].ToString().Trim();
                }
                else
                {
                    CarFare.Text = bookingFair[0].ToString();
                    CarExtraFare.Text = string.Empty;
                }
            }
            catch
            {
            }
        }

        private async void ApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {
            //check if cab can be cancelled

            var reason = "No reason.";
            this.Focus();
            if (!String.IsNullOrWhiteSpace(ReasonBox.Text))
            {
                reason = ReasonBox.Text;
            }

            var client = new MyClient();
            LoadinGrid.Visibility = Visibility.Visible;
            ApplicationBar.IsVisible = false;

            try
            {

                //insert condition here
                

                var result = await client.CancelBooking(bookingData.BookingId, reason);
                if (result)
                {
                    MessageBox.Show("Booking Cancelled.", "TaxiForSure", MessageBoxButton.OK);
                    //Storage.CurrentBookingIds.Remove(bookingData);
                    //if (Storage.PastBookingIds == null) Storage.PastBookingIds = new List<BookingData>();
                    //Storage.PastBookingIds.Insert(0, bookingData);
                    NavigationService.GoBack();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            LoadinGrid.Visibility = Visibility.Collapsed;
            ApplicationBar.IsVisible = true;
        }

        private void ReasonBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
            }
        }
    }
}