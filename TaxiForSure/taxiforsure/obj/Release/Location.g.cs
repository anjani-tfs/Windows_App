﻿#pragma checksum "C:\Users\Anjani-Serendipity\Documents\TaxiForSure\TaxiforSure\Location.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EBE29C0B3A82A5B2F2670B3691CFDF53"
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
        
        internal System.Windows.Controls.ScrollViewer gridHolder;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock FromBlock;
        
        internal System.Windows.Controls.Image pickFavourite;
        
        internal System.Windows.Controls.TextBlock ToBlock;
        
        internal System.Windows.Controls.Image dropFavourite;
        
        internal System.Windows.Documents.Run VehicleNumberRun;
        
        internal System.Windows.Documents.Run VehicleGap;
        
        internal System.Windows.Documents.Run CarNameRun;
        
        internal System.Windows.Controls.Grid grdMessage;
        
        internal System.Windows.Controls.TextBlock MessageBlock;
        
        internal System.Windows.Controls.StackPanel DriverDetailPanel;
        
        internal System.Windows.Controls.TextBlock NameBlock;
        
        internal System.Windows.Controls.TextBlock PhoneBlock;
        
        internal System.Windows.Controls.Primitives.Popup popup;
        
        internal System.Windows.Controls.RadioButton Work;
        
        internal System.Windows.Controls.RadioButton Office;
        
        internal System.Windows.Controls.RadioButton Hangout;
        
        internal System.Windows.Controls.RadioButton Other;
        
        internal Microsoft.Phone.Controls.PhoneTextBox txtOther;
        
        internal System.Windows.Controls.Button Submit;
        
        internal System.Windows.Controls.Button Cancel;
        
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
            this.gridHolder = ((System.Windows.Controls.ScrollViewer)(this.FindName("gridHolder")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.FromBlock = ((System.Windows.Controls.TextBlock)(this.FindName("FromBlock")));
            this.pickFavourite = ((System.Windows.Controls.Image)(this.FindName("pickFavourite")));
            this.ToBlock = ((System.Windows.Controls.TextBlock)(this.FindName("ToBlock")));
            this.dropFavourite = ((System.Windows.Controls.Image)(this.FindName("dropFavourite")));
            this.VehicleNumberRun = ((System.Windows.Documents.Run)(this.FindName("VehicleNumberRun")));
            this.VehicleGap = ((System.Windows.Documents.Run)(this.FindName("VehicleGap")));
            this.CarNameRun = ((System.Windows.Documents.Run)(this.FindName("CarNameRun")));
            this.grdMessage = ((System.Windows.Controls.Grid)(this.FindName("grdMessage")));
            this.MessageBlock = ((System.Windows.Controls.TextBlock)(this.FindName("MessageBlock")));
            this.DriverDetailPanel = ((System.Windows.Controls.StackPanel)(this.FindName("DriverDetailPanel")));
            this.NameBlock = ((System.Windows.Controls.TextBlock)(this.FindName("NameBlock")));
            this.PhoneBlock = ((System.Windows.Controls.TextBlock)(this.FindName("PhoneBlock")));
            this.popup = ((System.Windows.Controls.Primitives.Popup)(this.FindName("popup")));
            this.Work = ((System.Windows.Controls.RadioButton)(this.FindName("Work")));
            this.Office = ((System.Windows.Controls.RadioButton)(this.FindName("Office")));
            this.Hangout = ((System.Windows.Controls.RadioButton)(this.FindName("Hangout")));
            this.Other = ((System.Windows.Controls.RadioButton)(this.FindName("Other")));
            this.txtOther = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("txtOther")));
            this.Submit = ((System.Windows.Controls.Button)(this.FindName("Submit")));
            this.Cancel = ((System.Windows.Controls.Button)(this.FindName("Cancel")));
        }
    }
}

