using Microsoft.Web.WebView2.Core;
using Microsoft.Win32; // Needed for Registry (Start with Windows)
using System;
using System.IO;
using System.Net.NetworkInformation; // Needed for Ping
using System.Runtime.InteropServices; // Needed for Host Object
using System.Threading.Tasks;       // Needed for Task
using System.Windows;

namespace FPing_V2
{
    // This class will be exposed to JavaScript.
    // Methods inside must be marked with [JSExport]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class HostController
    {
        // This C# method will be callable from JavaScript to ping a server.
        [return: MarshalAs(UnmanagedType.FunctionPtr)]
        public async Task<long> PingServer(string address)
        {
            try
            {
                using (var pinger = new Ping())
                {
                    var reply = await pinger.SendPingAsync(address, 1000); // 1-second timeout
                    return reply.Status == IPStatus.Success ? reply.RoundtripTime : -1;
                }
            }
            catch
            {
                return -1; // Return -1 on failure
            }
        }

        public void SetStartup(bool startup)
        {
            try
            {
                string appName = "MyApp"; // Should match your assembly name
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if (startup)
                {
                    // Use AppDomain.CurrentDomain.BaseDirectory for the path to your .exe
                    string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, appName + ".exe");
                    rk.SetValue(appName, $"\"{exePath}\"");
                }
                else
                {
                    rk.DeleteValue(appName, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update startup settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }


    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            var options = new CoreWebView2EnvironmentOptions("--allow-file-access-from-files");
            var environment = await CoreWebView2Environment.CreateAsync(null, null, options);



            await webView.EnsureCoreWebView2Async(environment);

            // This is the bridge between C# and JavaScript
            webView.CoreWebView2.AddHostObjectToScript("controller", new HostController());

            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string indexPath = Path.Combine(appDirectory, "public" , "launcher" , "index.html");

            if (File.Exists(indexPath))
            {
                webView.CoreWebView2.Navigate(indexPath);
            }
            else
            {
                MessageBox.Show($"Error: Cannot find index.html at path: {indexPath}", "File Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}