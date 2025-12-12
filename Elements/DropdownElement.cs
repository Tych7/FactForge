using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media;

namespace DesktopApp
{
    public static class DropdownElement
    {
        public static Border Create( IEnumerable<string> options, string defaultValue, int width, int height, Action<string> onSelect)
        {
            Border elementBorder = new Border
            {
                Height = height,
                Width = width,
                BorderBrush = new SolidColorBrush(Color.Parse("#00FFFF")),
            };

            Grid mainGrid = new Grid{};

            ComboBox dropdown = new ComboBox
            {
                SelectedItem = defaultValue,
                ItemsSource = options,
                Width = width,
                Height = height,
                Classes = { "neon-dropdown" },
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            };

            dropdown.SizeChanged += (_, e) =>
            {
                dropdown.FontSize = e.NewSize.Height * 0.5;
            };

            dropdown.SelectionChanged += (_, __) =>
            {
                if (dropdown.SelectedItem is string selected)
                    onSelect?.Invoke(selected);
            };

            mainGrid.Children.Add(dropdown);

            elementBorder.Child = mainGrid;

            return elementBorder;
        }
    }
}
