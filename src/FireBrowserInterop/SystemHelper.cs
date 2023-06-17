using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;

namespace FireBrowserInterop
{
    public static class SystemHelper
    {
        public static async Task RestartApp()
        {
            AppRestartFailureReason result = await CoreApplication.RequestRestartAsync("restart");

            if (result is AppRestartFailureReason.NotInForeground or AppRestartFailureReason.Other)
            {
                throw new Exception("Failed to restart app. Please manually restart.");
            }
        }
        public static void WriteStringToClipboard(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = "";
            }

            var dataPackage = new DataPackage
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            dataPackage.SetText(text);
            Clipboard.SetContent(dataPackage);
        }
        public static string GetSystemArchitecture()
        {
            string architecture = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            return architecture ?? "Unknown";
        }
        public static void ShowShareUIURL(string title, string url)
        {
            var dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += (sender, args) =>
            {
                var request = args.Request;
                request.Data.SetWebLink(new Uri(url));
                request.Data.Properties.Title = title;
            };
            DataTransferManager.ShowShareUI();
        }

    }
}
