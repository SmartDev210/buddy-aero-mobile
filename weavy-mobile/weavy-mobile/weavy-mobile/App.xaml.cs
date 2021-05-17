using System;
using WeavyMobile.Services;
using WeavyMobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeavyMobile
{
    public partial class App : Application
    {
        public static string JwtToken { get; set; }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
