using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace TaxiforSure.Model
{

    // add selection highlight on location selection
    public class MyClient
    {
        HttpClient client;
        HttpClient client2;

        //TODO::change production url
        // String url = "http://qa.app.rtfs.in";
        String url = "http://app.aws.rtfs.in"; 
        // String url = "http://www.radiotaxiforsure.in";
        //string url2 = "http://182.18.181.42:9999";
         string url2 = "http://54.255.57.96:8881";
         //string url2 = "http://iospush.taxiforsure.com";

        public MyClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client2 = new HttpClient();
            client2.BaseAddress = new Uri(url2);
        }

        public void CancelPending()
        {
            client.CancelPendingRequests();
        }
        public void CoolFunc(object obj)
        {
            // obj.GetType().GetProperties();  
        }

        public async Task<bool> IsInAirportVicinity(double lat2, double lng2, String city)
        {
            var myApp = App.Current as App;
            if (myApp.IsRadiusInfoAvailable)
            {

            }
            else
            {
                try
                {
                    //TODO:: change to production url
                    //var response = await new HttpClient().GetStringAsync(" http://iospush.taxiforsure.com/api/consumer-app/config/?appVersion=4.0.0");
                    
                    
                    var response = await new HttpClient().GetStringAsync("http://54.255.57.96:8881/api/consumer-app/config/?appVersion=4.0.0");
        
                    // + bookingId);
                    JObject data = JObject.Parse(response);
                    if ("success".Equals((string)data["status"]))
                    {
                        var obj = data["response_data"]["AIRPORTS"];
                        //Chennai  
                        //myApp.ChennaiPort = getAirportDetails(obj, "Chennai", 0);
                        //Bangalore
                        //myApp.BanglorePort = getAirportDetails(obj, "Bangalore", 0);
                        //Delhi Terminal 1D
                        myApp.Delhi1D = getAirportDetails(obj, "Delhi", 0);
                        //Terminal 3
                        myApp.Delhi3T = getAirportDetails(obj, "Delhi", 1);

                        //myApp.HyderabadPort = getAirportDetails(obj, "Hyderabad", 0);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            

             
            if (city.ToLower() == "delhi")
            {
                return IsInRadius(myApp.Delhi1D.Lat, myApp.Delhi1D.Lng, lat2, lng2, myApp.Delhi1D.radius) ||
                       IsInRadius(myApp.Delhi3T.Lat, myApp.Delhi3T.Lng, lat2, lng2, myApp.Delhi3T.radius);
            }
            else{

                 return IsInRadius(Storage.latitudeOfAirportForCity(city), Storage.longitudeOfAirportForCity(city), lat2, lng2, Storage.radiusOfAirportForCity(city));

                }
                    

        }

        public bool IsInRadius(double lat1, double lng1, double lat2, double lng2, double radius)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(lat2 - lat1); // deg2rad below
            var dLon = deg2rad(lng2 - lng1);
            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
                ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            radius = radius / 1000;//radius in km
            if (radius < d) return false;
            else return true;
        }
        private Airport getAirportDetails(JToken obj, String city, int index)
        {
            return new Airport
              {
                  Name = (String)obj[city][index]["name"],
                  Lat = (double)obj[city][index]["latitude"],
                  Lng = (double)obj[city][index]["longitude"],
                  radius = (int)obj[city][index]["radius"]
              };
        }
        double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }

        public async Task<String> IsinCity(double lat, double lng)
        {
            String City = "";
            //API: /api/consumer-app/is_in_city/ 
            //Method: POST
            //Params: latitude=12.966700&longitude=77.566700&appVersion=2.8.4
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("latitude", lat.ToString()));
            postData.Add(new KeyValuePair<string, string>("longitude", lng.ToString()));
            postData.Add(new KeyValuePair<string, string>("appVersion", "2.8.4"));
            try
            {
                using (var content = new FormUrlEncodedContent(postData))
                {
                    var response = await client.PostAsync("/api/consumer-app/is_in_city/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        String a = await response.Content.ReadAsStringAsync();
                        JObject data = JObject.Parse(a);
                        if ("success".Equals((string)data["status"]))
                        {
                            bool isServicable = (bool)data["response_data"]["value"];
                            if (isServicable)
                            {
                                City = (String)data["response_data"]["city"];
                            }
                        }
                        return City;
                    }
                    else
                    {
                        return City;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BookingData>> BookingStatus(List<BookingData> ids)
        {
            //API: /api/get-bookings-status/
            //Method: POST
            //Params: appVersion, source, bookings_list
            var postData = new List<KeyValuePair<string, string>>();
            foreach (var id in ids)
            {
                postData.Add(new KeyValuePair<string, string>("bookings_list", id.BookingId));
            }
            postData.Add(new KeyValuePair<string, string>("source", "windows"));
            postData.Add(new KeyValuePair<string, string>("appVersion", "3.0.1"));
            try
            {
                using (var content = new FormUrlEncodedContent(postData))
                {
                    var response = await client.PostAsync("/api/get-bookings-status/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        String a = await response.Content.ReadAsStringAsync();
                        JObject data = JObject.Parse(a);
                        var resp = new List<BookingData>();
                        if ("success".Equals((string)data["status"]))
                        {
                            foreach (var id in ids)
                            {
                                String result = data["response_data"][id.BookingId].ToString().ToLower();
                                if (result.Equals("canceled") || result.Equals("completed"))
                                {
                                    if (result.Equals("canceled"))
                                    {
                                        result = "Canceled";
                                    }
                                    else
                                    {
                                        result = "Completed";
                                    }

                                    id.BookingStatus = result;
                                    resp.Add(id);
                                }
                            }
                        }
                        return resp;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> CancelBooking(String id, String reason)
        {
            //API: /api/get-bookings-status/
            //Method: POST
            //Params: appVersion, source, bookings_list
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("booking_id", id));
            postData.Add(new KeyValuePair<string, string>("cancellation_reason", reason));
            postData.Add(new KeyValuePair<string, string>("user_id ", "null"));
            postData.Add(new KeyValuePair<string, string>("appVersion", "3.0.1"));
            try
            {
                using (var content = new FormUrlEncodedContent(postData))
                {
                    var response = await client.PostAsync("/api/customer/cancel-taxi/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        String a = await response.Content.ReadAsStringAsync();
                        JObject data = JObject.Parse(a);
                        var resp = new List<BookingData>();
                        if ("success".Equals((string)data["status"]))
                        {
                            //    String result = data["response_data"]["status"].ToString();
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ObservableCollection<CarDetails>> Fare()
        {
            //assuming this to be pickNow
            //API: /api/consumer-app/fares-new/
            //&latitude=12.966700&longitude=77.566700&drop_latitude=12.97866&drop_longitude=77.5724
            string cityParam, pickupParam, tripTypeParam, phoneNumberParam, pickLatitudeParam, pickLongitudeParam, dropLatParam, dropLongParam;
            App myApp = (App)App.Current;
            cityParam = Storage.City;
            if (((App)App.Current).isPickNow == true)
                tripTypeParam = "pn";
            else
                tripTypeParam = "pl";
            phoneNumberParam = "";

            double long_pick, long_drop, lat_pick, lat_drop;
            try
            {
                long_pick = double.Parse(myApp.pickLongitudeParam);
                long_drop = double.Parse(myApp.dropLongParam);
                lat_pick = double.Parse(myApp.pickLatitudeParam);
                lat_drop = double.Parse(myApp.dropLatParam);

            }
            catch
            { }

            if (myApp.pickLatitudeParam.Length > 10)
            {
                pickLatitudeParam = myApp.pickLatitudeParam.Substring(0, 10);   //string.Format("{00:0000000}", myApp.pickLatitudeParam);// myApp.pickLatitudeParam.ToString('0000000');
            }
            else
            {
                pickLatitudeParam = myApp.pickLatitudeParam;
            }

            if (myApp.pickLongitudeParam.Length > 10)
            {
                pickLongitudeParam = myApp.pickLongitudeParam.Substring(0, 10);  // string.Format("{00:0000000}", myApp.pickLongitudeParam);
            }
            else
            {
                pickLongitudeParam = myApp.pickLongitudeParam;
            }

            if (myApp.dropLatParam.Length > 10)
            {
                dropLatParam = myApp.dropLatParam.Substring(0, 10);  // string.Format("{00:0000000}", myApp.pickLongitudeParam);
            }
            else
            {
                dropLatParam = myApp.dropLatParam;
            }

            if (myApp.dropLongParam.Length > 10)
            {
                dropLongParam = myApp.dropLongParam.Substring(0, 10);  // string.Format("{00:0000000}", myApp.pickLongitudeParam);
            }
            else
            {
                dropLongParam = myApp.dropLongParam;
            }

           // dropLatParam = myApp.dropLatParam;
            //dropLongParam = myApp.dropLongParam;

            pickupParam = myApp.pickUpParamForPickLater;
            // enter pickup time only


            String param = String.Format("pickup_time={0}&trip_type={1}&customer_number{2}&city={3}&latitude={4}&longitude={5}&drop_latitude={6}&drop_longitude={7}&booking_type={8}"
                , pickupParam, tripTypeParam, phoneNumberParam, cityParam, pickLatitudeParam, pickLongitudeParam, dropLatParam, dropLongParam, (((App)App.Current).IsSourceAirport || ((App)App.Current).IsTargetAirport) ? ((Query.Pickup.City == "Bangalore") ? "at" : "at-km") : "p2p");
            try
            {
                String result = await client.GetStringAsync("/api/consumer-app/fares-new/?appVersion=2.8.4&" + param);
                JObject data = JObject.Parse(result);
                if ("success".Equals((string)data["status"]))
                {
                    var cars = new ObservableCollection<CarDetails>();
                    if ((App.Current as App).BookingType == "p2p")
                    {
                        cars = GetKmCarDetail(data["response_data"]["p2p"]);
                        /*added below code so that when it's p2p , airport fares are cleared anjani */ 
                        myApp.cars_at.Clear();
                        myApp.cars_at_km.Clear();
                        myApp.cars_from_at_fixed.Clear();
                        myApp.cars_from_at_perKM.Clear();
                        /*   end of bug fix      */
                    }
                    else
                    {
                        JObject data_at, data_at_km, data_from_at_km_fares, data_from_at_flat_fares;




                        try
                        {
                            myApp.cars_at = GetFixedCarDetail(data["response_data"]["at"]);
                            myApp.cars_at_km = GetKmCarDetail(data["response_data"]["at-km"]);
                            myApp.cars_from_at_fixed = GetFixedCarDetail(data["response_data"]["from_at_flat_fares"]);
                            myApp.cars_from_at_perKM = GetKmCarDetail(data["response_data"]["from_at_km_fares"]);
                            return null;
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    return cars;
                }
                return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong, please try again later.", "TaxiForSure", MessageBoxButton.OK);
               // MessageBox.Show(ex.Message);
                return null;
            }

        }

        private ObservableCollection<CarDetails> GetFixedCarDetail(JToken tempObject)
        {
            ObservableCollection<CarDetails> cars = new ObservableCollection<CarDetails>();

            try
            {
                foreach (var car in tempObject)
                {
                    cars.Add(
                    new CarDetails
                    {
                        CarName = (String)car["car_name"],
                        CarModel = (String)car["car_model"],
                        CarType = (String)car["car_type"],
                        BaseFare = (double)car["fare"],
                        BaseKm = 0,
                        FareId = (int)car["fare_id"],
                        HasAc = (bool)car["has_ac"]
                    });
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(exp.Message);
            }
            return cars;
        }
        private ObservableCollection<CarDetails> GetKmCarDetail(JToken tempObject)
        {
            ObservableCollection<CarDetails> cars = new ObservableCollection<CarDetails>();

            try
            {
                foreach (var car in tempObject)
                {
                    cars.Add(
                    new CarDetails
                    {
                        CarName = (String)car["car_name"],
                        CarModel = (String)car["car_model"],
                        CarType = (String)car["car_type"],
                        BaseFare = (double)car["base_fare"],
                        BaseKm = (int)car["base_km"],
                        ExtraKmFare = (double)car["extra_km_fare"],
                        FareId = (int)car["fare_id"],
                        HasAc = (bool)car["has_ac"]
                    });
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(exp.Message);
            }
            return cars;
        }


        private async Task<BookingStatus> PickLater()
        {
            //API: /api/consumer-app/book-for-later/
            //Method: POST
            //Params(example):
            var postData = GetPostDataValue();
            try
            {
                var status = new BookingStatus();

                string temp = (Query.PickupTime.ToString());
                temp = temp.Substring(0, temp.IndexOf(' '));
                // mm/dd/yyyy

                //string[] myDates = temp.Split('/');
                //string finalDate = myDates[1] + "/" + myDates[0] + "/" + myDates[2];
                //MessageBox.Show(((App)App.Current).bookingDateSelected);
                postData.Add(new KeyValuePair<string, string>("pickup_date", ((App)App.Current).bookingDateSelected));

                //foreach (var x in postData)
                //{
                //    MessageBox.Show(x.Key + " " + x.Value);
                //}

                using (var content = new FormUrlEncodedContent(postData))
                {


                    var resp = await client.PostAsync("/api/consumer-app/book-for-later/", content);

                    if (resp.IsSuccessStatusCode)
                    {
                        String response = await resp.Content.ReadAsStringAsync();

                        //MessageBox.Show(response);

                        JObject data = JObject.Parse(response);
                        if ("success".Equals((string)data["status"]))
                        {
                            status.BookingId = (String)data["response_data"]["booking_id"];
                            status.Message = (String)data["response_data"]["status_message"];
                            status.IsConfirmed = (bool)data["response_data"]["booking_confirmed"];
                            status.IsWaiting = (bool)data["response_data"]["wait"];
                            return status;
                        }
                        return null;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message);
                return null;
            }
        }
        private async Task<BookingStatus> PickNow()
        {
            //API: /api/consumer-app/book-now/
            //Method: POST
            var postData = GetPostDataValue();
            try
            {
                var status = new BookingStatus();


                using (var content = new FormUrlEncodedContent(postData))
                {
                    var resp = await client.PostAsync("/api/consumer-app/book-now/", content);
                    if (resp.IsSuccessStatusCode)
                    {
                        String response = await resp.Content.ReadAsStringAsync();
                        JObject data = JObject.Parse(response);
                        if ("success".Equals((string)data["status"]))
                        {
                            status.BookingId = (String)data["response_data"]["booking_id"];
                            status.Message = (String)data["response_data"]["status_message"];
                            status.IsConfirmed = (bool)data["response_data"]["booking_confirmed"];
                            status.IsWaiting = (bool)data["response_data"]["wait"];
                            return status;
                        }
                        return null;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        private List<KeyValuePair<string, string>> GetPostDataValue()
        {
            var postData = new List<KeyValuePair<string, string>>();

            if (string.IsNullOrWhiteSpace(Customer.Name))
                postData.Add(new KeyValuePair<string, string>("customer_name", "Guest"));
            else
                postData.Add(new KeyValuePair<string, string>("customer_name", Customer.Name));
            if (string.IsNullOrWhiteSpace(Customer.Email))
            {
                postData.Add(new KeyValuePair<string, string>("customer_email", "no.email@example.com"));
            }
            else
                postData.Add(new KeyValuePair<string, string>("customer_email", Customer.Email));

            postData.Add(new KeyValuePair<string, string>("customer_number", Customer.Number.ToString()));
            postData.Add(new KeyValuePair<string, string>("coupon_code", Query.CouponCode + ""));
            postData.Add(new KeyValuePair<string, string>("pickup_address", Query.Pickup.Name));
            postData.Add(new KeyValuePair<string, string>("pickup_area", Query.Pickup.Area));
            postData.Add(new KeyValuePair<string, string>("city", Query.Pickup.City));
            postData.Add(new KeyValuePair<string, string>("landmark", Query.Pickup.Landmark + ""));
            postData.Add(new KeyValuePair<string, string>("pickup_latitude", Math.Round(Query.Pickup.Location.Latitude, 5).ToString()));
            postData.Add(new KeyValuePair<string, string>("pickup_longitude", Math.Round(Query.Pickup.Location.Longitude, 5).ToString()));
            postData.Add(new KeyValuePair<string, string>("drop_address", Query.Drop.Name));
            postData.Add(new KeyValuePair<string, string>("drop_area", Query.Drop.Area));
            postData.Add(new KeyValuePair<string, string>("drop_latitude", Math.Round(Query.Drop.Location.Latitude, 5).ToString()));
            postData.Add(new KeyValuePair<string, string>("drop_longitude", Math.Round(Query.Drop.Location.Longitude, 5).ToString()));
            postData.Add(new KeyValuePair<string, string>("car_type", Query.Car.CarType));
            postData.Add(new KeyValuePair<string, string>("extra_km_fare", Query.Car.ExtraKmFare.ToString()));
            if ((App.Current as App).BookingType == "p2p")
            {
                postData.Add(new KeyValuePair<string, string>("base_fare", Query.Car.BaseFare.ToString()));
                postData.Add(new KeyValuePair<string, string>("fare", "0"));
                postData.Add(new KeyValuePair<string, string>("fare_type", "2"));
            }
            else
            {
                //if extrakm is 0 then its fixed fare
                if (Query.Car.ExtraKmFare < 0.5)
                {
                    postData.Add(new KeyValuePair<string, string>("base_fare", "0"));
                    postData.Add(new KeyValuePair<string, string>("fare", Query.Car.BaseFare.ToString()));
                    postData.Add(new KeyValuePair<string, string>("fare_type", "1"));
                }
                //else its airport and km
                else
                {
                    postData.Add(new KeyValuePair<string, string>("fare", "0"));
                    postData.Add(new KeyValuePair<string, string>("base_fare", Query.Car.BaseFare.ToString()));
                    postData.Add(new KeyValuePair<string, string>("fare_type", "2"));
                }
            }
            postData.Add(new KeyValuePair<string, string>("base_km", Query.Car.BaseKm.ToString()));

            if (!Query.PickNow)
            {
                //date in format 21/12/2013
                //time in format 21:15
                string dateInForm = Query.PickupTime.ToString("dd/mm/yyyy");

                //postData.Add(new KeyValuePair<string, string>("pickup_date", dateInForm));

                postData.Add(new KeyValuePair<string, string>("pickup_time", Query.PickupTime.ToString("HH:mm")));
            }
            if (((App)App.Current).IsSourceAirport || ((App)App.Current).IsTargetAirport)
            {
                if (Query.Pickup.City == "Bangalore")
                    postData.Add(new KeyValuePair<string, string>("booking_type", "at"));
                else
                    postData.Add(new KeyValuePair<string, string>("booking_type", "at-km"));

                if (((App)App.Current).IsSourceAirport)
                    postData.Add(new KeyValuePair<string, string>("direction", "from"));
                else
                    postData.Add(new KeyValuePair<string, string>("direction", "to"));

            }
            else
            {
                postData.Add(new KeyValuePair<string, string>("booking_type", "p2p"));
            }

            postData.Add(new KeyValuePair<string, string>("appVersion", "2.8.4"));
            postData.Add(new KeyValuePair<string, string>("source", "windows"));
            return postData;
        }

        public async Task<BookingStatus> BookTaxi()
        {
            try
            {
                BookingStatus status;
                if (Query.PickNow)
                {
                    status = await PickNow();
                }
                else
                {
                    status = await PickLater();
                }
                if (status == null || status.IsWaiting)
                {
                    bool IsConfirmed = await RetryBookingStatus(status);
                    if (!IsConfirmed)
                    {
                        status.CallbackSuccess = await CallBack(status.BookingId);
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<bool> RetryBookingStatus(BookingStatus status)
        {
            for (int i = 0; i < 20; i++)
            {
                try
                {
                    //TFS-AT-C831075
                    var response = await client.GetStringAsync("/api/consumer-app/booking-status/?appVersion=3.0.1&booking_id=" + status.BookingId);
                    // + bookingId);
                    JObject data = JObject.Parse(response);
                    if ("success".Equals((string)data["status"]))
                    {
                        var obj = data["response_data"];
                        status.IsWaiting = (bool)obj["wait"];
                        if (!status.IsWaiting)
                        {
                            status.IsConfirmed = (bool)obj["booking_confirmed"];
                            return status.IsConfirmed;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                await Task.Delay(TimeSpan.FromSeconds(6));
            }
            return false;
        }

        public async Task<TaxiStatus> TrackTaxi(String bookingId)
        {
            //GET
            //  /api/customer/track-taxi/
            //Params: booking_id=TFS-PP-C770063  TFS-AT-C831075
            try
            {
                var response = await client.GetStringAsync("/api/customer/track-taxi/?booking_id=" + bookingId);
                JObject data = JObject.Parse(response);

                if ("success".Equals((string)data["status"]))
                {
                    TaxiStatus ts = new TaxiStatus();
                    var obj = data["response_data"];
                    GeoCoordinate driverCoordinate = null;
                    int statusid = (int)obj["booking_status"];
                    if (statusid != 1)
                    {
                        try
                        {
                            ts.Location = new GeoCoordinate((double)obj["latitude"], (double)obj["longitude"]);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("driver coordinates not found");
                        }
                    }
                    ts.StatusId = (int)obj["booking_status"];
                    ts.Message = (String)obj["status_message"];
                    ts.Status = (String)obj["status"];
                    ts.Fare = (String)obj["booking_fare"];
                    ts.PickupLocation = new GeoCoordinate((double)obj["pickup_area_latitude"],
                        (double)obj["pickup_area_longitude"]);
                    return ts;
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> CallBack(String bookingId)
        {
            try
            {
                var response =
                    await client.GetStringAsync("api/consumer-app/callback/?appVersion=4.0.0&booking_id=" + bookingId);
                JObject data = JObject.Parse(response);
                if ("success".Equals((string)data["status"]))
                {
                    var obj = data["response_data"];
                    var statusMessage = (String)obj["status_message"];
                    if (statusMessage.Equals("Success"))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return false;
        }
        public async Task<GeoCoordinate> GeoCode(String address)
        {
            try
            {
                var newClient = new HttpClient();
                var response = await newClient.GetStringAsync("http://maps.google.com/maps/api/geocode/json?sensor=false&address=" + address);
                JObject data = JObject.Parse(response);
                if ("OK".Equals((string)data["status"]))
                {
                    var results = data["results"][0]["geometry"]["l"];
                    double lat = (double)results["lat"];
                    double lng = (double)results["lng"];
                    return new GeoCoordinate(lat, lng);
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        //public String GetImageUrl(GeoCoordinate clientCoordinate, GeoCoordinate cabCoordinate)
        //{
        //    String cabParam = "";

        //    if (cabCoordinate != null)
        //        cabParam = "&markers=color:yellow%7Clabel:T%7C" + cabCoordinate.Latitude + "," + cabCoordinate.Longitude;
        //    try
        //    {
        //        return "http://maps.googleapis.com/maps/api/staticmap?sensor=false&size=450x450&maptype=roadmap&markers=color:blue%7Clabel:Y%7C" + clientCoordinate.Latitude + "," + clientCoordinate.Longitude + cabParam;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //        return "";
        //    }
        //}
        public async Task<DriverDetails> CallDriver(String bookingId)
        {
            try
            {
                var response = await client.GetStringAsync("/api/customer/call-driver/?appVersion=3.0.1&booking_id=" + bookingId);
                JObject data = JObject.Parse(response);
                if ("success".Equals((String)data["status"]))
                {
                    var driver = new DriverDetails();
                    var obj = data["response_data"];
                    driver.Name = (String)obj["driver_name"];
                    driver.Number = (String)obj["driver_number"];
                    driver.VehicleNumber = (String)obj["vehicle_number"];
                    driver.Message = (String)obj["status_message"];
                    driver.Status = (String)obj["status"];
                    driver.PickupLocation = new GeoCoordinate((double)obj["pickup_latitude"],
                      (double)obj["pickup_longitude"]);
                    return driver;
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<LocationInfo> ReverseGeoCode(GeoCoordinate lctn)
        {
            try
            {
                var l = new LocationInfo();
                var newClient = new HttpClient();
                var response = await newClient.GetStringAsync("http://maps.googleapis.com/maps/api/geocode/json?latlng=" + lctn.Latitude + "," + lctn.Longitude + "&sensor=true");
                JObject data = JObject.Parse(response);
                if ("OK".Equals((string)data["status"]))
                {
                    var address = (String)data["results"][0]["formatted_address"];
                    l.Name = address;
                    l.Area = address;
                    l.Location = lctn;
                    //l.Landmark;
                    return l;
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<LocationInfo> GeoCode(AutoCompleteResult selection)
        {
            try
            {
                var l = new LocationInfo();
                l.Name = selection.Description;
                var newClient = new HttpClient();
                var response = await newClient.GetStringAsync("https://maps.googleapis.com/maps/api/place/details/json?key=AIzaSyAzq8x5-mNRdBHrziXfgmu8Z7XpO16419Q&sensor=false&reference=" + selection.Reference);
                JObject data = JObject.Parse(response);
                if ("OK".Equals((string)data["status"]))
                {
                    var results = data["result"]["geometry"]["location"];
                    double lat = (double)results["lat"];
                    double lng = (double)results["lng"];
                    l.Area = (String)data["result"]["vicinity"];
                    l.Location = new GeoCoordinate(lat, lng);
                    //l.Landmark;
                    return l;
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<BookingInfo> BookingInfo(String pastBookingId)
        {
            try
            {
                var response = await client.GetStringAsync("/api/consumer-app/booking-info/?booking_id=" + pastBookingId+"&appVersion=3.0.2");
                JObject data = JObject.Parse(response);
                if ("success".Equals((String)data["status"]))
                {
                    var bookinginfo = new BookingInfo();
                    var obj = data["response_data"];
                    bookinginfo.Fare = (String)obj["fare"];
                    return bookinginfo;
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }


        public async Task<List<AutoCompleteResult>> GetAutoComplete(String text, GeoCoordinate lctn)
        {
            string response;
            string AuthKey = string.Empty;

            try
            {
                var newClient = new HttpClient();
                String location = "";
                string lat = string.Empty;
                string lng = string.Empty;

                if (lctn != null)
                {
                    lat = lctn.Latitude.ToString();
                    lng = lctn.Longitude.ToString();
                    location = "&l=" + lat + "%2c" + lng;
                }
                if (url == "http://qa.app.rtfs.in")
                {
                    AuthKey = "AIzaSyAzq8x5-mNRdBHrziXfgmu8Z7XpO16419Q";
                }
                else
                {
                    AuthKey = "AIzaSyDgeafiFnwok2xJZzOVcbw_dIkph46rB48";
                }
                response = await newClient.GetStringAsync(
                   // "https://maps.googleapis.com/maps/api/place/autocomplete/json?key=AIzaSyAzq8x5-mNRdBHrziXfgmu8Z7XpO16419Q&sensor=false&components=country:in&radius=80000&types=geocode&input=" + text + location);
                  // "https://maps.googleapis.com/maps/api/place/autocomplete/json?key=AIzaSyAzq8x5-mNRdBHrziXfgmu8Z7XpO16419Q&sensor=false&components=country:in&radius=5000&types=geocode&input=" + text + "&mPickupLatitude=" + lat + "&mPickupLongitude=" + lng + "");
                  "https://maps.googleapis.com/maps/api/place/autocomplete/json?location="+lat+","+lng+"&radius=5000&sensor=true&input="+text+"&key="+AuthKey+"&components=country:in");
                JObject data = JObject.Parse(response);

                if ("OK".Equals((string)data["status"]))
                {
                    var results = new List<AutoCompleteResult>();
                    //predictions- description,reference
                    foreach (var result in data["predictions"])
                    {
                        results.Add(new AutoCompleteResult
                        {
                            Description = (String)result["description"],
                            Reference = (String)result["reference"]
                        });
                    }
                    return results;
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}