using System;
using System.Globalization;
using System.Windows.Data;

namespace CourseSelectionSystem
{
    // 用于将多个绑定值传递给一个命令参数
    public class MultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // 直接返回所有值，ViewModel 将其解析为 object[]
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}