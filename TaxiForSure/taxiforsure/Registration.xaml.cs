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
using System.Text.RegularExpressions;

namespace TaxiforSure
{
    public partial class Registration : PhoneApplicationPage
    {
        MyClient client = new MyClient();

        public Registration()
        {
            InitializeComponent();
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            txtname.Text = string.Empty;
            txtPhoneNo.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtConfirmPassword.Password = string.Empty;
            txtPassword.Password = string.Empty;
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        async void CancelBtn_Click(object sender, EventArgs e)
        {
            txtname.Text = string.Empty;
            txtPhoneNo.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtConfirmPassword.Password = string.Empty;
            txtPassword.Password = string.Empty;
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        async void ProceedBtn_Click(object sender, EventArgs e)
        {
            this.Focus();
            string appversion="5.2";
            LoadinGrid.Visibility = Visibility.Visible;
            if (!string.IsNullOrEmpty(txtname.Text.Trim()) && !string.IsNullOrEmpty(txtEmail.Text.Trim()) && !string.IsNullOrEmpty(txtPhoneNo.Text.Trim()) && !string.IsNullOrEmpty(txtPassword.Password.Trim()) && !string.IsNullOrEmpty(txtConfirmPassword.Password.Trim()))
            {
                if (txtPassword.Password != txtConfirmPassword.Password)
                {
                    MessageBox.Show("Your password and confirmation password do not match.", "TaxiForSure", MessageBoxButton.OK);
                    LoadinGrid.Visibility = Visibility.Collapsed;
                    return;
                }
                if (!Regex.IsMatch(txtPhoneNo.Text, @"^(?![012])(\d{10})$"))
                {
                    MessageBox.Show("Enter a valid 10 digit number.", "TaxiForSure", MessageBoxButton.OK);
                    LoadinGrid.Visibility = Visibility.Collapsed;
                    return;
                }
                if (!Regex.IsMatch(txtEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                    {
                        MessageBox.Show("Enter a valid email id.", "TaxiForSure", MessageBoxButton.OK);
                        LoadinGrid.Visibility = Visibility.Collapsed;
                        return;
                    }
                }

                var regDetails = await client.Registration(txtPhoneNo.Text.Trim(), txtname.Text.Trim(), txtEmail.Text.Trim(), txtPassword.Password.Trim(), "", "Google Search", appversion);
                if (regDetails != null)
                {
                    MessageBox.Show("Registered successfully.", "TaxiForSure", MessageBoxButton.OK);
    
                    if (NavigationService.CanGoBack)
                        NavigationService.GoBack();
                    txtname.Text = string.Empty;
                    txtPhoneNo.Text = string.Empty;
                    txtEmail.Text = string.Empty;
                    txtConfirmPassword.Password = string.Empty;
                    txtPassword.Password = string.Empty;
                }
            }
            else
            {
                MessageBox.Show("All fields are mendatory.", "TaxiForSure", MessageBoxButton.OK);
            }
            LoadinGrid.Visibility = Visibility.Visible;           
        }

        private void txtEmailForHint_GotFocus(object sender, RoutedEventArgs e)
        {
            PhoneTextBox pass = (PhoneTextBox)sender;
            if (pass.Name == "txtPasswordForHint")
            {
                txtPasswordForHint.Visibility = Visibility.Collapsed;
                txtPassword.Visibility = Visibility.Visible;
                txtPassword.Focus();
            }
            else
            {
                txtConfirmPasswordForHint.Visibility = Visibility.Collapsed;
                txtConfirmPassword.Visibility = Visibility.Visible;
                txtConfirmPassword.Focus();
            }
        }

        private void txtEmailForHint_LostFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox pass = (PasswordBox)sender;
            if (pass.Name == "txtPassword")
            {
                if (string.IsNullOrEmpty(txtPassword.Password))
                {
                    txtPasswordForHint.Visibility = Visibility.Visible;
                    txtPassword.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtConfirmPassword.Password))
                {
                    txtConfirmPasswordForHint.Visibility = Visibility.Visible;
                    txtConfirmPassword.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}