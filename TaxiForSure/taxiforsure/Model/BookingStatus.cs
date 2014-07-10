using System;

namespace TaxiforSure.Model
{
    public class BookingStatus
    {
        public String BookingId { get; set; }
        public String Message { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsWaiting { get; set; }
        public bool CallbackSuccess { get; set; }
        public BookingStatus(string id, string message, bool isconfirmed, bool iswaiting)
        {
            BookingId = id;
            Message = message;
            IsConfirmed = isconfirmed;
            IsWaiting = iswaiting;
            CallbackSuccess = false;
        }
        public BookingStatus()
        {
        }
    }
}
