using System;
using System.IO;
using System.Windows;

namespace PS4GSCInjector
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            base.OnStartup(e);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            WriteCrashLog(e.Exception);
            MessageBox.Show(e.Exception.ToString(), "Application Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            WriteCrashLog(e.ExceptionObject as Exception);
        }

        private static void WriteCrashLog(Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            try
            {
                File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "crash.log"), exception.ToString());
            }
            catch
            {
            }
        }
    }
}
