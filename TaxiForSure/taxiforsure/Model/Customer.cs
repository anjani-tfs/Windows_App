using System;
using System.IO.IsolatedStorage;

namespace TaxiforSure.Model
{
    public static class Customer
    {
        //TODO Isolated Storage Integration
        private static IsolatedStorageSettings isoSettings = IsolatedStorageSettings.ApplicationSettings;
        public static String Name
        {
            get
            {
                if (isoSettings.Contains("CutomerName")) return (String)isoSettings["CutomerName"];
                else
                    return "";
            }
            set { isoSettings["CutomerName"] = value; }
        }

        public static long Number
        {
            get
            {
                if (isoSettings.Contains("CutomerNumber")) return (long)isoSettings["CutomerNumber"];
                else
                    return 0;
            }
            set { isoSettings["CutomerNumber"] = value; }
        }

        public static String Email
        {
            get
            {
                if (isoSettings.Contains("CutomerEmail")) return (String)isoSettings["CutomerEmail"];
                else
                    return "";
            }
            set { isoSettings["CutomerEmail"] = value; }

        }

        public static String BookingId
        {
            get
            {
                if (isoSettings.Contains("BookingId")) return (String)isoSettings["BookingId"];
                else
                    return "";
            }
            set { isoSettings["BookingId"] = value; }

        }

        public static String UserId
        {
            get
            {
                if (isoSettings.Contains("UserId")) return (String)isoSettings["UserId"];
                else
                    return "";
            }
            set { isoSettings["UserId"] = value; }

        }
    }

    public static class SignInCustomer
    {
        //TODO Isolated Storage Integration
        private static IsolatedStorageSettings isoSettings = IsolatedStorageSettings.ApplicationSettings;
        public static String Name
        {
            get
            {
                if (isoSettings.Contains("SignInCutomerName")) return (String)isoSettings["SignInCutomerName"];
                else
                    return "";
            }
            set { isoSettings["SignInCutomerName"] = value; }
        }

        public static long Number
        {
            get
            {
                if (isoSettings.Contains("SignInCutomerNumber")) return (long)isoSettings["SignInCutomerNumber"];
                else
                    return 0;
            }
            set { isoSettings["SignInCutomerNumber"] = value; }
        }

        public static String Email
        {
            get
            {
                if (isoSettings.Contains("SignInCutomerEmail")) return (String)isoSettings["SignInCutomerEmail"];
                else
                    return "";
            }
            set { isoSettings["SignInCutomerEmail"] = value; }

        }

        public static String BookingId
        {
            get
            {
                if (isoSettings.Contains("SignInBookingId")) return (String)isoSettings["SignInBookingId"];
                else
                    return "";
            }
            set { isoSettings["SignInBookingId"] = value; }

        }

        public static String UserId
        {
            get
            {
                if (isoSettings.Contains("SignInUserId")) return (String)isoSettings["SignInUserId"];
                else
                    return "";
            }
            set { isoSettings["SignInUserId"] = value; }

        }
    }
}
