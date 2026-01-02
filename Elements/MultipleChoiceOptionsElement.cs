using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace DesktopApp
{
    public static class MultipleChoiceOptionsElement
    {
        private static readonly List<string> OptionIds = ["A", "B", "C", "D"];

        public static (Grid optionsGrid, List<TextBox> optionInputs) Create(
            List<string> currentOptions,
            List<string> optionColors,
            string correctAnswer,
            string optionCount,
            int height)
        {
            var inputs = new List<TextBox>();

            var optionsGrid = new Grid
            {
                Height = height,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(40,0,40,40)
            };

            // Columns are always 2
            optionsGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            optionsGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

            // Rows depend on option count
            if (int.Parse(optionCount) == 2)
            {
                optionsGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            }
            else
            {
                optionsGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                optionsGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            }

            for (int i = 0; i < int.Parse(optionCount); i++)
            {
                var inputField = CreateOptionField(
                    currentOptions[i],
                    optionColors[i],
                    OptionIds[i],
                    correctAnswer,
                    out var textBox);

                if (int.Parse(optionCount) == 2)
                {
                    // One row, side by side
                    Grid.SetRow(inputField, 0);
                    Grid.SetColumn(inputField, i);
                }
                else
                {
                    // Normal 2x2 layout
                    Grid.SetRow(inputField, i / 2);
                    Grid.SetColumn(inputField, i % 2);
                }

                optionsGrid.Children.Add(inputField);
                inputs.Add(textBox);
            }


            return (optionsGrid, inputs);
        }

        private static Border CreateOptionField(
            string option,
            string color,
            string id,
            string correctAnswer,
            out TextBox textBox)
        {
            var border = new Border
            {
                Margin = new Thickness(20),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                BorderBrush = new SolidColorBrush(Color.Parse(color)),
                Background = new SolidColorBrush(Color.Parse("#90000000")),
                CornerRadius = new CornerRadius(10),
                BorderThickness = new Thickness(5),
                BoxShadow = new BoxShadows(
                    new BoxShadow
                    {
                        Color = Color.Parse(color),
                        Blur = 25,
                        Spread = 5,
                        OffsetX = 0,
                        OffsetY = 0
                    }
                )
            };

            var optionFieldGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            var textGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Star)
                },
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(20)
            };

            var idText = new TextBlock
            {
                Text = $"{id} -",
                Margin = new Thickness(20, 0, 10, 0),
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = new SolidColorBrush(Color.Parse(color)),
                FontWeight = FontWeight.Bold,
                FontSize = 70,
                Effect = new DropShadowEffect
                {
                    Color = Color.Parse(color),
                    BlurRadius = 15,
                    OffsetX = 0,
                    OffsetY = 0,
                    Opacity = 0.8
                }
            };

            textBox = new TextBox
            {
                Text = option,
                Watermark = $"Option {id} here...",
                BorderBrush = Brushes.Transparent,
                Background = Brushes.Transparent,
                Foreground = new SolidColorBrush(Color.Parse(color)),
                TextWrapping = TextWrapping.NoWrap,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                FontWeight = FontWeight.Regular,
                AcceptsReturn = false,
                Effect = new DropShadowEffect
                {
                    Color = Color.Parse(color),
                    BlurRadius = 15,
                    OffsetX = 0,
                    OffsetY = 0,
                    Opacity = 0.8
                }
            };

            ElementHander.AutoFitTextBox(textBox, false);

            textGrid.Children.Add(idText);
            textGrid.Children.Add(textBox);
            Grid.SetColumn(textBox, 1);
            optionFieldGrid.Children.Add(textGrid);

            if(option == correctAnswer && option != "") optionFieldGrid.Children.Add(CreateCheckMark());

            border.Child = optionFieldGrid;
            return border;
        }

        private static Border CreateCheckMark()
        {
            var checkMarkIcon = new PathIcon
            {
                Data = AppHandler.GetIcon("checkmark_circle_regular"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Height = 30,
                Width = 30,
                Foreground = new SolidColorBrush(Color.Parse("#00FF00"))
            };

            var checkMark = new Border
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5,5,0,0),
                Child = checkMarkIcon,
                Effect = new DropShadowEffect
                {
                    Color = Color.Parse("#00FF00"),
                    BlurRadius = 30,
                    Opacity = 1,
                    OffsetX = 0,
                    OffsetY = 0,
                }
            };
            
            return checkMark;
        }
    }
}
