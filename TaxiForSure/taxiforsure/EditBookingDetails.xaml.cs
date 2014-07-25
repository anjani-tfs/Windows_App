using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TaxiforSure.Model;

namespace TaxiforSure
{
    public partial class EditBookingDetails : PhoneApplicationPage
    {
        public EditBookingDetails()
        {
            InitializeComponent();
            
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (Storage.IsLogin)
            {
                for (int i = 0; i < Storage.SignInPastBookingIds.Count; i++)
                {
                    if (Storage.SignInPastBookingIds[i].BookingId == SignInCustomer.BookingId)
                    {
                        txtname.Text = SignInCustomer.Name;
                        txtEmail.Text = SignInCustomer.Email;
                        txtPhoneno.Text = "Phone no : " + SignInCustomer.Number.ToString();
                        txtPickAddress.Text = "Pickup adress : " + Storage.SignInPastBookingIds[i].From;
                        txtDropAddress.Text = "Drop address : " + Storage.SignInPastBookingIds[i].To;
                        txtFare.Text = "Fare : " + Storage.SignInPastBookingIds[i].Fare;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Storage.PastBookingIds.Count; i++)
                {
                    if (Storage.PastBookingIds[i].BookingId == Customer.BookingId)
                    {
                        txtname.Text = Customer.Name;
                        txtEmail.Text = Customer.Email;
                        txtPhoneno.Text = "Phone no : " + Customer.Number.ToString();
                        txtPickAddress.Text = "Pickup adress : " + Storage.PastBookingIds[i].From;
                        txtDropAddress.Text = "Drop address : " + Storage.PastBookingIds[i].To;
                        txtFare.Text = "Fare : " + Storage.PastBookingIds[i].Fare;
                    }
                }
            }
        }

        async void CancelBtn_Click(object sender, EventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        async void ProceedBtn_Click(object sender, EventArgs e)
        {
        }
    }
}