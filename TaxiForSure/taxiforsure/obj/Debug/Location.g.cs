﻿#pragma checksum "C:\Users\Ankit Khullar\Downloads\Compressed\TaxiforSure\TaxiforSure\Location.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E1BB32BBE0AB597C4B68DB7F8C2231EF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
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
    
    
    public partial class Location : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Maps.Controls.Map LocationMap;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock FromBlock;
        
        internal System.Windows.Controls.TextBlock ToBlock;
        
        internal System.Windows.Documents.Run VehicleNumberRun;
        
        internal System.Windows.Documents.Run VehicleGap;
        
        internal System.Windows.Documents.Run CarNameRun;
        
        internal System.Windows.Controls.TextBlock MessageBlock;
        
        internal System.Windows.Controls.StackPanel DriverDetailPanel;
        
        internal System.Windows.Controls.TextBlock NameBlock;
        
        internal System.Windows.Controls.TextBlock PhoneBlock;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TaxiforSure;component/Location.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.LocationMap = ((Microsoft.Phone.Maps.Controls.Map)(this.FindName("LocationMap")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.FromBlock = ((System.Windows.Controls.TextBlock)(this.FindName("FromBlock")));
            this.ToBlock = ((System.Windows.Controls.TextBlock)(this.FindName("ToBlock")));
            this.VehicleNumberRun = ((System.Windows.Documents.Run)(this.FindName("VehicleNumberRun")));
            this.VehicleGap = ((System.Windows.Documents.Run)(this.FindName("VehicleGap")));
            this.CarNameRun = ((System.Windows.Documents.Run)(this.FindName("CarNameRun")));
            this.MessageBlock = ((System.Windows.Controls.TextBlock)(this.FindName("MessageBlock")));
            this.DriverDetailPanel = ((System.Windows.Controls.StackPanel)(this.FindName("DriverDetailPanel")));
            this.NameBlock = ((System.Windows.Controls.TextBlock)(this.FindName("NameBlock")));
            this.PhoneBlock = ((System.Windows.Controls.TextBlock)(this.FindName("PhoneBlock")));
        }
    }
}

