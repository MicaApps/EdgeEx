using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FireBrowser.Pages.SettingsPages
{
    public sealed partial class TestDialog : ContentDialog
    {
        public TestDialog()
        {
            this.InitializeComponent();
           
        }

        private void TextAccount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextAccount.Text) && string.IsNullOrEmpty(TextId.Text))
            {
                Btn.IsEnabled = false;
            }
            else
            {
                Btn.IsEnabled = true;
            }
        }

        private async void Btn_Click(object sender, RoutedEventArgs e)
        {
            string folderName = TextAccount.Text.Trim();

            if (!string.IsNullOrEmpty(folderName))
            {
                try
                {
                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    StorageFolder newFolder = await localFolder.CreateFolderAsync(folderName, CreationCollisionOption.FailIfExists);

                    // Folder created successfully
                    Debug.WriteLine($"Folder '{newFolder.Name}' created.");
                }
                catch (Exception ex)
                {
                    // Failed to create folder
                    Debug.WriteLine($"Error creating folder: {ex.Message}");
                }
            }
        }

        private void TextId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextAccount.Text) && string.IsNullOrEmpty(TextId.Text))
            {
                Btn.IsEnabled = false;
            }
            else
            {
                Btn.IsEnabled = true;
            }
        }
    }
}
