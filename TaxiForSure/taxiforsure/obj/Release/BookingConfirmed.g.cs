﻿#pragma checksum "C:\Users\Anjani-Serendipity\Desktop\TaxiForSure\TaxiforSure\BookingConfirmed.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "84FDC4B21B720C583532B0EDAB94092A"
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
    
    
    public partial class BookingConfirmed : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Image confirmedImage;
        
        internal System.Windows.Controls.TextBlock IdBlock;
        
        internal System.Windows.Controls.TextBlock MessageBlock;
        
        internal System.Windows.Controls.Button DoneBtn;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TaxiforSure;component/BookingConfirmed.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.confirmedImage = ((System.Windows.Controls.Image)(this.FindName("confirmedImage")));
            this.IdBlock = ((System.Windows.Controls.TextBlock)(this.FindName("IdBlock")));
            this.MessageBlock = ((System.Windows.Controls.TextBlock)(this.FindName("MessageBlock")));
            this.DoneBtn = ((System.Windows.Controls.Button)(this.FindName("DoneBtn")));
        }
    }
}

