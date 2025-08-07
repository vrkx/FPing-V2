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
   
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class HostController
    {
    
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
                string appName = "FPing-V2"; 
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
            webView.Source = new Uri("https://vrkx.github.io/f-replicate/src/index");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
          
        }
    }
}