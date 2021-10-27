using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ElenasList.Droid.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeavyMobile;
using WeavyMobile.Services;
using WindowsAzure.Messaging;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(AzureNotificationHubService))]
namespace ElenasList.Droid.Notifications
{

    public class AzureNotificationHubService : INotificationService
    {
        public void Register(string tag)
        {
            // get stored token from Firebase Messaging registration process. Store this in a setting when the firebase registration completes.

            
            var token =  Preferences.Get("token", "");
            if (!string.IsNullOrEmpty(token))
            {
                // create azure notificaion hub
                var hub = new NotificationHub(Constants.NotificationHubName, Constants.ListenConnectionString, Application.Context);
                try
                {
                    hub.UnregisterAll(token);
                }
                catch (Exception ex)
                {
                    // handle error
                }

                // notifcation payload template for Android Firebase Messaging
                const string template = "{\"data\" : { \"message\" : \"$(message)\", \"badge\" : \"#(badge)\", \"sound\" : \"default\", \"url\" : \"$(url)\" }}";
                try
                {   
                    var result = hub.RegisterTemplate(token, "Template", template, new string[] { tag });

                }
                catch (Exception ex)
                {
                    // handle error
                }
            }
        }

        public void ResetBadgeCount(int badge)
        {
            
        }
    }
}