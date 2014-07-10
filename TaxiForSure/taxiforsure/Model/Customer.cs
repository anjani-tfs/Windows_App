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
    }
}
