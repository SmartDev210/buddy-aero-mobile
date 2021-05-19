using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeavyMobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeavyMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
        }
        private async void LoginWithWeb(object sender, EventArgs e)
        {
            try
            {
                var authResult = await WebAuthenticator.AuthenticateAsync(
                new Uri($"{Constants.ApiUrl}/mobile-api/auth"),
                new Uri("elenaslist-mobile://"));

                var accessToken = authResult?.AccessToken;

                if (accessToken == "fail")
                    await Shell.Current.DisplayAlert("", "Failed to login", "Dismiss");
                else
                {
                    App.JwtToken = accessToken;
                    Preferences.Set("loggedin", true);

                    MessagingCenter.Send(this, "TOKEN_REFRESH");

                    await Shell.Current.GoToAsync($"//{nameof(SpacesPage)}");
                }
            } catch (Exception)
            {
            }
            
        }
    }
}