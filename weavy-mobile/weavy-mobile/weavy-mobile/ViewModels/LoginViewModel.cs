using System;
using System.Collections.Generic;
using System.Text;
using WeavyMobile.Helpers;
using WeavyMobile.Models;
using WeavyMobile.Views;
using Xamarin.Forms;

namespace WeavyMobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public string Email { get; set; }
        public string Password { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            JwtTokenResult result = new JwtTokenResult();
            try
            {
                 result = await RestHelper.PostAsync<JwtTokenResult>($"{Constants.ApiUrl}/mobile-api/weavy-jwt", new { Email, Password });

                if (!string.IsNullOrEmpty(result.Token))
                {
                    App.JwtToken = result.Token;
                    // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                    await Shell.Current.GoToAsync($"//{nameof(SpacesPage)}");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Login Failed", result.Status, "Dismiss");
                }

            } catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Dismiss");
            }

            
            
        }
    }
}
