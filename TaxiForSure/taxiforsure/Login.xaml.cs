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
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace TaxiforSure
{
    public partial class Login : PhoneApplicationPage
    {
        MyClient client = new MyClient();
        public Login()
        {
            InitializeComponent();

            if (((App)App.Current).GPSOnlyOnce == true)
            {
                ((App)App.Current).GPSOnlyOnce = false;
                getGPSActivatedForPrefil();
            }
        }

        async private void getGPSActivatedForPrefil()
        {
            if (Storage.AskForGps())
            {
                try
                {

                    //var c = new MyClient();
                   // await checkLocation(c, await c.ReverseGeoCode(await (App.Current as App).GetLocation()));
                }
                catch (Exception ex)
                {
                   // Debug.WriteLine(ex.Message);
                }
            }

        }

        async void ProceedBtn_Click(object sender, EventArgs e)
        {
            this.Focus();
            LoadinGrid.Visibility = Visibility.Visible;

            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("Please enter valid username and password.", "TaxiForSure", MessageBoxButton.OK);
            }
            else
            {
                if (!Regex.IsMatch(txtUsername.Text, @"^(?![012])(\d{10})$"))
                {
                    MessageBox.Show("Enter a valid 10 digit number.", "TaxiForSure", MessageBoxButton.OK);
                    LoadinGrid.Visibility = Visibility.Collapsed;
                    return;
                }
                string AppVersion = "5.2";
                var logDetails = await client.Login(txtUsername.Text, txtPassword.Password, AppVersion);
                if (logDetails != null)
                {
                    Storage.IsLogin = true;
                    Storage.UserName = txtUsername.Text;
                    txtUsername.Text = string.Empty;
                    txtPassword.Password = string.Empty;
                    if (NavigationService.CanGoBack)
                        NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("Please enter valid username and password.", "TaxiForSure", MessageBoxButton.OK);
                    txtUsername.Text = string.Empty;
                    txtPassword.Password = string.Empty;
                    txtPassword.Visibility = Visibility.Collapsed;
                    txtPaswordForHint.Visibility = Visibility.Visible;
                }
            }
            LoadinGrid.Visibility = Visibility.Collapsed;
        }

        private void txtEmailForHint_GotFocus(object sender, RoutedEventArgs e)
        {
            PhoneTextBox pass = (PhoneTextBox)sender;

            txtPaswordForHint.Visibility = Visibility.Collapsed;
            txtPassword.Visibility = Visibility.Visible;
            txtPassword.Focus();

        }

        private void txtEmailForHint_LostFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox pass = (PasswordBox)sender;

            if (string.IsNullOrEmpty(txtPassword.Password))
            {
                txtPaswordForHint.Visibility = Visibility.Visible;
                txtPassword.Visibility = Visibility.Collapsed;
            }
        }

        async void CancelBtn_Click(object sender, EventArgs e)
        {
            txtUsername.Text = string.Empty;
            txtPassword.Password = string.Empty;
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Registration.xaml",UriKind.RelativeOrAbsolute));
        }
    }
}