using Microsoft.Phone.Controls;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace TaxiforSure
{
    public partial class BookingConfirmed : PhoneApplicationPage
    {
        public BookingConfirmed()
        {
            InitializeComponent();
            App myApp = (App)App.Current;
            if (myApp.isPickNow == true)
            {
                MessageBlock.Text = "";
            }
        }
        bool isConfirmed, isWait;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);



            try
            {
                isConfirmed = bool.Parse(NavigationContext.QueryString["IsConfirmed"]);
                isWait = bool.Parse(NavigationContext.QueryString["IsWait"]);
            }
            catch
            {
                isConfirmed = false;
                isWait = false;
            }
            if (isConfirmed)
            {
                //     ConfirmBlock.Text = "Booking Confirmed";
            }
            else if (!isConfirmed)
            {
                //     ConfirmBlock.Text = "Our customer care will get in touch to confirm the booking";
                MessageBlock.Text = NavigationContext.QueryString["Message"];
            }
            else if (isWait)
            {
                //   ConfirmBlock.Text = "Please wait";
                MessageBlock.Text = NavigationContext.QueryString["Message"];
            }
            else
            {
                MessageBlock.Text = NavigationContext.QueryString["Message"];
            }
            IdBlock.Text = "Booking ID: " + NavigationContext.QueryString["Id"];
            if (IdBlock.Text.Contains("unavail"))
            {
                IdBlock.Visibility = System.Windows.Visibility.Collapsed;
                confirmedImage.Source = new BitmapImage(new Uri("Assets/Icons/cancel.png", UriKind.Relative));
            }
            else
            {
                confirmedImage.Source = new BitmapImage(new Uri("Assets/Icons/Confirmed.png", UriKind.Relative));
            }

        }

        private void DoneBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isConfirmed)
                NavigationService.Navigate(new Uri("/HomePage.xaml", UriKind.RelativeOrAbsolute));

            NavigationService.Navigate(new Uri("/TrackMyTaxi.xaml", UriKind.RelativeOrAbsolute));
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            e.Cancel = true;
            NavigationService.Navigate(new Uri("/HomePage.xaml", UriKind.RelativeOrAbsolute));
            base.OnBackKeyPress(e);
        }
    }
}