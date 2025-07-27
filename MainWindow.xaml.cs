using FPing_V2.Animations;
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
        private bool isGridOneVisible = true;

        public MainWindow()
        {
            InitializeComponent();
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

        }
    }
}