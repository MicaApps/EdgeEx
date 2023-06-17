using System;
using System.IO;
using Windows.ApplicationModel.Email;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;

namespace FireExceptions
{
    public class ExceptionsHelper
    {
        public static async void LogException(Exception ex)
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("bug.fbug", CreationCollisionOption.OpenIfExists);

                using (StreamWriter writer = new StreamWriter(await file.OpenStreamForWriteAsync()))
                {
                    writer.WriteLine("Exception Occurred at " + DateTime.Now.ToString());
                    writer.WriteLine("Message: " + ex.Message);
                    writer.WriteLine("Stack Trace: " + ex.StackTrace);
                    writer.WriteLine("Inner Trace: " + ex.InnerException);
                    writer.WriteLine("Source Trace: " + ex.Source);
                    writer.WriteLine("-------------------------------------------------------------");
                }

                var dialog = new MessageDialog($"The app encountered an error. Would you like to send the bug report to the developers?, {ex.Message}", "Error");
                dialog.Commands.Add(new UICommand("Yes", async (command) =>
                {
                    var email = new EmailMessage();
                    email.Subject = "Bug Report";
                    email.To.Add(new EmailRecipient("firebrowserdevs@gmail.com"));
                    email.Body = "Any Extra Info Can Help Find The Bug" +
                    "Please find attached file for the bug report above.";
                    var fileStream = await file.OpenReadAsync();
                    var contentType = file.ContentType;
                    var streamRef = RandomAccessStreamReference.CreateFromStream(fileStream);
                    email.Attachments.Add(new EmailAttachment(file.Name, streamRef, contentType));
                    await EmailManager.ShowComposeNewEmailAsync(email);
                }));
                dialog.Commands.Add(new UICommand("No"));
                await dialog.ShowAsync();
            }
            catch { }
        }
    }
}
