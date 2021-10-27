using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using WeavyMobile;
using Android.Content;
using Weavy.WebView.Plugin.Forms.Droid;
using AndroidX.Core.App;
using ElenasList.Droid;
using WindowsAzure.Messaging.NotificationHubs;
using Firebase.Iid;
using Firebase.Messaging;
using Xamarin.Essentials;
using Constants = WeavyMobile.Constants;

namespace weavy_mobile.Droid
{
    
    [Activity(Label = "Buddy.aero", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static readonly string CHANNEL_ID = "my_notification_channel";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // push notification
            // https://docs.microsoft.com/en-us/azure/notification-hubs/xamarin-notification-hubs-push-notifications-android-gcm

            // Listen for push notifications
            NotificationHub.SetListener(new AzureNotificationHubListener());

            // Start the API
            NotificationHub.Start(this.Application, Constants.NotificationHubName, Constants.ListenConnectionString);


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            WeavyWebChromeClient.HandleActivityResult(requestCode, resultCode, data);

            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
    public class AzureNotificationHubListener : Java.Lang.Object, INotificationListener
    {
        public void OnPushNotificationReceived(Context context, INotificationMessage message)
        {
            var intent = new Intent(Application.Context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(Application.Context, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(Application.Context, MainActivity.CHANNEL_ID);

            notificationBuilder.SetContentTitle(message.Title)
                        .SetSmallIcon(ElenasList.Droid.Resource.Drawable.ic_launcher)
                        .SetContentText(message.Body)
                        .SetAutoCancel(true)
                        .SetShowWhen(false)
                        .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(Application.Context);

            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
    
    [Service(Name = "com.elenaslist.ElenasMobile.MyFirebaseMessagingService")]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        public override void OnNewToken(string p0)
        {
            base.OnNewToken(p0);
            Preferences.Set("token", p0);
        }
    }
}