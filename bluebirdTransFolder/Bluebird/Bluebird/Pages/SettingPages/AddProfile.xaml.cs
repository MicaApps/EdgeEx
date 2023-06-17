using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Bluebird.Pages.SettingPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddProfile : Page
    {
        public AddProfile()
        {
            this.InitializeComponent();
        }

        private async void SavePrf_Click(object sender, RoutedEventArgs e)
        {
            string folderName = ProfileUs.Text;

            try
            {
                // Try to get the folder
                StorageFolder folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(folderName);

                // If the folder was found, it exists
                if (folder != null)
                {
                    // do something
                    System.Diagnostics.Debug.WriteLine("Folder exists");
                    return;
                }
            }
            catch (FileNotFoundException)
            {
                StorageFolder newFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(folderName, CreationCollisionOption.FailIfExists);
            }
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("ProfileCore.rash", CreationCollisionOption.OpenIfExists);
            await FileIO.AppendTextAsync(file, ProfileUs.Text + Environment.NewLine);
        }
    }
}
