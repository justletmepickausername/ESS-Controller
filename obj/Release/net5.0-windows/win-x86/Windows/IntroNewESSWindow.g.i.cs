﻿#pragma checksum "..\..\..\..\..\Windows\IntroNewESSWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "FCA51D58F2B397E1B9E5EBF622C2613852086710"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ESS_Controller.Windows;
using MahApps.Metro;
using MahApps.Metro.Accessibility;
using MahApps.Metro.Actions;
using MahApps.Metro.Automation.Peers;
using MahApps.Metro.Behaviors;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Converters;
using MahApps.Metro.Markup;
using MahApps.Metro.Theming;
using MahApps.Metro.ValueBoxes;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace ESS_Controller.Windows {
    
    
    /// <summary>
    /// IntroNewESSWindow
    /// </summary>
    public partial class IntroNewESSWindow : MahApps.Metro.Controls.MetroWindow, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 11 "..\..\..\..\..\Windows\IntroNewESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ESS_Controller.Windows.IntroNewESSWindow Intro;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\..\Windows\IntroNewESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtblockTitle;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\..\Windows\IntroNewESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboboxProducts;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\..\Windows\IntroNewESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtblockProducts;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\..\Windows\IntroNewESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnContinue;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\..\Windows\IntroNewESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtblockSerialNumber;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\..\Windows\IntroNewESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtboxSerialNumber;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\..\Windows\IntroNewESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listViewSerialNumbers;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.9.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ESS Controller;component/windows/intronewesswindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Windows\IntroNewESSWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.9.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Intro = ((ESS_Controller.Windows.IntroNewESSWindow)(target));
            return;
            case 3:
            this.txtblockTitle = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.comboboxProducts = ((System.Windows.Controls.ComboBox)(target));
            
            #line 25 "..\..\..\..\..\Windows\IntroNewESSWindow.xaml"
            this.comboboxProducts.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.comboboxProducts_SelectionChanged_1);
            
            #line default
            #line hidden
            return;
            case 5:
            this.txtblockProducts = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.btnContinue = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\..\..\..\Windows\IntroNewESSWindow.xaml"
            this.btnContinue.Click += new System.Windows.RoutedEventHandler(this.btnContinue_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.txtblockSerialNumber = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.txtboxSerialNumber = ((System.Windows.Controls.TextBox)(target));
            
            #line 29 "..\..\..\..\..\Windows\IntroNewESSWindow.xaml"
            this.txtboxSerialNumber.KeyDown += new System.Windows.Input.KeyEventHandler(this.TextBox_KeyDown);
            
            #line default
            #line hidden
            return;
            case 9:
            this.listViewSerialNumbers = ((System.Windows.Controls.ListView)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.9.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 2:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.Control.MouseDoubleClickEvent;
            
            #line 18 "..\..\..\..\..\Windows\IntroNewESSWindow.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.ListViewItem_MouseDoubleClick);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}

