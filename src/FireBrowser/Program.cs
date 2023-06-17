using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireTabs;
using FireBrowser;
using FireBrowser.SetupForm;
using Kimtoo;

namespace FireBrowser
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            WebContainer container = new WebContainer();

            // Add the initial Tab
            container.Tabs.Add(
                // Our First Tab created by default in the Application will have as content the Form1
                new FireTitleTab(container)
                {
                    Content = new BrowserWindow
                    {
                        Text = "Fire Browser"
                    }
                }
            );



            if (FireBrowser.Properties.Settings.Default.SetupDone == false)
            {

                BrowserSetup myForm = new BrowserSetup();
                myForm.TopLevel = true;
                myForm.Show();

            }
            else
            {
                // Set initial tab the first one

            }

            container.SelectedTabIndex = 0;

            container.WindowState = FormWindowState.Maximized;

            // Create tabs and start application
            FireTitleApplicationContext applicationContext = new FireTitleApplicationContext();
            applicationContext.Start(container);
            Application.Run(applicationContext);
        }
    }
}
