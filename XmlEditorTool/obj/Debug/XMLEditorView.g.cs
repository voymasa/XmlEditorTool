﻿#pragma checksum "..\..\XMLEditorView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "6E49674AAF389F31F3E333D18F2604EC56EB177C17CFC5A1529F4E9E6A0FEE61"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
using XmlEditorTool;
using XmlEditorTool.Utility;


namespace XmlEditorTool {
    
    
    /// <summary>
    /// XMLEditorView
    /// </summary>
    public partial class XMLEditorView : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\XMLEditorView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Menu MenuBar;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\XMLEditorView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem AppMenu;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\XMLEditorView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem Settings;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\XMLEditorView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem Exit;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\XMLEditorView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid FileUploadGrid;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\XMLEditorView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel UploadPanel;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\XMLEditorView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView XmlTreeView;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\XMLEditorView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label FileNameLabel;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\XMLEditorView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView MacroTreeView;
        
        #line default
        #line hidden
        
        
        #line 144 "..\..\XMLEditorView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid DgGrid;
        
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
            System.Uri resourceLocater = new System.Uri("/XmlEditorTool;component/xmleditorview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\XMLEditorView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            this.MenuBar = ((System.Windows.Controls.Menu)(target));
            return;
            case 2:
            this.AppMenu = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 3:
            this.Settings = ((System.Windows.Controls.MenuItem)(target));
            
            #line 38 "..\..\XMLEditorView.xaml"
            this.Settings.Click += new System.Windows.RoutedEventHandler(this.OpenSettings);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Exit = ((System.Windows.Controls.MenuItem)(target));
            
            #line 39 "..\..\XMLEditorView.xaml"
            this.Exit.Click += new System.Windows.RoutedEventHandler(this.CloseApp);
            
            #line default
            #line hidden
            return;
            case 5:
            this.FileUploadGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.UploadPanel = ((System.Windows.Controls.StackPanel)(target));
            
            #line 60 "..\..\XMLEditorView.xaml"
            this.UploadPanel.Drop += new System.Windows.DragEventHandler(this.UploadDrop);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 64 "..\..\XMLEditorView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.UploadFileClick);
            
            #line default
            #line hidden
            return;
            case 8:
            this.XmlTreeView = ((System.Windows.Controls.TreeView)(target));
            
            #line 74 "..\..\XMLEditorView.xaml"
            this.XmlTreeView.SelectedItemChanged += new System.Windows.RoutedPropertyChangedEventHandler<object>(this.XmlTreeViewItemSelected);
            
            #line default
            #line hidden
            return;
            case 9:
            this.FileNameLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 10:
            this.MacroTreeView = ((System.Windows.Controls.TreeView)(target));
            return;
            case 11:
            this.DgGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

