﻿#pragma checksum "C:\Users\Anjani-Serendipity\Documents\TaxiForSure\TaxiforSure\Registration.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8325AADB719B17BFBF45521573E2CB25"
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
    
    
    public partial class Registration : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.PhoneTextBox txtname;
        
        internal Microsoft.Phone.Controls.PhoneTextBox txtPhoneNo;
        
        internal Microsoft.Phone.Controls.PhoneTextBox txtEmail;
        
        internal System.Windows.Controls.PasswordBox txtPassword;
        
        internal Microsoft.Phone.Controls.PhoneTextBox txtPasswordForHint;
        
        internal System.Windows.Controls.PasswordBox txtConfirmPassword;
        
        internal Microsoft.Phone.Controls.PhoneTextBox txtConfirmPasswordForHint;
        
        internal System.Windows.Controls.Grid LoadinGrid;
        
        internal System.Windows.Controls.Button btnSignIn;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton ProceedBtn;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TaxiforSure;component/Registration.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtname = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("txtname")));
            this.txtPhoneNo = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("txtPhoneNo")));
            this.txtEmail = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("txtEmail")));
            this.txtPassword = ((System.Windows.Controls.PasswordBox)(this.FindName("txtPassword")));
            this.txtPasswordForHint = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("txtPasswordForHint")));
            this.txtConfirmPassword = ((System.Windows.Controls.PasswordBox)(this.FindName("txtConfirmPassword")));
            this.txtConfirmPasswordForHint = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("txtConfirmPasswordForHint")));
            this.LoadinGrid = ((System.Windows.Controls.Grid)(this.FindName("LoadinGrid")));
            this.btnSignIn = ((System.Windows.Controls.Button)(this.FindName("btnSignIn")));
            this.ProceedBtn = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("ProceedBtn")));
        }
    }
}

