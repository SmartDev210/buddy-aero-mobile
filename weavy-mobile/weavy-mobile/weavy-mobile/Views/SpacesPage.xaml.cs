using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weavy.WebView.Plugin.Forms.Models;
using WeavyMobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeavyMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]    
    public partial class SpacesPage : ContentPage
    {
        private SpacesViewModel viewModel;
        public SpacesPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new SpacesViewModel();

            // a persistent demo jwt token that works for the demo site above
            weavyWebView.AuthenticationToken = App.JwtToken;

            // load the web view after init is complete. Note that the uri property is set in the .xaml view
            weavyWebView.InitCompleted += (s, a) =>
            {
                viewModel.IsBusy = true;
                weavyWebView.Load(Constants.WeavyUrl);
            };
            
            weavyWebView.SignedOut += async (sender, args) =>
            {
                Preferences.Remove("loggedin");
                await Shell.Current.GoToAsync("//LoginPage");
            };
            // listen to badge updated event
            weavyWebView.BadgeUpdated += (sender, args) =>
            {
                Console.WriteLine(args.Conversations); // messenger notifications
                Console.WriteLine(args.Notifications); // all other notifications
            };

            // web view is loading page
            weavyWebView.Loading += (sender, args) =>
            {
                Console.WriteLine("Loading webview...");
            };

            // an external link was clicked in the web view
            weavyWebView.LinkClicked += (sender, args) =>
            {
                Console.WriteLine("Link clicked...", args.Url);
                Launcher.OpenAsync(args.Url);
            };

            // web view has finished loading page
            weavyWebView.LoadFinished += (sender, args) =>
            {
                viewModel.IsBusy = false;

                Console.WriteLine("Load webview finished...");
                
                // example of getting current logged in user
                weavyWebView.GetUser((data) => {
                    try
                    {
                        var user = JsonConvert.DeserializeObject<User>(data);
                    }
                    catch (Exception)
                    {
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            await Shell.Current.GoToAsync("//LoginPage");
                            await Shell.Current.DisplayAlert("", "Failed to login to weavy. Please try again later!", "OK");
                        });
                    }
                });
            };
            
            // an error occurred when loading the page
            weavyWebView.LoadError += (sender, args) =>
            {
                Console.WriteLine("Error when loading webview!");
            };

            // get the current theme from the Weavy web site
            weavyWebView.Theming += (sender, theme) =>
            {
                Console.WriteLine("Got theme!", theme);
            };

            weavyWebView.SSOError += (sender, args) =>
            {
                Preferences.Remove("loggedin");
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync("//LoginPage");
                    await Shell.Current.DisplayAlert("", "Failed to login to weavy. Please try again later!", "OK");
                });
            };

            MessagingCenter.Subscribe<App>(this, "APP_RESUME", (s) => {
                weavyWebView.Resume();
            });
            MessagingCenter.Subscribe<LoginPage>(this, "TOKEN_REFRESH", (s) => {
                weavyWebView.AuthenticationToken = App.JwtToken;
                weavyWebView.Reload();
            });
        }
    }
}