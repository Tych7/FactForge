using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media;

namespace DesktopApp
{
    public static class DropdownElement
    {
        public static (Border dropDownElement, ComboBox dropDown) Create( IEnumerable<string> options, string defaultValue, int height)
        {
            Border elementBorder = new Border
            {
                Height = height,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                BorderBrush = new SolidColorBrush(Color.Parse("#00FFFF")),
            };

            ComboBox dropdown = new ComboBox
            {
                SelectedItem = defaultValue,
                ItemsSource = options,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                Classes = { "neon-dropdown" },
                FontSize = height * 0.5
            };

            elementBorder.Child = dropdown;

            return (elementBorder, dropdown);
        }
    }
}
