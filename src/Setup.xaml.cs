// SetupControl.xaml.cs
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media; // Needed for Colors

namespace FPing_V2 // <--- IMPORTANT: Ensure this namespace matches your project's
{
    public partial class Setup : UserControl
    {
        // Define an event that the main window can subscribe to
        public event EventHandler SetupCompleted;

        private const string DefaultNamePlaceholder = "Hi! I'm...";

        public Setup()
        {
            InitializeComponent();
            NameTextBox.Text = DefaultNamePlaceholder; // Set initial placeholder
            NameTextBox.Foreground = (SolidColorBrush)FindResource("SubTextColor"); // Set initial color
            CheckNameInput(); // Check initial state of Next button
        }

        // Logic for the TextBox placeholder
        private void NameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text == DefaultNamePlaceholder)
            {
                NameTextBox.Text = "";
                NameTextBox.Foreground = (SolidColorBrush)FindResource("TextColor");
            }
        }

        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                NameTextBox.Text = DefaultNamePlaceholder;
                NameTextBox.Foreground = (SolidColorBrush)FindResource("SubTextColor");
            }
        }

        // Enable/disable the Next button based on text input
        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckNameInput();
        }

        private void CheckNameInput()
        {
            // Enable Next button if text is not placeholder and not empty/whitespace
            NextButton.IsEnabled = !string.IsNullOrWhiteSpace(NameTextBox.Text) && NameTextBox.Text != DefaultNamePlaceholder;
        }


        // Click handlers for the buttons
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // In a multi-step setup, this would navigate to the previous step.
            // For a single step, you might just close the app or show a message.
            MessageBox.Show("Going back is not implemented for this single-step setup.", "Back", MessageBoxButton.OK, MessageBoxImage.Information);
            // Optionally: Application.Current.Shutdown();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            // In a real scenario, you'd save the name:
            string userName = NameTextBox.Text;
            MessageBox.Show($"Hello, {userName}! Starting the launcher...", "Welcome", MessageBoxButton.OK, MessageBoxImage.Information);

            // Raise the event to notify the parent window that setup is complete
            SetupCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}