using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Windows;
using Newtonsoft.Json;

namespace TaxiforSure.Model
{
    public static class Storage
    {
        private static IsolatedStorageSettings isoSettings = IsolatedStorageSettings.ApplicationSettings;


        public static double radiusOfAirportForCity(string city)
        {
            Dictionary<string, string> airportDictForCity = Storage.airportDictForCity(city);
            double radius = double.Parse((string)airportDictForCity["radius"]);
            return radius;


        }
        
        public static double latitudeOfAirportForCity(string city)
        {

            Dictionary<string, string> airportDictForCity = Storage.airportDictForCity(city);
            double lat = double.Parse((string)airportDictForCity["latitude"]);
            return lat;

        }
        
        public static double longitudeOfAirportForCity(string city)
        {


            Dictionary<string, string> airportDictForCity = Storage.airportDictForCity(city);
            double longitude = double.Parse((string)airportDictForCity["longitude"]);
            return longitude;
        }

        public static string airportNameForCity(string city)
        {


            Dictionary<string, string> airportDictForCity = Storage.airportDictForCity(city);
            string airportName = (string)airportDictForCity["name"];
            return airportName;
        }

        
        public static Dictionary<string, string> airportDictForCity(string city)
        {

            Dictionary<string, List<Dictionary<string, string>>> airportDict = Storage.airportDictionary;

            List<Dictionary<string, string>> airportDictForCity = (List<Dictionary<string, string>>)airportDict[city];

            return airportDictForCity[0];
            //string airportDictionaryString = (string)airportDict[city].ToString();
            //List<Dictionary<string, string>> airportDictionaryDict = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(airportDictionaryString);
            

            /*
            foreach (string key in citySpecificDictionary.Keys)
            {
                string value = (string)citySpecificDictionary[key];
                returnDict.Add(key, value);
            }
             * */


        }



        public static Dictionary<string, List<Dictionary<string, string>>> airportDictionary
        {

            
            get
            {

                if (isoSettings.Contains("airportDictionary"))
                    return (Dictionary<string, List<Dictionary<string, string>>>)isoSettings["airportDictionary"];
                else
                    return null;

            }

            
            set
            {
               
                isoSettings["airportDictionary"] = value;
            }    
          
        } 
        
        
        public static List<string> cityList
        {

            get
            {
                List<string> l = new List<string>();
                if (isoSettings.Contains("cityList"))
                    l = (List<string>)isoSettings["cityList"];
                else
                    return l;

                return l;
            }
            
            set
            {
                isoSettings["cityList"] = value;
            }
             
        }
        
         

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

