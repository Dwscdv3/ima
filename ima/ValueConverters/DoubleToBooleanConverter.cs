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
using System.Windows;
using System.Windows.Data;

namespace Dwscdv3.ima.ValueConverters
{
    internal class DoubleToBooleanConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty ThresholdProperty =
            DependencyProperty.Register(
                nameof(Threshold),
                typeof(double),
                typeof(DoubleToBooleanConverter),
                new PropertyMetadata(0.5));
        
        public static readonly DependencyProperty IsInvertedProperty =
            DependencyProperty.Register(
                nameof(IsInverted),
                typeof(bool),
                typeof(DoubleToBooleanConverter),
                new PropertyMetadata(false));
        
        public double Threshold
        {
            get => (double)GetValue(ThresholdProperty);
            set => SetValue(ThresholdProperty, value);
        }

        public bool IsInverted
        {
            get => (bool)GetValue(IsInvertedProperty);
            set => SetValue(IsInvertedProperty, value);
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (global::System.Convert.ToDouble(value) >= Threshold) ^ IsInverted;

        // Not implemented
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => 0.0;
    }
}
