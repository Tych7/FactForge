using Avalonia.Controls;


namespace DesktopApp
{
    public static class ModifySlideElement
    {
        public static Grid CreateOpenQuestionSlide(QuizSlide slide)
        {
            Grid mainGrid = new Grid
            {
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                Margin = new Avalonia.Thickness(20)
            };

            TextBox Header = new TextBox
            {
                FontSize = 40,
                Height = 150,
                Classes = { "neon-input" },
                Text = slide.Question,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
            };
            mainGrid.Children.Add(Header);

            return mainGrid;
        }

        public static Grid CreateMultipleChoiceQuestionSlide(QuizSlide slide)
        {
            Grid mainGrid = new Grid
            {
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                Margin = new Avalonia.Thickness(20)
            };

            TextBox Header = new TextBox
            {
                FontSize = 40,
                Height = 150,
                Classes = { "neon-input" },
                Text = slide.Question,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
            };
            mainGrid.Children.Add(Header);

            return mainGrid;
        }

        public static Grid CreateTextSlide(QuizSlide slide)
        {
            Grid mainGrid = new Grid
            {
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                Margin = new Avalonia.Thickness(20)
            };

            TextBox Header = new TextBox
            {
                FontSize = 40,
                Height = 150,
                Classes = { "neon-input" },
                Text = slide.Question,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
            };
            mainGrid.Children.Add(Header);

            return mainGrid;
        }

    }
}

