﻿#pragma checksum "C:\Users\Anjani-Serendipity\Documents\TaxiForSure\TaxiforSure\TrackMyTaxi.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6781926054CEC1BBA6983535F8063ABA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace TaxiforSure {
    
    
    public partial class TrackMyTaxi : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel WaitingBar;
        
        internal System.Windows.Controls.Primitives.Popup popup;
        
        internal System.Windows.Controls.TextBlock bookingidPopup;
        
        internal System.Windows.Controls.Button edit;
        
        internal System.Windows.Controls.Button receipt;
        
        internal Microsoft.Phone.Controls.PhoneTextBox txtEmail;
        
        internal System.Windows.Controls.Button receiptSend;
        
        internal System.Windows.Controls.Button receiptCancel;
        
        internal System.Windows.Controls.Button Cancel;
        
        internal System.Windows.Controls.Primitives.Popup popupCurrentBooking;
        
        internal System.Windows.Controls.TextBlock bookingidCurrentPopup;
        
        internal System.Windows.Controls.Button CancelBooking;
        
        internal System.Windows.Controls.Button Close;
        
        internal Microsoft.Phone.Controls.Pivot MainPivot;
        
        internal Microsoft.Phone.Controls.PivotItem Pivot2;
        
        internal System.Windows.Controls.TextBlock PastBooking;
        
        internal System.Windows.Controls.ListBox PastBookingList;
        
        internal Microsoft.Phone.Controls.PivotItem Pivot1;
        
        internal System.Windows.Controls.TextBlock CurrentBooking;
        
        internal System.Windows.Controls.ListBox CurrentBookingList;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/TaxiforSure;component/TrackMyTaxi.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.WaitingBar = ((System.Windows.Controls.StackPanel)(this.FindName("WaitingBar")));
            this.popup = ((System.Windows.Controls.Primitives.Popup)(this.FindName("popup")));
            this.bookingidPopup = ((System.Windows.Controls.TextBlock)(this.FindName("bookingidPopup")));
            this.edit = ((System.Windows.Controls.Button)(this.FindName("edit")));
            this.receipt = ((System.Windows.Controls.Button)(this.FindName("receipt")));
            this.txtEmail = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("txtEmail")));
            this.receiptSend = ((System.Windows.Controls.Button)(this.FindName("receiptSend")));
            this.receiptCancel = ((System.Windows.Controls.Button)(this.FindName("receiptCancel")));
            this.Cancel = ((System.Windows.Controls.Button)(this.FindName("Cancel")));
            this.popupCurrentBooking = ((System.Windows.Controls.Primitives.Popup)(this.FindName("popupCurrentBooking")));
            this.bookingidCurrentPopup = ((System.Windows.Controls.TextBlock)(this.FindName("bookingidCurrentPopup")));
            this.CancelBooking = ((System.Windows.Controls.Button)(this.FindName("CancelBooking")));
            this.Close = ((System.Windows.Controls.Button)(this.FindName("Close")));
            this.MainPivot = ((Microsoft.Phone.Controls.Pivot)(this.FindName("MainPivot")));
            this.Pivot2 = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("Pivot2")));
            this.PastBooking = ((System.Windows.Controls.TextBlock)(this.FindName("PastBooking")));
            this.PastBookingList = ((System.Windows.Controls.ListBox)(this.FindName("PastBookingList")));
            this.Pivot1 = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("Pivot1")));
            this.CurrentBooking = ((System.Windows.Controls.TextBlock)(this.FindName("CurrentBooking")));
            this.CurrentBookingList = ((System.Windows.Controls.ListBox)(this.FindName("CurrentBookingList")));
        }
    }
}

