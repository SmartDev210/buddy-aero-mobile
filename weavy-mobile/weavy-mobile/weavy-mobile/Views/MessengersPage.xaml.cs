using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weavy.WebView.Plugin.Forms.Models;
using WeavyMobile.Services;
using WeavyMobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeavyMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(Url), "url")]
    public partial class MessengersPage : ContentPage
    {
        private MessengersViewModel viewModel;
        public string Url
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    InitialUrl = value;
                    if (weavyMessenger.Uri != InitialUrl)
                        weavyMessenger.Load(InitialUrl);
                }
            }
        }
        public string InitialUrl { get; set; }
        public MessengersPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new MessengersViewModel();

            // a persistent demo jwt token that works for the demo site above

            weavyMessenger.AuthenticationToken = App.JwtToken;

            // load the web view after init is complete. Set the url to the /messenger
            weavyMessenger.InitCompleted += (sender, args) =>
            {
                viewModel.IsBusy = true;
                if (string.IsNullOrEmpty(InitialUrl))
                {
                    InitialUrl = $"{Constants.WeavyUrl}/messenger";
                    weavyMessenger.Load($"{Constants.WeavyUrl}/messenger");
                } else
                {
                    weavyMessenger.Load(InitialUrl);
                }
            };

            weavyMessenger.BadgeUpdated += (sender, args) =>
            {
                
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DependencyService.Get<INotificationService>().ResetBadgeCount(args.Conversations + args.Notifications);
                });
                
            };
            weavyMessenger.SignedOut += async (sender, args) =>
            {
                Preferences.Remove("loggedin");
                await Shell.Current.GoToAsync("//LoginPage");
            };
            weavyMessenger.LinkClicked += (sender, args) =>
            {
                Console.WriteLine("Link clicked...", args.Url);
                if (args.Url.Contains("/direct-call") || args.Url.Contains("/direct-space"))
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await Shell.Current.GoToAsync($"//SpacesPage?url={args.Url}");
                        //await Shell.Current.DisplayAlert("", "Failed to login to weavy. Please try again later!", "OK");
                    });
                }
                else
                {
                    Launcher.OpenAsync(args.Url);
                }
            };

            // web view has finished loading page
            weavyMessenger.LoadFinished += (sender, args) =>
            {
                viewModel.IsBusy = false;
                Console.WriteLine("Load webview finished...");

                // exampleof getting current logged in user
                weavyMessenger.GetUser((data) => {
                    try
                    {
                        var user = JsonConvert.DeserializeObject<User>(data);
                    } catch (Exception)
                    {
                        Preferences.Remove("loggedin");
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            await Shell.Current.GoToAsync("//LoginPage");
                            await Shell.Current.DisplayAlert("", "Failed to login to weavy. Please try again later!", "OK");
                        });
                    }
                });
            };

            weavyMessenger.SSOError += (sender, args) =>
            {
                Preferences.Remove("loggedin");
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync("//LoginPage");
                    await Shell.Current.DisplayAlert("", "Failed to login to weavy. Please try again later!", "OK");
                });
            };

            MessagingCenter.Subscribe<LoginPage>(this, "TOKEN_REFRESH", (s) => {
                weavyMessenger.AuthenticationToken = App.JwtToken;
                weavyMessenger.Reload();
            });
        }
    }
}