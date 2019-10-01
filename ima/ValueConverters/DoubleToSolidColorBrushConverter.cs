/**
 * Copyright (C) 2019  Dwscdv3 <dwscdv3@hotmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Dwscdv3.ima.ValueConverters
{
    internal class DoubleToSolidColorBrushConverter : DependencyObject, IValueConverter
    {
        private readonly PropertyInfo[] Channels =
            new string[] { "ScR", "ScG", "ScB" }
            .Select(propName => typeof(Color).GetProperty(propName))
            .ToArray();

        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register(
                nameof(From),
                typeof(Color),
                typeof(DoubleToSolidColorBrushConverter),
                new PropertyMetadata(new Color()));
        
        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register(
                nameof(To),
                typeof(Color),
                typeof(DoubleToSolidColorBrushConverter),
                new PropertyMetadata(new Color()));
        
        public Color From
        {
            get => (Color)GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        public Color To
        {
            get => (Color)GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            new SolidColorBrush(From + Color.Multiply(To - From, global::System.Convert.ToSingle(value)));

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var channel = GetMostDifferencedChannel();
            var current = (float)channel.GetValue(((SolidColorBrush)value).Color);
            var min = (float)channel.GetValue(From);
            var max = (float)channel.GetValue(To);
            return (current - min) / (max - min);
        }

        private PropertyInfo GetMostDifferencedChannel() =>
            Channels
            .OrderByDescending(prop => Math.Abs((float)prop.GetValue(To) - (float)prop.GetValue(From)))
            .First();
    }
}
