using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media;

namespace DesktopApp
{
    public static class DropdownElement
    {
        public static (Border dropDownElement, ComboBox dropDown) Create( IEnumerable<string> options, string defaultValue, int width, int height)
        {
            Border elementBorder = new Border
            {
                Height = height,
                Width = width,
                BorderBrush = new SolidColorBrush(Color.Parse("#00FFFF")),
            };

            ComboBox dropdown = new ComboBox
            {
                SelectedItem = defaultValue,
                ItemsSource = options,
                Width = width,
                Height = height,
                Classes = { "neon-dropdown" },
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                FontSize = height * 0.5
            };

            elementBorder.Child = dropdown;

            return (elementBorder, dropdown);
        }
    }
}
