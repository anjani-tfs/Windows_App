﻿#pragma checksum "C:\Users\Anjani-Serendipity\Desktop\TaxiForSure\TaxiforSure\LocationSelect.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EF105BB44669314BCFE51EA6583DD62B"
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
    
    
    public partial class LocationSelect : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBox inputBox;
        
        internal System.Windows.Controls.StackPanel OptionsPanel;
        
        internal System.Windows.Controls.TextBlock AirportTextBlock;
        
        internal System.Windows.Controls.TextBlock DelhiAirBlock;
        
        internal System.Windows.Controls.ListBox resultBox;
        
        internal System.Windows.Controls.Grid LoadinGrid;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TaxiforSure;component/LocationSelect.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.inputBox = ((System.Windows.Controls.TextBox)(this.FindName("inputBox")));
            this.OptionsPanel = ((System.Windows.Controls.StackPanel)(this.FindName("OptionsPanel")));
            this.AirportTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("AirportTextBlock")));
            this.DelhiAirBlock = ((System.Windows.Controls.TextBlock)(this.FindName("DelhiAirBlock")));
            this.resultBox = ((System.Windows.Controls.ListBox)(this.FindName("resultBox")));
            this.LoadinGrid = ((System.Windows.Controls.Grid)(this.FindName("LoadinGrid")));
        }
    }
}

