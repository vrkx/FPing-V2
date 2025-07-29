// ColorBrightnessConverter.cs
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FPing_V2 
{
    // This converter takes a Color and a brightness factor and returns a new Color
    public class ColorBrightnessConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2 || !(values[0] is Color) || !(values[1] is double))
            {
                return Colors.Transparent; // Or some default color
            }

            Color originalColor = (Color)values[0];
            double brightnessFactor = (double)values[1];

            // Calculate new R, G, B values
            byte newR = (byte)Math.Min(255, originalColor.R * brightnessFactor);
            byte newG = (byte)Math.Min(255, originalColor.G * brightnessFactor);
            byte newB = (byte)Math.Min(255, originalColor.B * brightnessFactor);

            return Color.FromArgb(originalColor.A, newR, newG, newB);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}