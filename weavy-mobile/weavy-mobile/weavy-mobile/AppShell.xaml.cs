using System;
using System.Collections.Generic;
using WeavyMobile.ViewModels;
using WeavyMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeavyMobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var loggedIn = Preferences.Get("loggedin", false);
            if (loggedIn)
            {
                await Shell.Current.GoToAsync($"//{nameof(SpacesPage)}");
            }
        }

    }
}
