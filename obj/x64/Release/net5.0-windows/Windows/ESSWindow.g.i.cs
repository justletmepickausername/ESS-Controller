﻿#pragma checksum "..\..\..\..\..\Windows\ESSWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2EEE4DB7B9FD76954D7E672AF30E286F1F35E100"
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
using LiveCharts.Wpf;
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
    /// ESSWindow
    /// </summary>
    public partial class ESSWindow : MahApps.Metro.Controls.MetroWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\..\..\..\Windows\ESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ESS_Controller.Windows.ESSWindow Main;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\..\Windows\ESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtblockTitle;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\..\Windows\ESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtblockPorts;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\..\Windows\ESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboBoxPortsList;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\..\Windows\ESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtboxMaxTemp;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\..\Windows\ESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtblockMaxTemp;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\..\Windows\ESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnStartStopESS;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\..\Windows\ESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtboxMinTemp;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\..\Windows\ESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtblockMinTemp;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\..\Windows\ESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtboxCycles;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\..\Windows\ESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtblockCycles;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\..\Windows\ESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtboxStayTime;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\..\Windows\ESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtblockStayTime;
        
        #line default
        #line hidden
        
        /// <summary>
        /// listviewLog Name Field
        /// </summary>
        
        #line 42 "..\..\..\..\..\Windows\ESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public System.Windows.Controls.ListView listviewLog;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\..\Windows\ESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal LiveCharts.Wpf.CartesianChart ovenGraph;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\..\..\Windows\ESSWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal LiveCharts.Wpf.LineSeries mySeries;
        
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
            System.Uri resourceLocater = new System.Uri("/ESS Controller;V1.0.0.0;component/windows/esswindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Windows\ESSWindow.xaml"
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
            this.Main = ((ESS_Controller.Windows.ESSWindow)(target));
            return;
            case 2:
            this.txtblockTitle = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.txtblockPorts = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.comboBoxPortsList = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.txtboxMaxTemp = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.txtblockMaxTemp = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.btnStartStopESS = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\..\..\Windows\ESSWindow.xaml"
            this.btnStartStopESS.Click += new System.Windows.RoutedEventHandler(this.btnStartStopESS_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.txtboxMinTemp = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.txtblockMinTemp = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.txtboxCycles = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.txtblockCycles = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 12:
            this.txtboxStayTime = ((System.Windows.Controls.TextBox)(target));
            return;
            case 13:
            this.txtblockStayTime = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 14:
            this.listviewLog = ((System.Windows.Controls.ListView)(target));
            return;
            case 15:
            this.ovenGraph = ((LiveCharts.Wpf.CartesianChart)(target));
            return;
            case 16:
            this.mySeries = ((LiveCharts.Wpf.LineSeries)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
