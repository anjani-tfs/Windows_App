﻿#pragma checksum "C:\Users\Anjani-Serendipity\Documents\TaxiForSure\TaxiforSure\CancelBooking.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "29E0FDA29E6307881EC5145183A9DA7F"
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
using Microsoft.Phone.Shell;
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
    
    
    public partial class CancelBooking : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock FromBlock;
        
        internal System.Windows.Controls.TextBlock ToBlock;
        
        internal System.Windows.Controls.TextBlock TimeBlock;
        
        internal System.Windows.Controls.TextBlock CarBlock;
        
        internal System.Windows.Controls.TextBlock CarFare;
        
        internal System.Windows.Controls.TextBlock CarExtraFare;
        
        internal Microsoft.Phone.Controls.PhoneTextBox ReasonBox;
        
        internal System.Windows.Controls.Grid LoadinGrid;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton app_nextButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TaxiforSure;component/CancelBooking.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.FromBlock = ((System.Windows.Controls.TextBlock)(this.FindName("FromBlock")));
            this.ToBlock = ((System.Windows.Controls.TextBlock)(this.FindName("ToBlock")));
            this.TimeBlock = ((System.Windows.Controls.TextBlock)(this.FindName("TimeBlock")));
            this.CarBlock = ((System.Windows.Controls.TextBlock)(this.FindName("CarBlock")));
            this.CarFare = ((System.Windows.Controls.TextBlock)(this.FindName("CarFare")));
            this.CarExtraFare = ((System.Windows.Controls.TextBlock)(this.FindName("CarExtraFare")));
            this.ReasonBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("ReasonBox")));
            this.LoadinGrid = ((System.Windows.Controls.Grid)(this.FindName("LoadinGrid")));
            this.app_nextButton = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("app_nextButton")));
        }
    }
}

