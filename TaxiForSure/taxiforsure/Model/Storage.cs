using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Windows;

namespace TaxiforSure.Model
{
    public static class Storage
    {
        private static IsolatedStorageSettings isoSettings = IsolatedStorageSettings.ApplicationSettings;
        public static bool IsGpsAllowed
        {
            get
            {
                if (isoSettings.Contains("IsGpsAllowed"))
                    return (bool)isoSettings["IsGpsAllowed"];
                else
                    return false;
            }
            set
            {
                isoSettings["IsGpsAllowed"] = value;
            }
        }
        public static bool IsPushAllowed
        {
            get
            {
                if (isoSettings.Contains("IsPushAllowed"))
                    return (bool)isoSettings["IsPushAllowed"];
                else
                    return true;
            }
            set
            {
                isoSettings["IsPushAllowed"] = value;
            }
        }
        public static bool IsFirstLogin
        {
            get
            {
                if (isoSettings.Contains("IsFirstLogin"))
                    return false;
                else
                    return true;
            }
            set
            {
                isoSettings["IsFirstLogin"] = value;
            }
        }
        public static String LastBookingId
        {
            get
            {
                if (isoSettings.Contains("LastBookingId"))
                    return (String)isoSettings["LastBookingId"];
                else
                    return "";
            }
            set
            {
                isoSettings["LastBookingId"] = value;
            }
        }
        public static LocationInfo LastLocation
        {
            get
            {
                if (isoSettings.Contains("LastLocation"))
                    return (LocationInfo)isoSettings["LastLocation"];
                else
                    return null;
            }
            set
            {
                isoSettings["LocationInfo"] = value;
            }
        }

        public static List<BookingData> PastBookingIds
        {
            get
            {
                if (isoSettings.Contains("PastBookingIds"))
                    return (List<BookingData>)isoSettings["PastBookingIds"];
                else
                    return null;
            }
            set
            {
                isoSettings["PastBookingIds"] = value;
            }
        }
        public static List<BookingData> CurrentBookingIds
        {
            get
            {
                if (isoSettings.Contains("CurrentBookingIds"))
                    return (List<BookingData>)isoSettings["CurrentBookingIds"];
                else
                    return null;
            }
            set
            {
                isoSettings["CurrentBookingIds"] = value;
            }
        }

        public static String City
        {
            get
            {
                if (!isoSettings.Contains("City")) return "Bangalore";
                else
                {
                    return (String)isoSettings["City"];
                }
            }
            set { isoSettings["City"] = value; }

        }

        public static bool AskForGps()
        {
            

                if (!IsGpsAllowed)
                {
                    
                    var result =
                        MessageBox.Show(
                            "This application requires GPS for best results. To enable gps select ok",
                            "TaxiForSure - Allow Gps", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        Storage.IsGpsAllowed = true;
                        return true;
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            
            
        }
    }
}

