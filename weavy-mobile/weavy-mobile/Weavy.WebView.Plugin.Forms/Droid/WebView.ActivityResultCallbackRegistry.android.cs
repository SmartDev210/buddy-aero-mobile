using Android.App;
using Android.Content;
using System;
using System.Collections.Concurrent;

namespace Weavy.WebView.Plugin.Forms.Droid
{

    /// <summary>
    /// Keeps track of registered activity result callbacks
    /// </summary>
    public static class WeavyActivityResultCallbackRegistry
    {
        static readonly ConcurrentDictionary<int, Action<Result, Intent>> s_activityResultCallbacks =
            new ConcurrentDictionary<int, Action<Result, Intent>>();

        static int s_nextActivityResultCallbackKey;

        public static void InvokeCallback(int requestCode, Result resultCode, Intent data)
        {
            Action<Result, Intent> callback;

            if (s_activityResultCallbacks.TryGetValue(requestCode, out callback))
            {
                callback(resultCode, data);
            }
        }

        internal static int RegisterActivityResultCallback(Action<Result, Intent> callback)
        {
            int requestCode = s_nextActivityResultCallbackKey;

            while (!s_activityResultCallbacks.TryAdd(requestCode, callback))
            {
                s_nextActivityResultCallbackKey += 1;
                requestCode = s_nextActivityResultCallbackKey;
            }

            s_nextActivityResultCallbackKey += 1;

            return requestCode;
        }

        internal static void UnregisterActivityResultCallback(int requestCode)
        {
            Action<Result, Intent> callback;
            s_activityResultCallbacks.TryRemove(requestCode, out callback);
        }
    }

}
