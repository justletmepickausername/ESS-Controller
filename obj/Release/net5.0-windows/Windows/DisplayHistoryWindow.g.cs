﻿#pragma checksum "..\..\..\..\Windows\DisplayHistoryWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E11249EDDAA30AA8D0DD0437342E1EE42A41513E"
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
using MahApps.Metro.Controls;
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
    /// DisplayHistoryWindow
    /// </summary>
    public partial class DisplayHistoryWindow : MahApps.Metro.Controls.MetroWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\Windows\DisplayHistoryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ESS_Controller.Windows.DisplayHistoryWindow Display;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\Windows\DisplayHistoryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtblockTitle;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\Windows\DisplayHistoryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtblockMaxTemp;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\Windows\DisplayHistoryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtblockMinTemp;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\Windows\DisplayHistoryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtblockCycles;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\Windows\DisplayHistoryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtblockStayTime;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\Windows\DisplayHistoryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtblockDate;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\Windows\DisplayHistoryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtblockProduct;
        
        #line default
        #line hidden
        
        /// <summary>
        /// ovenGraph Name Field
        /// </summary>
        
        #line 22 "..\..\..\..\Windows\DisplayHistoryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public LiveCharts.Wpf.CartesianChart ovenGraph;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\Windows\DisplayHistoryWindow.xaml"
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
            System.Uri resourceLocater = new System.Uri("/ESS Controller;component/windows/displayhistorywindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\DisplayHistoryWindow.xaml"
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
            this.Display = ((ESS_Controller.Windows.DisplayHistoryWindow)(target));
            return;
            case 2:
            this.txtblockTitle = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.txtblockMaxTemp = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.txtblockMinTemp = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.txtblockCycles = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.txtblockStayTime = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.txtblockDate = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.txtblockProduct = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.ovenGraph = ((LiveCharts.Wpf.CartesianChart)(target));
            return;
            case 10:
            this.mySeries = ((LiveCharts.Wpf.LineSeries)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

