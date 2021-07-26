using System;
using System.Globalization;

using Foundation;
using UIKit;
using WeavyMobile.iOS.Notifications;
using WeavyMobile.Services;
using WindowsAzure.Messaging;

[assembly: Xamarin.Forms.Dependency(typeof(AzureNotificationHubService))]
namespace WeavyMobile.iOS.Notifications
{

    /// <summary>
    /// iOS implementation of the Notification Service
    /// </summary>
    public class AzureNotificationHubService : INotificationService
    {

        /// <summary>
        /// Register a device to the Azure notification hub
        /// </summary>
        /// <param name="tag">The user tag to associate the registration with</param>
        public void Register(string tag)
        {

            //Get token from APN registration process. Store the token when registration process was completed.            
            var token = NSUserDefaults.StandardUserDefaults["token"];

            if (token != null)
            {

                var hub = new SBNotificationHub(Constants.ListenConnectionString, Constants.NotificationHubName);

                hub.UnregisterAll(token as NSData, (error) => {
                    if (error != null)
                    {
                        return;
                    }

                    var tags = new NSSet(tag);
                    var expire = DateTime.Now.AddYears(1).ToString(CultureInfo.CreateSpecificCulture("en-US"));

                    NSError returnError;
                    
                    hub.RegisterTemplate(token as NSData,
                        "Template",
                        "{\"aps\":{\"alert\":\"$(message)\", \"content-available\":\"#(silent)\", \"badge\":\"#(badge)\", \"sound\":\"$(sound)\"}, \"url\":\"$(url)\"}",
                        expire,
                        tags,
                        out returnError);
                    

                });
            }


        }

        public void ResetBadgeCount(int badge)
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = badge;
        }
    }
}