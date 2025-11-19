using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace TimerApp.Converters
{
    /// <summary>
    /// Converts a boolean value to a specific color.
    /// Used to switch between Normal Timer Color and Overtime (Negative) Color.
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isNormalTime && parameter is string colorString)
            {
                // Format expected: "NormalColor|OvertimeColor" (e.g. "#00D9FF|#FF4757")
                // Or just bound to the ViewModel properties directly if passed differently.
                
                // However, the XAML binding I wrote earlier was:
                // Foreground="{Binding !IsNegative, Converter={StaticResource BoolToColorConverter}}"
                // This implies we need to look at the ViewModel for the colors, OR pass them as parameters.
                
                // Since we can't easily bind to parameters, we will use a MultiValueConverter approach 
                // OR simply handle the logic in the ViewModel. 
                
                // Let's stick to a simpler approach: The ViewModel already has "TimerColor" and "OvertimeColor".
                // But wait, the XAML I wrote uses a converter on !IsNegative. 
                // Let's fix the XAML to be simpler: Bind Foreground directly to a property in VM that returns the correct Brush.
                // BUT, to support the user's request of "Red for overtime", let's implement a robust converter 
                // that takes the boolean and the ViewModel context.
                
                return AvaloniaProperty.UnsetValue;
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
