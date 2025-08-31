using System.Globalization;

namespace MauiAuthPageTemplate.Converters;

public class PinCircleFillConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string pin && int.TryParse(parameter?.ToString(), out int circleIndex))
        {
            // circleIndex начинается с 1, а индексы строк в pin с 0
            return pin.Length >= circleIndex ? Colors.SlateGray : Colors.Transparent;
        }
        return Colors.Transparent; // Цвет по умолчанию, если значение некорректно
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
