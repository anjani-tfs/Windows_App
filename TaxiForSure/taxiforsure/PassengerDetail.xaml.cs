using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using TaxiforSure.Model;

namespace TaxiforSure
{
    public partial class PassengerDetail : PhoneApplicationPage
    {
        BookingStatus taxiStatus;
        bool isTaxiBooking;
        public PassengerDetail()
        {
            InitializeComponent();

            
            isTaxiBooking = false;

            if (Storage.IsLogin)
            {
                PhoneBox.Text = SignInCustomer.Number.ToString();
                NameBox.Text = SignInCustomer.Name;
                EmailBox.Text = SignInCustomer.Email;
            }
            else
            {
                NameBox.Text = Customer.Name;
                EmailBox.Text = Customer.Email;
                if (Customer.Number != 0)
                    PhoneBox.Text = Customer.Number.ToString();
            }
        }

     
        async void ProceedBtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(PhoneBox.Text))
            {
                MessageBox.Show("Please enter a valid phone number.", "TaxiForSure", MessageBoxButton.OK);
                return;
            }
            if (Storage.IsLogin)
            {
                if (String.IsNullOrWhiteSpace(NameBox.Text))
                {
                    SignInCustomer.Name = "Guest";
                }
                else
                {
                    SignInCustomer.Name = NameBox.Text;
                }
                if (String.IsNullOrWhiteSpace(EmailBox.Text))
                {
                    SignInCustomer.Email = "no.email@example.com";
                }
                else
                {
                    SignInCustomer.Email = EmailBox.Text.ToLower();
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(NameBox.Text))
                {
                    Customer.Name = "Guest";
                }
                else
                {
                    Customer.Name = NameBox.Text;
                }
                if (String.IsNullOrWhiteSpace(EmailBox.Text))
                {
                    Customer.Email = "no.email@example.com";
                }
                else
                {
                    Customer.Email = EmailBox.Text.ToLower();
                }
            }


            if (!Regex.IsMatch(PhoneBox.Text, @"^(?![012])(\d{10})$"))
            {
                MessageBox.Show("Enter a valid 10 digit number.", "TaxiForSure", MessageBoxButton.OK);
                return;
            }
            if (!Regex.IsMatch(EmailBox.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                if (!string.IsNullOrWhiteSpace(EmailBox.Text))
                {
                    MessageBox.Show("Enter a valid email id.", "TaxiForSure", MessageBoxButton.OK);
                    return;
                }

            }
            //Validate
            Query.CouponCode = CouponBox.Text;

            if (Storage.IsLogin)
            {
                SignInCustomer.Email = EmailBox.Text;
            }
            else
            {
                Customer.Email = EmailBox.Text;
            }
            try
            {
                App myApp = (App)App.Current;

                if (Storage.IsLogin)
                {
                    SignInCustomer.Number = long.Parse(PhoneBox.Text);
                }
                else
                {
                    Customer.Number = long.Parse(PhoneBox.Text);
                }
                LoadinGrid.Visibility = Visibility.Visible;
                if (Query.PickNow == true)
                {
                    LoadingText.Text = "Contacting nearby taxis. Please Wait...";
                }
                else
                {
                    LoadingText.Text = "Please wait...";
                }
                this.Focus();
                ApplicationBar.IsVisible = false;
            

                    var client = new MyClient();
                    var result = await client.BookTaxi();

                    if (result != null && result.IsConfirmed)
                    {
                        string FareLocal=string.Empty;
                        if (!string.IsNullOrEmpty(CarExtraFare.Text))
                        {
                            FareLocal = CarFare.Text + ", " + CarExtraFare.Text;
                        }
                        else
                        {
                            FareLocal = CarFare.Text;
                        }

                        if (myApp.isPickNow)
                        {
                            string[] datetime = Query.PickupTime.ToString("g").Split(' ');

                            string date = string.Empty;

                            if (datetime.Length > 0)
                            {
                                date = datetime[0].ToString().Trim();
                            }
                            else
                            {
                                date=Query.PickupTime.ToString("g");
                            }

                            if (Storage.CurrentBookingIds == null) Storage.CurrentBookingIds = new List<BookingData>();
                            Storage.CurrentBookingIds.Insert(0,
                                new BookingData
                                {
                                    BookingId = result.BookingId.ToUpper(),
                                    CarName = Query.Car.CarName,
                                    From = Query.Pickup.Name,
                                    To = Query.Drop.Name,
                                    Time =date,// Query.PickupTime.ToString("g"),
                                    Fare=FareLocal

                                });
                            TimeBlock.Text = "Earliest Available";
                        }
                        else
                        {
                            if (Storage.CurrentBookingIds == null) Storage.CurrentBookingIds = new List<BookingData>();
                            Storage.CurrentBookingIds.Insert(0,
                                new BookingData
                                {
                                    BookingId = result.BookingId.ToUpper(),
                                    CarName = Query.Car.CarName,
                                    From = Query.Pickup.Name,
                                    To = Query.Drop.Name,
                                    Time = Query.PickupTime.ToString("g"),
                                    Fare = FareLocal
                                });

                        }

                        NavigationService.Navigate(new Uri("/BookingConfirmed.xaml?Id=" + result.BookingId +
                            "&Message=" + result.Message +
                            "&IsConfirmed=" + result.IsConfirmed +
                            "&IsWait=" + result.IsWaiting, UriKind.RelativeOrAbsolute));
                    }
                    //else if (  )
                    //{

                    //    MessageBox.Show("You will be getting a call soon!", "TaxiForSure", MessageBoxButton.OK);
                    //    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
                    //}

                    else
                    {
                        if (myApp.isPickNow)
                        {
                            NavigationService.Navigate(new Uri("/BookingConfirmed.xaml?Id=" + "unavailable" +
                                        "&Message=" + "Please wait while our team finds you a taxi. We will send you an SMS shortly." +
                                        "&IsConfirmed=" + " not confirmed " +
                                        "&IsWait=" + "false", UriKind.RelativeOrAbsolute));
                        }
                        else
                        {
                            NavigationService.Navigate(new Uri("/BookingConfirmed.xaml?Id=" + "unavailable" +
                                        "&Message=" + "Please wait while our representative gives you a call." +
                                        "&IsConfirmed=" + " not confirmed " +
                                        "&IsWait=" + "false", UriKind.RelativeOrAbsolute));
                        }
                    }
                    taxiStatus = result;
                
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Please enter your 10 digit mobile number.", "TaxiForSure", MessageBoxButton.OK);
                PhoneBox.Focus();
               // Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
               // Debug.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message);
                MessageBox.Show("Something went wrong. Please try again after some time.", "TaxiForSure", MessageBoxButton.OK);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CarBlock.Text = Query.Car.CarName + "";
            FromBlock.Text = Query.Pickup.Name + "";
            ToBlock.Text = Query.Drop.Name + "";
            TimeBlock.Text = Query.PickupTime.ToString("g");
            if (Query.Car.BaseKm == 0)
            {
                CarFare.Text = "₹ " + Query.Car.BaseFare + " Fixed Fare";
                CarExtraFare.Text = string.Empty;
            }
            else
            {
                CarFare.Text = "₹ " + Query.Car.BaseFare + " for " + Query.Car.BaseKm + " km";
                CarExtraFare.Text = "₹ " + Query.Car.ExtraKmFare + " per km after that";
            }

            if (((App)App.Current).isPickNow == true)
            {
                TimeBlock.Text = "Earliest Available";
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (LoadinGrid.Visibility == Visibility.Visible)
            {
                e.Cancel = true;
            }
            base.OnBackKeyPress(e);
        }



        private void NameBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EmailBox.Focus();
            }
        }



        private void EmailBox_OnKeyDown(object sender, KeyEventArgs e)
        {


            if (e.Key == Key.Enter)
            {
                CouponBox.Focus();
            }
        }
        private void CouponBox_OnKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                this.Focus();
                ProceedBtn_Click(null, null);
            }
        }
    }
}