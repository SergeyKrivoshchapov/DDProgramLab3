using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Lab4Timp.Converters
{
    /// <summary>
    /// Конвертирует bool в Visibility. 
    /// true -> Visible, false -> Collapsed (или Hidden, если указано).
    /// Поддерживает инверсию (Inverse=true).
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Если true, то false становится Visible, а true — Collapsed/Hidden.
        /// </summary>
        public bool Inverse { get; set; }

        /// <summary>
        /// Если true, то вместо Collapsed используется Hidden (сохраняет макет).
        /// </summary>
        public bool UseHiddenInsteadOfCollapsed { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is bool b)
                flag = b;

            if (Inverse)
                flag = !flag;

            if (flag)
                return Visibility.Visible;
            else
                return UseHiddenInsteadOfCollapsed ? Visibility.Hidden : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                bool result = visibility == Visibility.Visible;
                if (Inverse)
                    result = !result;
                return result;
            }
            return false;
        }
    }
}