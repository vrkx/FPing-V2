using FPing_V2.Animations;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FPing_V2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string SelectedTheme = "Default";
        private bool isGridOneVisible = true;
        TimeSpan minimumLoadingDisplayTime = TimeSpan.FromSeconds(3);
        public MainWindow()
        {
            InitializeComponent();

          
            InitializeApplicationFlow();

        }




        private async void InitializeApplicationFlow()
        {
              await Task.Delay(minimumLoadingDisplayTime);

            bool hasCompletedSetup = Data.Default.SetupDone;
           

            if (!hasCompletedSetup)
            {
                // Scenario 1: First-time user or initial setup not completed
                Console.WriteLine("Initial setup not completed. Showing welcome screen.");
                ShowWelcomeScreen();
            }
        
            else
            {
                // Scenario 3: Both initial setup and theme selection have been handled
                Console.WriteLine("Setup and theme selection handled. Loading launcher.");
                LoadLauncher();
                
            }
        }


        private void ShowWelcomeScreen()
        {

            Loading.FadeOut();
            
            WBanner.FadeInAndBounceUp();
        
        }

        private void LoadLauncher()
        {
            WelocmS.Content = "Welcome!" + " " + Data.Default.UserName;

            if (Data.Default.Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase))
            {

                this.Background = new SolidColorBrush(Colors.Black);
                this.Foreground = new SolidColorBrush(Colors.White);
                LauncherBG.Background = new SolidColorBrush(Colors.Black);

            }
            else if (Data.Default.Theme.Equals("White", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Applying Light theme specific adjustments...");
                this.Background = new SolidColorBrush(Colors.LightGray);
                LauncherBG.Background = new SolidColorBrush(Colors.LightGray);
                this.Foreground = new SolidColorBrush(Colors.Black);
            }
            else if (Data.Default.Theme.Equals("Default", StringComparison.OrdinalIgnoreCase))
            {
                string hexColorCode = "#151520"; // Red (FF RRGGBB)
                System.Windows.Media.Color DefaultColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(hexColorCode);

                SolidColorBrush myBrush = new SolidColorBrush(DefaultColor);

                this.Background = myBrush;
                LauncherBG.Background = myBrush;
                this.Foreground = new SolidColorBrush(Colors.White);
            }

            Launcher.Visibility = Visibility.Visible;

        }

        private void SwitchViews_Click(object sender, RoutedEventArgs e)
        {
            if (isGridOneVisible)
            {
                // Current state: Grid One is visible, Grid Two is hidden
                // Action: Fade out Grid One, Fade in Grid Two with bounce

                // 1. Fade out gridOne
                WBanner.FadeOut(0.5, () =>
                {

                    WBanner.Visibility = Visibility.Collapsed;
                });

                // 2. Fade in gridTwo with bounce
                // It's important to make gridTwo visible *before* animating it if it was collapsed.
                // Our FadeInAndBounceUp method already handles this by setting Visibility.Visible.
                Options.FadeInAndBounceUp(initialOffsetY: -50, durationInSeconds: 1.0, delayInSeconds: 0.1); // Small delay for sequential feel

                isGridOneVisible = false; // Update state
            }
        }
           

        private void Continue1_Click(object sender, RoutedEventArgs e)
        {
            WBanner.FadeOut();
       
            Options.FadeInAndBounceUp();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            SelectedTheme = "White";
            White.BorderBrush = new SolidColorBrush(Colors.LightGreen);
            Black.BorderBrush = new SolidColorBrush(Colors.Gray);
            Defaukt.BorderBrush = new SolidColorBrush(Colors.Gray);


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SelectedTheme = "Default";
            White.BorderBrush = new SolidColorBrush(Colors.Gray);
            Black.BorderBrush = new SolidColorBrush(Colors.Gray);
            Defaukt.BorderBrush = new SolidColorBrush(Colors.LightGreen);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SelectedTheme = "Black";
            White.BorderBrush = new SolidColorBrush(Colors.Gray);
            Black.BorderBrush = new SolidColorBrush(Colors.LightGreen);
            Defaukt.BorderBrush = new SolidColorBrush(Colors.Gray);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            Data.Default["Theme"] = SelectedTheme;
            Data.Default.Save();
            Options.FadeOut();
            NameIn.Visibility = Visibility.Visible;
            Options.Visibility = Visibility.Hidden;
            NameIn.FadeInAndBounceUp();


        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

            NameIn.FadeOut();
            Loading.FadeInAndBounceUp();
            NameIn.Visibility = Visibility.Hidden;

            Data.Default["Theme"] = SelectedTheme;
            Data.Default["UserName"] = Inpu.Text;
            Data.Default.SetupDone = true;
            Data.Default.Save();

            WelocmS.Content = "Welcome!" + " " + Inpu.Text;

            if (Data.Default.Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase))
            {

                this.Background = new SolidColorBrush(Colors.Black);
                this.Foreground = new SolidColorBrush(Colors.White);
                LauncherBG.Background = new SolidColorBrush(Colors.Black);

            }
            else if (Data.Default.Theme.Equals("White", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Applying Light theme specific adjustments...");
                this.Background = new SolidColorBrush(Colors.LightGray);
                LauncherBG.Background = new SolidColorBrush(Colors.LightGray);
                this.Foreground = new SolidColorBrush(Colors.Black);
            }
            else if (Data.Default.Theme.Equals("Default", StringComparison.OrdinalIgnoreCase))
            {
                string hexColorCode = "#151520"; // Red (FF RRGGBB)
                System.Windows.Media.Color DefaultColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(hexColorCode);

                SolidColorBrush myBrush = new SolidColorBrush(DefaultColor);

                this.Background = myBrush ;
                LauncherBG.Background = myBrush;
                this.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {


                Home.Opacity = 0;
            Settings.Opacity = 0;

            Servers.FadeIn();
            Servers.Opacity = 1;
            
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
        
            
                Servers.Opacity = 0;
            Settings.Opacity = 0;
            Home.FadeIn();
            Home.Opacity = 1;

            


        }

        private void WhiteTH_Click(object sender, RoutedEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.LightGray);
            LauncherBG.Background = new SolidColorBrush(Colors.LightGray);
            this.Foreground = new SolidColorBrush(Colors.Black);

            Data.Default.Theme = "White";
            Data.Default.Save();

        }

        private void Deff_Click(object sender, RoutedEventArgs e)
        {
            string hexColorCode = "#151520"; // Red (FF RRGGBB)
            System.Windows.Media.Color DefaultColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(hexColorCode);

            SolidColorBrush myBrush = new SolidColorBrush(DefaultColor);

            this.Background = myBrush;
            LauncherBG.Background = myBrush;
            this.Foreground = new SolidColorBrush(Colors.White);

            Data.Default.Theme = "Default";
            Data.Default.Save();

        }

        private void Defsf_Click(object sender, RoutedEventArgs e)
        {

            this.Background = new SolidColorBrush(Colors.Black);
            this.Foreground = new SolidColorBrush(Colors.White);
            LauncherBG.Background = new SolidColorBrush(Colors.Black);

            Data.Default.Theme = "Black";
            Data.Default.Save();


        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {


            Home.Opacity = 0;
            Servers.Opacity = 0;

            Settings.FadeIn();
            Settings.Opacity = 1;

        }
    }
}