using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace DesktopApp
{
    public static class SlidingPanelElement
    {
        private static int transitionDuration = 300;
        public static Border Create(Grid parentGrid, StackPanel scrollFeatureContent, string header, int width)
        {
            var translate = new TranslateTransform
            {
                X = width,
                Transitions = new Transitions
                {
                    new DoubleTransition
                    {
                        Property = TranslateTransform.XProperty,
                        Duration = TimeSpan.FromMilliseconds(transitionDuration),
                        Easing = new CubicEaseOut()
                    }
                }
            };


            Border elementBorder = new Border
            {
                Classes = { "neon-frame" },
                Height = 1080,
                Width = width,
                BorderBrush = new SolidColorBrush(Color.Parse("#00FFFF")),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                BorderThickness = new Thickness(6,0,0,0),
                CornerRadius = new CornerRadius(0),

                RenderTransform = translate,
                RenderTransformOrigin = new RelativePoint(1, 0.5, RelativeUnit.Relative),
            };

            Grid mainGrid = new Grid{};

            TextBlock Header = new TextBlock
            {
                Text = header,
                Classes = {"neon-header"},
                FontSize = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0,10,0,0)
            };
            mainGrid.Children.Add(Header);

            var optionsFame = new Border
            {
                Classes = { "neon-frame" },
                Height = 1080 - 220,
                Width = width - 40,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0,0,0,30)
            };

            var scrollFeature = new ScrollViewer
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Content = scrollFeatureContent
            };
            
            optionsFame.Child = scrollFeature;
            mainGrid.Children.Add(optionsFame);


            var closeButton = AddCloseButton("Close", width - 40);
            closeButton.Click += async (sender, e) =>
            {
                 translate.X = width;
                await Task.Delay(transitionDuration);
                parentGrid.Children.Remove(elementBorder);
            };
            mainGrid.Children.Add(closeButton);

            elementBorder.Child = mainGrid;

            elementBorder.AttachedToVisualTree += (_, _) =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    translate.X = 0;
                });
            };


            return elementBorder;
        }

        private static Button AddCloseButton(string content, double width)
        {
            return new Button
            {
                Content = content,
                Classes = { "neon-text-button" },
                Margin = new Thickness(0, 0, 0, 20),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Width = width,
                Height = 70,
                Foreground= new SolidColorBrush(Color.Parse("#8C52FF"))
            };
        }
    }
}
