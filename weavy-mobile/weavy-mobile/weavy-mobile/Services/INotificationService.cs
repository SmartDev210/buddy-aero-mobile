using System;
namespace WeavyMobile.Services
{
    public interface INotificationService
    {

        /// <summary>
        /// Register with the Azure Notification Hub
        /// </summary>
        /// <param name="tag">The tag of the user to register.</param>
        void Register(string tag);

        /// <summary>
        /// Reset app's badge count
        /// </summary>
        /// <param name="badge"></param>
        void ResetBadgeCount(int badge);
    }
}
