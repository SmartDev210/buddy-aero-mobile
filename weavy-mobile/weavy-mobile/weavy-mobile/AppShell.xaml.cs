using System;
using System.Collections.Generic;
using WeavyMobile.ViewModels;
using WeavyMobile.Views;
using Xamarin.Forms;

namespace WeavyMobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
