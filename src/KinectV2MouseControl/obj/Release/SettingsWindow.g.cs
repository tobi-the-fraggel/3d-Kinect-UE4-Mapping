﻿#pragma checksum "..\..\SettingsWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "622DA5A7E4590E06E02CEAFE13F48A69"
//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using Mousenect;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Mousenect {
    
    
    /// <summary>
    /// SettingsWindow
    /// </summary>
    public partial class SettingsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\SettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider MouseSensitivity;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\SettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMouseSensitivity;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\SettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDefault;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\SettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkNoClick;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\SettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider CursorSmoothing;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\SettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtCursorSmoothing;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Mousenect;component/settingswindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SettingsWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 9 "..\..\SettingsWindow.xaml"
            ((Mousenect.SettingsWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.MouseSensitivity = ((System.Windows.Controls.Slider)(target));
            
            #line 11 "..\..\SettingsWindow.xaml"
            this.MouseSensitivity.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.MouseSensitivity_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtMouseSensitivity = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.btnDefault = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\SettingsWindow.xaml"
            this.btnDefault.Click += new System.Windows.RoutedEventHandler(this.btnDefault_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.chkNoClick = ((System.Windows.Controls.CheckBox)(target));
            
            #line 15 "..\..\SettingsWindow.xaml"
            this.chkNoClick.Checked += new System.Windows.RoutedEventHandler(this.chkNoClick_Checked);
            
            #line default
            #line hidden
            
            #line 15 "..\..\SettingsWindow.xaml"
            this.chkNoClick.Unchecked += new System.Windows.RoutedEventHandler(this.chkNoClick_Unchecked);
            
            #line default
            #line hidden
            return;
            case 6:
            this.CursorSmoothing = ((System.Windows.Controls.Slider)(target));
            
            #line 16 "..\..\SettingsWindow.xaml"
            this.CursorSmoothing.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.CursorSmoothing_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.txtCursorSmoothing = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
