using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using WindowsAzure.Messaging.NotificationHubs;
using UserNotifications;

namespace WeavyMobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());


            //RegisterForRemoteNotifications();

            
            // Set the Message listener
            MSNotificationHub.SetDelegate(new AzureNotificationHubListener());

            //Start the SDK
            MSNotificationHub.Start(Constants.ListenConnectionString, Constants.NotificationHubName);
            
            return base.FinishedLaunching(app, options);
        }

        void RegisterForRemoteNotifications()
        {
            // register for remote notifications based on system version
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert |
                    UNAuthorizationOptions.Sound |
                    UNAuthorizationOptions.Sound,
                    (granted, error) =>
                    {
                        if (granted)
                            InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                    });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if (Xamarin.Essentials.Platform.OpenUrl(app, url, options))
                return true;
            return base.OpenUrl(app, url, options);
        }

        public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
        {
            if (Xamarin.Essentials.Platform.ContinueUserActivity(application, userActivity, completionHandler))
                return true;
            return base.ContinueUserActivity(application, userActivity, completionHandler);
        }
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            NSUserDefaults.StandardUserDefaults["token"] = deviceToken;
            
        }
        public override void OnActivated(UIApplication uiApplication)
        {
            // UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
            base.OnActivated(uiApplication);
        }
    }

    public class AzureNotificationHubListener : MSNotificationHubDelegate
    {
        public override void DidReceivePushNotification(MSNotificationHub notificationHub, MSNotificationHubMessage message)
        {
            var alertTitle = message.Title ?? "Aviation Back-Channel";
            var alertBody = message.Body;

            // this sample assumes {aps: { alert: { title: "hello", body: "world" } } }
            if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active
                && AppShell.Current?.CurrentItem?.CurrentItem?.Title == "Messengers"
                && alertBody.Contains(":")) {

            } else
            {
                var myAlert = UIAlertController.Create(alertTitle, alertBody, UIAlertControllerStyle.Alert);
                myAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(myAlert, true, null);
            }
        }
    }
}
