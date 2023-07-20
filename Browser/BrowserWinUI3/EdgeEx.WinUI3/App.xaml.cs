using EdgeEx.WinUI3.Helpers;
using EdgeEx.WinUI3.Interfaces;
using EdgeEx.WinUI3.Pages;
using EdgeEx.WinUI3.Toolkits;
using EdgeEx.WinUI3.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Serilog;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EdgeEx.WinUI3
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Database Path
        /// </summary>
        public static string DatabasePath { get; } = System.IO.Path.Combine(ApplicationData.Current.LocalFolder.Path, "EdgeEx.WinUI3.sqlite");
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Services = ConfigureServices();
            InitLogger();
            InitDatabase();
            this.InitializeComponent();
            
        }
        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<ISqlSugarClient>(s =>
            {
                SqlSugarScope sqlSugar = new SqlSugarScope(new ConnectionConfig()
                {
                    DbType = SqlSugar.DbType.Sqlite,
                    ConnectionString = $"DataSource={DatabasePath}",
                    IsAutoCloseConnection = true,
                },
               db =>
               {
                   //单例参数配置，所有上下文生效
                   db.Aop.OnLogExecuting = (string sql, SugarParameter[] pars) =>
                   {
                       Log.Debug(sql);
                   };
               });
                return sqlSugar;
            });
            services.AddSingleton<ICallerToolkit, CallerToolkit>();
            services.AddSingleton<LocalSettingsToolkit>();
            services.AddSingleton<ResourceToolkit>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<SettingsViewModel>();
            return services.BuildServiceProvider();
        }
        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            WindowHelper.MainWindow = m_window;
            WindowHelper.TrackWindow(m_window);
            Frame frame = new Frame();
            m_window.PersistenceId = Guid.NewGuid().ToString("N");
            frame.Navigate(typeof(MainPage), new Uri("EdgeEx://NewTab/"));
            m_window.Content = frame;
            m_window.Activate();
        }

        /// <summary>
        /// Init DataBase
        /// </summary>
        public static void InitDatabase()
        {
            ISqlSugarClient db = App.Current.Services.GetService<ISqlSugarClient>();
            db.DbMaintenance.CreateDatabase();
            // db.CodeFirst.InitTables(typeof(CodeFirstTable1), typeof(CodeFirstTable2));
        }
        /// <summary>
        /// Init Serilog Logger
        /// </summary>
        public static void InitLogger()
        {
            string logPath = System.IO.Path.Combine(ApplicationData.Current.LocalFolder.Path, "Logs");
            if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.File(System.IO.Path.Combine(logPath, "EdgeEx.WinUI3.log"), outputTemplate: "{Timestamp:MM-dd HH:mm:ss.fff} [{Level:u4}] {SourceContext} | {Message:lj} {Exception}{NewLine}", rollingInterval: RollingInterval.Day, shared: true)
               .CreateLogger();
        }
        private WindowEx m_window;
    }
}
