using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using TaxiforSure.Model;

namespace TaxiforSure
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (Storage.IsGpsAllowed)
            {
                LocationSwitch.IsChecked = true;
            }
            if (Storage.IsPushAllowed)
            {
                PushSwitch.IsChecked = true;
            }
        }

        private void LocationSwitch_OnChecked(object sender, RoutedEventArgs e)
        {
            Storage.IsGpsAllowed = true;
        }

        private void PushSwitch_OnChecked(object sender, RoutedEventArgs e)
        {
            Storage.IsPushAllowed = true;
        }

        private void PushSwitch_OnUnchecked(object sender, RoutedEventArgs e)
        {
            Storage.IsPushAllowed = false;
        }

        private void LocationSwitch_OnUnchecked(object sender, RoutedEventArgs e)
        {
            Storage.IsGpsAllowed = false;
        }

        private void Privacy_Click(object sender, RoutedEventArgs e)
        {
            var w = new WebBrowserTask();
            w.Uri = new Uri("http://www.taxiforsure.com/privacy/", UriKind.Absolute);
            w.Show();
        }
    }
}