﻿

#pragma checksum "C:\Users\Ronny\documents\visual studio 2013\Projects\MutiaraIman\MutiaraIman\MutiaraIman.Windows\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9AA72F5C4231A0C2E84A5EB8DCA1F5D4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MutiaraIman
{
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 12 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.pageRoot_Loaded;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 395 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.ContentWrapper_Tapped;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 416 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ShareButton_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 378 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.ContentList_SelectionChanged;
                 #line default
                 #line hidden
                #line 379 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.ContentList_Tapped;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 362 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_Click;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 328 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.CategoryList_SelectionChanged;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


