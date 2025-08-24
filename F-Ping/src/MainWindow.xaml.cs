
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Core;
using Microsoft.Win32; // Needed for Registry (Start with Windows)
using System;
using System.Diagnostics;
using System.IO;
using System.IO;
using System.Net.NetworkInformation; // Needed for Ping
using System.Runtime.InteropServices; // Needed for Host Object
using System.Threading.Tasks;       // Needed for Task
using System.Windows;
using System.Windows.Controls;

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

   
        private CoreWebView2 _webView2;
        string _Uri = "http://127.0.0.1:5500/index.html";


        public MainWindow()
        {
            InitializeComponent();
            InitializeAsync();
     
            webView.CoreWebView2InitializationCompleted += OnCoreWebView2InitializationCompleted;
        }


        private async Task InitializeAsync()
        {

            // 1. --- Configuration ---
            // The name of the Node.js executable.
            // If "node" doesn't work, provide the full path, e.g., @"C:\Program Files\nodejs\node.exe"
            string nodeExecutable = "node";

            // The path to the JavaScript file. We assume it's in the same directory as the C# executable.
            string jsScriptFileName = "host.js";
            string jsScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ,"node", jsScriptFileName);

            // Arguments to pass to the Node.js script.
            string scriptArguments = "arg1 arg2 \"a third argument with spaces\"";

            Console.WriteLine("--- Starting Node.js Process ---");
            Console.WriteLine($"Looking for JavaScript file at: {jsScriptPath}");

            // 2. --- File Path Check ---
            if (!File.Exists(jsScriptPath))
            {
                Console.WriteLine($"Error: The JavaScript file was not found at {jsScriptPath}.");
                Console.WriteLine("Make sure it is in the same folder as your C# executable.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            try
            {
                // 3. --- Process Setup ---
                string fullArguments = $"{jsScriptPath} {scriptArguments}";
                Console.WriteLine($"Attempting to run: {nodeExecutable} {fullArguments}");

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = nodeExecutable,
                    Arguments = fullArguments,
                    RedirectStandardOutput = true, // Capture standard output
                    RedirectStandardError = true,  // Capture standard error
                    UseShellExecute = false,       // Must be false to redirect streams
                    CreateNoWindow = true          // Don't show a command window
                };

                // 4. --- Execute and Read Output ---
                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    string errors = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    // 5. --- Display Results ---
                    Console.WriteLine("--- Node.js Script Output ---");
                    Console.WriteLine(output);
                    Console.WriteLine("-----------------------------");

                    if (!string.IsNullOrEmpty(errors))
                    {
                        Console.WriteLine("!!! Node.js Script Errors !!!");
                        Console.WriteLine(errors);
                        Console.WriteLine("-----------------------------");
                    }

                    Console.WriteLine($"Node.js process exited with code: {process.ExitCode}");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: The '{nodeExecutable}' executable was not found.");
                Console.WriteLine("Make sure Node.js is installed and in your system's PATH.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        


        var options = new CoreWebView2EnvironmentOptions("--allow-file-access-from-files");

            // Create the environment with the options
            var environment = await CoreWebView2Environment.CreateAsync(null, null, options);

            // Ensure CoreWebView2 with the created environment
            await webView.EnsureCoreWebView2Async(environment);  
        }

        private async void  OnCoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                _webView2 = webView.CoreWebView2;


                webView.CoreWebView2.AddHostObjectToScript("controller", new HostController());

                // a few configs so the app feels more real

                webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
                webView.CoreWebView2.Settings.IsPinchZoomEnabled = false;
                webView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
                webView.CoreWebView2.Settings.IsZoomControlEnabled = false;



                webView.Source = new Uri (_Uri);

            }
            else
            {
                MessageBox.Show("WebView2 initialization failed.");
            }
        }



        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
    }


