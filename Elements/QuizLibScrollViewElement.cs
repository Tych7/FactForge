using System;
using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace DesktopApp
{
    public static class QuizLibScrollViewElement
    {
        public static Grid Create(
            int width,
            ScrollViewer libScrollView,
            Action onTabOneClick,
            Action onTabTwoClick,
            Action onTabThreeClick)
        {
            const int SelectedLeft = 0;
            const int UnselectedLeft = 20;

            //LEFT SIDE
            Grid leftSide = new Grid
            {
                Width = width,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            Border scrollViewBorder = new Border
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = width - 100,
                Classes = { "neon-frame" },
                Child = libScrollView
            };
            leftSide.Children.Add(scrollViewBorder);

            Button tab1 = CreateTab("text_sort_ascending_regular", 20);
            Button tab2 = CreateTab("star_regular", 100);
            Button tab3 = CreateTab("clock_regular", 180);

            var tabs = new[] { tab1, tab2, tab3 };

            void SelectTab(Button selected)
            {
                foreach (var tab in tabs)
                {
                    var margin = tab.Margin;
                    tab.Margin = new Thickness(tab == selected ? SelectedLeft : UnselectedLeft, margin.Top, 0, 0);
                    tab.Width = tab == selected ? 88 + UnselectedLeft : 82;
                }
            }
            tab1.Click += (_, _) => { SelectTab(tab1); onTabOneClick?.Invoke(); };
            tab2.Click += (_, _) => { SelectTab(tab2); onTabTwoClick?.Invoke(); };
            tab3.Click += (_, _) => { SelectTab(tab3); onTabThreeClick?.Invoke(); };
            SelectTab(tab1); // Initial state 

            leftSide.Children.Add(tab1);
            leftSide.Children.Add(tab2);
            leftSide.Children.Add(tab3);

            return leftSide;
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
                Margin = new Thickness(10, 0, 0, 0)
            };

            return new Button
            {
                Height = 80,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Classes = { "neon-icon-button" },
                Content = new Border { Child = alphabetIcon },
                Margin = new Thickness(0, topMargin, 0, 0),
                BorderThickness = new Thickness(6,6,0,6),
                CornerRadius = new CornerRadius(10,0,0,10),
                Effect = new DropShadowEffect
                {
                    Color = Color.Parse("#00000000"),
                    BlurRadius = 0,
                    Opacity = 0,
                    OffsetX = 0,
                    OffsetY = 0
                }
            };
        }

        private static StackPanel CreateDetailsButtons(Action onDeleteClick, Action onEditClick, Action onStartClick)
        {
            StackPanel buttonUnit = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0,0,0,20),
                Spacing = 10
            };

            Button startButton = new Button
            {
                Content = "START",
                Height = 80,
                Width = 200,
                Classes = {"neon-text-button"},
                Foreground = SolidColorBrush.Parse("#8C52FF"),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
            };
            startButton.Click += (_, __) => onStartClick?.Invoke();
            buttonUnit.Children.Add(startButton);

            var editIcon = new PathIcon
            {
                Data = AppHandler.GetIcon("edit_regular"),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Height = 30,
                Width = 30
            };
            Button editButton = new Button
            {
                Height = 80,
                Width = 80,
                Classes = {"neon-icon-button"},
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Content = new Border { Child = editIcon },                
            };
            editButton.Click += (_, __) => onEditClick?.Invoke();
            buttonUnit.Children.Add(editButton);

            var deleteButtonIcon = new PathIcon
            {
                Data = AppHandler.GetIcon("delete_regular"),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Height = 30,
                Width = 30
            };
            Button deleteButton = new Button
            {
                Height = 80,
                Width = 80,
                Classes = {"neon-icon-button"},
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Content = new Border { Child = deleteButtonIcon },
            };
            deleteButton.Click += (_, __) => onDeleteClick?.Invoke();
            buttonUnit.Children.Add(deleteButton);

            return buttonUnit;
        }
    }
}