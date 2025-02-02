﻿#pragma checksum "..\..\..\DoorGenerator.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6E6FAA4114404C285DD218FC0598F4C7A8B60FA8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SevenStarsTools;
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


namespace SevenStarsTools {
    
    
    /// <summary>
    /// DoorGenerator
    /// </summary>
    public partial class DoorGenerator : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\..\DoorGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOpenFile;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\DoorGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image sourceImage;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\DoorGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnGenerate;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\DoorGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSave;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\DoorGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock details;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\DoorGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox fileName;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\DoorGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox onsave_explorer_checkbox;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\DoorGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid generatedImageGrid;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SevenStarsTools;component/doorgenerator.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\DoorGenerator.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.btnOpenFile = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\..\DoorGenerator.xaml"
            this.btnOpenFile.Click += new System.Windows.RoutedEventHandler(this.btnOpenFile_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.sourceImage = ((System.Windows.Controls.Image)(target));
            return;
            case 3:
            this.btnGenerate = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\..\DoorGenerator.xaml"
            this.btnGenerate.Click += new System.Windows.RoutedEventHandler(this.btnGenerate_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnSave = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\..\DoorGenerator.xaml"
            this.btnSave.Click += new System.Windows.RoutedEventHandler(this.btnSave_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.details = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.fileName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.onsave_explorer_checkbox = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 8:
            this.generatedImageGrid = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

