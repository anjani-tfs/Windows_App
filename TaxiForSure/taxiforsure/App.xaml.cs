﻿using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using TaxiforSure.Model;
using TaxiforSure.Resources;
using Windows.Devices.Geolocation;

namespace TaxiforSure
{
    public partial class App : Application
    {
        public string selectedCity;
        private GeoCoordinate MyLocation { get; set; }
        public static GeoCoordinate MySavedLocation { get; set; }
        public bool firstAskForGPS;
        public string pickupParam, tripTypeParam, phoneNumberParam, pickLatitudeParam, pickLongitudeParam, dropLatParam, dropLongParam;
        public bool isPickup;
        public bool isPickNow;
        public string pickUpParamForPickLater;
        public String BookingType = "p2p";
        public bool IsTargetAirport = false;
        public bool IsSourceAirport = false;
        public BookingData CancelBookingData = null;
        public CurrentPastBookingDetails SignInCancelBookingData = null;
        public ObservableCollection<Model.CarDetails> cars_at;
        public ObservableCollection<Model.CarDetails> cars_at_km;
        public ObservableCollection<Model.CarDetails> cars_from_at_fixed;
        public ObservableCollection<Model.CarDetails> cars_from_at_perKM;
        public string bookingDateSelected;
        public bool isCancelAllowed;
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }
        public bool dateChecker;
        public string bookLater_pickupDate;
        public bool GPSSelectCount;
        public bool IsRadiusInfoAvailable = false;
        public Airport ChennaiPort, BanglorePort, Delhi1D, Delhi3T;
        public bool GPSOnlyOnce; // this is the only correct variable used
        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions.
            UnhandledException += Application_UnhandledException;

            // Standard XAML initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Language display initialization
            InitializeLanguage();

            isPickup = true;
            dateChecker = true;
            isPickNow = true;
            bookLater_pickupDate = "";
            firstAskForGPS = true;
            cars_at = new ObservableCollection<Model.CarDetails>();
            cars_at_km = new ObservableCollection<Model.CarDetails>();
            cars_from_at_fixed = new ObservableCollection<Model.CarDetails>();
            cars_from_at_perKM = new ObservableCollection<Model.CarDetails>();
            selectedCity = "Bangalore";
            Color myTaxiForSureColor = new Color();
            myTaxiForSureColor.A = 255;
            myTaxiForSureColor.R = 255;
            myTaxiForSureColor.G = 216;
            myTaxiForSureColor.B = 0;
            GPSSelectCount = false;
            isCancelAllowed = true;
            Resources.Remove("PhoneAccentColor");
            Resources.Add("PhoneAccentColor", Colors.Green);
            ((SolidColorBrush)Resources["PhoneAccentBrush"]).Color = myTaxiForSureColor;

            bookingDateSelected = "";

            GPSOnlyOnce = true;

            // fallback values to airport if google APIs response
            
            

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters
                //    Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }
        public async Task<GeoCoordinate> GetLocation()
        {
            if (MyLocation != null)
                return MyLocation;
            var geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );
                MyLocation = new System.Device.Location.GeoCoordinate(geoposition.Coordinate.Latitude,
                    geoposition.Coordinate.Longitude);
                MySavedLocation = MyLocation;
                return MyLocation;
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the l master switch is off
                    //          StatusTextBlock.Text = "l  is disabled in phone settings.";

                }
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        async void fillAirport()
        {
            var response = await new HttpClient().GetStringAsync(" http://iospush.taxiforsure.com/api/consumer-app/config/?appVersion=4.0.0");
            // + bookingId);
            JObject data = JObject.Parse(response);
            if ("success".Equals((string)data["status"]))
            {
                App myApp = (App)App.Current;
                myApp.Delhi1D.Lat = double.Parse((string)data["response_data"]["AIRPORTS"]["Delhi"][0]["latitude"]);
                myApp.Delhi1D.Lng = double.Parse((string)data["response_data"]["AIRPORTS"]["Delhi"][0]["longitude"]);
                myApp.Delhi3T.Lat = double.Parse((string)data["response_data"]["AIRPORTS"]["Delhi"][1]["latitude"]);
                myApp.Delhi3T.Lng = double.Parse((string)data["response_data"]["AIRPORTS"]["Delhi"][1]["longitude"]);
                myApp.ChennaiPort.Lat = double.Parse((string)data["response_data"]["AIRPORTS"]["Chennai"]["latitude"]);
                myApp.ChennaiPort.Lng = double.Parse((string)data["response_data"]["AIRPORTS"]["Chennai"]["longitude"]);
                myApp.BanglorePort.Lat = double.Parse((string)data["response_data"]["AIRPORTS"]["Bangalore"]["latitude"]);
                myApp.BanglorePort.Lng = double.Parse((string)data["response_data"]["AIRPORTS"]["Bangalore"]["longitude"]);
                
            }
            


        }


        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            // Ensure that application state is restored appropriately
            if (e.IsApplicationInstancePreserved)
            {
                //    ApplicationDataStatus = "application instance preserved.";
                return;
            }
            //   if (PhoneApplicationService.Current.State.ContainsKey("ApplicationDataObject"))
            {
                // If it exists, assign the data to the application member variable.
                // = PhoneApplicationService.Current.State["ApplicationDataObject"] as string;
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            //    if (!string.IsNullOrEmpty(ApplicationDataObject))
            {
                // Store it in the State dictionary.
                //   PhoneApplicationService.Current.State["ApplicationDataObject"] = ApplicationDataObject;
            }
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            // Ensure that required application state is persisted here.
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += CheckForResetNavigation;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        #endregion

        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        //
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }
    }
}