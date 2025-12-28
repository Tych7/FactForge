
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Tmds.DBus.Protocol;

namespace DesktopApp
{
    public static class CreateSlideOverviewElements
    {
        public static Button CreateSlideElement(int slideId, string content, int slideTypeIndex)
        {
            var button = new Button
            {
                Content = content,
                Classes = { "neon-text-button" },
                Margin = new Thickness(10,10,10,0),
                Height = 80,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Foreground = new SolidColorBrush(Color.Parse("#8C52FF")),
            };

            button.Click += (_, _) =>
            {
                // Save previous slide
                ModifyQuizHandler.Instance.WriteNewQuestionData();
                
                // Show new slide
                ModifyQuizHandler.Instance.SelectSlide(slideId, slideTypeIndex);
            };

            ModifyQuizHandler.Instance.overviewButtons[slideId] = button;
            return button;
        }

        public static Button CreateInsertElement()
        {
            var addIcon = new PathIcon
            {
                Data = AppHandler.GetIcon("add_regular"),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Height = 20,
                Width = 20
            };

            Button button = new Button
            {
                Height = 50,
                Width = 50,
                Classes = {"neon-icon-button"},
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BorderThickness = new Thickness(3),
                CornerRadius = new CornerRadius(30),
                Content = new Border { Child = addIcon },                
                Margin = new Thickness(0,10,0,0)
            };

            button.Click += (_, _) =>
            {

            };

            return button;
        }
    }
}