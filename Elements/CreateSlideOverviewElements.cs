using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using static QuizSlide;

namespace DesktopApp
{
    public static class CreateSlideOverviewElements
    {   
        public static event Action<int>? InsertSlide;
        public static Button CreateSlideElement(QuizSlide slide, string content, int slideTypeIndex)
        {
            var contentGrid = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Orientation = Orientation.Horizontal 
            };

            Border slideElementIcon = new();
            switch (slide.Type)
            {
                case SlideTypes.MultipleChoiceQuestion or SlideTypes.OpenQuestion:
                    slideElementIcon = CreateSlideElementIcon("question_regular", 40,40);
                    break;
                case SlideTypes.Text:
                    slideElementIcon = CreateSlideElementIcon("text_font_regular", 40,40);
                    break;
            }
            
            contentGrid.Children.Add(slideElementIcon);

            var slideElementText = new TextBlock
            {
                Text = content,
                Classes = {"neon-text"},
                FontSize = 35,
                VerticalAlignment = VerticalAlignment.Center,
            };
            contentGrid.Children.Add(slideElementText);

            var button = new Button
            {
                Content = contentGrid,
                Margin = new Thickness(10,10,10,0),
                Height = 80,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Foreground = new SolidColorBrush(Color.Parse("#8C52FF")),
                Classes = {"neon-empty-button"}
            };

            // left click
            button.Click += (_, _) =>
            {
                ModifyQuizHandler.Instance.WriteNewQuestionData();
                ModifyQuizHandler.Instance.SelectSlide(slide.Id, slideTypeIndex);
            };

            // --- Context menu ---
            var insertBefore = new MenuItem { Header = "Insert slide before", FontSize = 18 };
            insertBefore.Click += (_, _) =>
            {
                ModifyQuizHandler.Instance.WriteNewQuestionData();
                ModifyQuizHandler.Instance.SelectSlide(slide.Id, slideTypeIndex);
                if (InsertSlide != null && ModifyQuizHandler.Instance.currentSelectedSlide != null) InsertSlide.Invoke(ModifyQuizHandler.Instance.currentSelectedSlide.Id);
            };

            var insertAfter = new MenuItem { Header = "Insert slide after", FontSize = 18 };
            insertAfter.Click += (_, _) =>
            {
                ModifyQuizHandler.Instance.WriteNewQuestionData();
                ModifyQuizHandler.Instance.SelectSlide(slide.Id, slideTypeIndex);
                if (InsertSlide != null && ModifyQuizHandler.Instance.currentSelectedSlide != null) InsertSlide.Invoke(ModifyQuizHandler.Instance.currentSelectedSlide.Id + 1);
            };

            var contextMenu = new ContextMenu();
            contextMenu.Items.Add(insertBefore);
            contextMenu.Items.Add(insertAfter);

            // attach context menu to button
            button.ContextMenu = contextMenu;

            // Save button reference
            ModifyQuizHandler.Instance.overviewButtons[slide.Id] = button;

            return button;
        }

        private static Border CreateSlideElementIcon(string pathIconName, int height, int width)
        {   
            var slideIcon = new PathIcon
            {
                Data = AppHandler.GetIcon(pathIconName),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Height = height - 20,
                Width = width - 20,
                Foreground = new SolidColorBrush(Color.Parse("#8C52FF")),
            };

            var iconBorder = new Border
            {
                Height = height,
                Width = height,
                CornerRadius = new CornerRadius(height / 2),
                BorderThickness = new Thickness(2),
                BorderBrush = new SolidColorBrush(Color.Parse("#00FFFF")),
            };
            iconBorder.Child = slideIcon;

            return iconBorder;
        }
    }
}