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
    public partial class MessengersPage : ContentPage
    {
        private MessengersViewModel viewModel;
        public MessengersPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new MessengersViewModel();

            // a persistent demo jwt token that works for the demo site above

            weavyMessenger.AuthenticationToken = App.JwtToken;

            // load the web view after init is complete. Set the url to the /messenger
            weavyMessenger.InitCompleted += (sender, args) =>
            {
                weavyMessenger.Load($"{Constants.WeavyUrl}/messenger");
            };

            weavyMessenger.BadgeUpdated += (sender, args) =>
            {

            };

            weavyMessenger.LinkClicked += (sender, args) =>
            {
                Console.WriteLine("Link clicked...", args.Url);
                Launcher.OpenAsync(args.Url);
            };

            // web view has finished loading page
            weavyMessenger.LoadFinished += (sender, args) =>
            {
                Console.WriteLine("Load webview finished...");

                // exampleof getting current logged in user
                weavyMessenger.GetUser((data) => {
                    var user = JsonConvert.DeserializeObject<User>(data);
                });
            };
            
        }
    }
}