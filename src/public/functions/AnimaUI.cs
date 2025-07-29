using System;
using System.Linq;
using System.Windows; 
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FPing_V2.Animations 
{
    public static class UIElementAnimations
    {
        /// <summary>
        /// Fades out a UI element.
        /// </summary>
        /// <param name="element">The UI element to fade out.</param>
        /// <param name="durationInSeconds">The duration of the fade-out animation in seconds.</param>
        /// <param name="onCompleted">An optional action to execute when the animation completes.</param>
        /// <param name="hideElementAfterFade">If true, sets Visibility.Collapsed after fade-out. Default is true.</param>
        // CHANGE: Change 'this UIElement element' to 'this FrameworkElement element'
        public static void FadeOut(this FrameworkElement element, double durationInSeconds = 0.1, Action onCompleted = null, bool hideElementAfterFade = true)
        {
            if (element == null) return;

            // Ensure the element is visible before starting to fade out
            element.Visibility = Visibility.Visible; // This is good to prevent issues if it was Collapsed

            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = element.Opacity,
                To = 0.0,
                Duration = new Duration(TimeSpan.FromSeconds(durationInSeconds)),
                // *** CRITICAL for staying faded out ***
                FillBehavior = FillBehavior.Stop
            };

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeOutAnimation);

            Storyboard.SetTarget(fadeOutAnimation, element);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath(UIElement.OpacityProperty));

            // Handle completion: either hide element, call user action, or both
            if (hideElementAfterFade || onCompleted != null)
            {
                storyboard.Completed += (s, e) =>
                {
                    if (hideElementAfterFade)
                    {
                        element.Visibility = Visibility.Collapsed; // *** Hides the element from layout ***
                    }
                    onCompleted?.Invoke(); // Call the user's provided action
                };
            }

            storyboard.Begin(element);
        }

        public static void FadeIn(this FrameworkElement element, double durationInSeconds = 0.1, Action onCompleted = null, bool hideElementAfterFade = true)
        {
            if (element == null) return;

            // Ensure the element is visible before starting to fade out
            element.Visibility = Visibility.Visible; // This is good to prevent issues if it was Collapsed

            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = element.Opacity,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromSeconds(durationInSeconds)),
                // *** CRITICAL for staying faded out ***
                FillBehavior = FillBehavior.Stop
            };

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeOutAnimation);

            Storyboard.SetTarget(fadeOutAnimation, element);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath(UIElement.OpacityProperty));

          

            storyboard.Begin(element);
        }

        /// <summary>
        /// Fades in a UI element with an upward bouncy animation.
        /// </summary>
        /// <param name="element">The UI element to animate.</param>
        /// <param name="initialOffsetY">The starting offset from the element's final Y position (e.g., -50 for 50 pixels up).</param>
        /// <param name="durationInSeconds">The duration of the animation in seconds.</param>
        /// <param name="delayInSeconds">An optional delay before the animation starts.</param>
        /// <param name="onCompleted">An optional action to execute when the animation completes.</param>
        // CHANGE: Change 'this UIElement element' to 'this FrameworkElement element'
        public static void FadeInAndBounceUp(this FrameworkElement element, double initialOffsetY = -50.0, double durationInSeconds = 0.1, double delayInSeconds = 0.0, Action onCompleted = null)
        {
            if (element == null) return;

            element.Visibility = Visibility.Visible; // Ensure element is visible before starting animation

            // Handle RenderTransform: Ensure it's a TransformGroup and has a TranslateTransform
            TransformGroup transformGroup;
            if (element.RenderTransform == null || !(element.RenderTransform is TransformGroup))
            {
                transformGroup = new TransformGroup();
                if (element.RenderTransform != null)
                {
                    transformGroup.Children.Add(element.RenderTransform);
                }
                element.RenderTransform = transformGroup;
            }
            else
            {
                transformGroup = element.RenderTransform as TransformGroup;
            }

            TranslateTransform translateTransform = transformGroup.Children.OfType<TranslateTransform>().FirstOrDefault();
            if (translateTransform == null)
            {
                translateTransform = new TranslateTransform();
                transformGroup.Children.Add(translateTransform);
            }

            // Set initial state immediately to prevent flicker before animation starts
            element.Opacity = 0.0;
            translateTransform.Y = initialOffsetY;

            // 1. Opacity Animation (Fade In)
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromSeconds(durationInSeconds)),
                BeginTime = TimeSpan.FromSeconds(delayInSeconds),
                FillBehavior = FillBehavior.Stop
            };
            Storyboard.SetTarget(fadeInAnimation, element);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath(UIElement.OpacityProperty));

            // 2. TranslateTransform Animation (Bouncy Up)
            DoubleAnimation bounceUpAnimation = new DoubleAnimation
            {
                From = initialOffsetY,
                To = 0.0,
                Duration = new Duration(TimeSpan.FromSeconds(durationInSeconds)),
                BeginTime = TimeSpan.FromSeconds(delayInSeconds),
                FillBehavior = FillBehavior.Stop,
                EasingFunction = new BounceEase
                {
                    EasingMode = EasingMode.EaseIn,
                    Bounces = 3,
                    Bounciness = 3.0
                }
            };
            Storyboard.SetTarget(bounceUpAnimation, translateTransform);
            Storyboard.SetTargetProperty(bounceUpAnimation, new PropertyPath(TranslateTransform.YProperty));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeInAnimation);
            storyboard.Children.Add(bounceUpAnimation);

            if (onCompleted != null)
            {
                storyboard.Completed += (s, e) => onCompleted();
            }

            storyboard.Begin(element); // This will now work correctly
            element.Opacity = 1.0;
        }
    }
}