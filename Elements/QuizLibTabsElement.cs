using System;
using Avalonia.Controls;
using Avalonia.Layout;

namespace DesktopApp
{
    public static class QuizLibTabsElement
    {
        public static Grid Create(
            int height,
            int width,
            ScrollViewer libScrollView,
            Action onTabOneClick,
            Action onTabTwoClick,
            Action onTabThreeClick)
        {
            const int SelectedLeft = 0;
            const int UnselectedLeft = 20;

            Grid mainGrid = new Grid
            {
                Height = height,
                Width = width
            };

            Button tab1 = CreateTab("text_sort_ascending_regular", 20);
            Button tab2 = CreateTab("star_regular", 100);
            Button tab3 = CreateTab("clock_regular", 180);

            var tabs = new[] { tab1, tab2, tab3 };

            void SelectTab(Button selected)
            {
                foreach (var tab in tabs)
                {
                    var margin = tab.Margin;
                    tab.Margin = new Avalonia.Thickness(
                        tab == selected ? SelectedLeft : UnselectedLeft,
                        margin.Top,
                        0,
                        0
                    );
                }
            }

            tab1.Click += (_, _) =>
            {
                SelectTab(tab1);
                onTabOneClick?.Invoke();
            };

            tab2.Click += (_, _) =>
            {
                SelectTab(tab2);
                onTabTwoClick?.Invoke();
            };

            tab3.Click += (_, _) =>
            {
                SelectTab(tab3);
                onTabThreeClick?.Invoke();
            };

            // Initial state
            SelectTab(tab1);

            mainGrid.Children.Add(tab1);
            mainGrid.Children.Add(tab2);
            mainGrid.Children.Add(tab3);

            Border scrollViewBorder = new Border
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = 550,
                Classes = { "neon-frame" },
                Child = libScrollView
            };

            mainGrid.Children.Add(scrollViewBorder);

            return mainGrid;
        }


        private static Button CreateTab(string icon, int topMargin)
        {
            var alphabetIcon = new PathIcon
            {
                Data = AppHandler.GetIcon(icon),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Stretch,
                Height = 40,
                Width = 40,
                Margin = new Avalonia.Thickness(10, 0, 0, 0)
            };

            return new Button
            {
                Height = 80,
                Width = 150,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Classes = { "neon-icon-button" },
                Content = new Border { Child = alphabetIcon },
                Margin = new Avalonia.Thickness(0, topMargin, 0, 0)
            };
        }
    }
}