﻿#pragma checksum "..\..\..\..\..\Windows\EditProductWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6CF5AE4A84AE803BA7BEBF659441208746822E7A"
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
using System.Windows.Forms.Integration;
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
    /// EditProductWindow
    /// </summary>
    public partial class EditProductWindow : MahApps.Metro.Controls.MetroWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\..\..\..\Windows\EditProductWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboboxProducts;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\..\Windows\EditProductWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtboxDwellTime;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\..\Windows\EditProductWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtboxCycles;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\..\Windows\EditProductWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtboxMaxTemp;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\..\Windows\EditProductWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtboxMinTemp;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\..\Windows\EditProductWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnUpdate;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.7.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ESS Controller;V1.0.0.0;component/windows/editproductwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Windows\EditProductWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.7.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.comboboxProducts = ((System.Windows.Controls.ComboBox)(target));
            
            #line 18 "..\..\..\..\..\Windows\EditProductWindow.xaml"
            this.comboboxProducts.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.comboboxProducts_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtboxDwellTime = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.txtboxCycles = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.txtboxMaxTemp = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.txtboxMinTemp = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.btnUpdate = ((System.Windows.Controls.Button)(target));
            
            #line 45 "..\..\..\..\..\Windows\EditProductWindow.xaml"
            this.btnUpdate.Click += new System.Windows.RoutedEventHandler(this.btnUpdate_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

