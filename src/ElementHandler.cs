using Avalonia.Controls;
using Avalonia.Media;
using Avalonia;
using Avalonia.Layout;
using System.Linq;
using static QuizSlide;
using System.Collections.Generic;
using Avalonia.Controls.ApplicationLifetimes;

namespace DesktopApp;

public static class ElementHander
{
    public static string? currentOpenQuizTitle;
    public static CurrentSlideData? currentData;
    public static int currentTime;
    public static List<TextBox>? currentAwnserOptions;
    public static string? currentCorrectAnswer;
    public static string? currentAmountOfAnswers;
    public static Grid? currentMultipleChoiceGrid;
    public static TextBox? currentHeaderText;
    public static TextBox? currentSubtextText;
    public static TextBox? currentCorrectAnswerTextBox;
    public static ComboBox? currentCorrectAnswerDropDown;
    public static ComboBox? currentheaderTextSizeDropDown;
    public static ComboBox? currentsubTextSizeDropDown;

    public static (int headerTextSize, int subtextTextSize) textSizes = (150, 80);

    public static ComboBox OpenSlidePanelClick(QuizSlide slide)
    {
        StackPanel slideOptions = new StackPanel();
        ComboBox timeDropDown = new ComboBox();
        

        if(slide.Type == SlideTypes.Text)
        {
            //TEXT SIZES OPTION
            (StackPanel headerTextSizeOption, currentheaderTextSizeDropDown) = CreateHeaderTextSizePanelOption(slide);
            slideOptions.Children.Add(headerTextSizeOption);

            //TEXT SIZES OPTION
            (StackPanel subTextTextSizeOption, currentsubTextSizeDropDown) = CreateSubTextSizePanelOption(slide);
            slideOptions.Children.Add(subTextTextSizeOption);
        }
        else
        {
            //TIME OPTIONS
            (StackPanel timeOption, timeDropDown) = CreateTimePanelOption(slide);
            slideOptions.Children.Add(timeOption);

            //CORRECT AWNSER OPTIONS
            StackPanel correctAwnserOption = CreateCorrectAwnserPanelOption(slide);
            slideOptions.Children.Add(correctAwnserOption);

            if (slide.Type == SlideTypes.MultipleChoiceQuestion)
            {
                //Amount of Options
                StackPanel optionCountOption = CreateOptionCountPanelOption(slide);
                slideOptions.Children.Add(optionCountOption);   
            }
        }
        

        var mainWindow = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow as MainWindow;

        if (mainWindow != null)
        {
            var optionPanel = SlidingPanelElement.Create(mainWindow.ModalLayerContainer, slideOptions, "OPTIONS", 500);
            if (optionPanel != null)
            {
                mainWindow.ModalLayerContainer.IsHitTestVisible = true;
                mainWindow.ModalLayerContainer.Children.Add(optionPanel);
            }
        }

        return timeDropDown;
    }

    private static (StackPanel textSizeOption, ComboBox textSizeDropDown) CreateHeaderTextSizePanelOption(QuizSlide slide)
    {
        StackPanel textSizeOption = new StackPanel{Orientation = Orientation.Vertical, VerticalAlignment = VerticalAlignment.Stretch, Margin = new Thickness(0,20,0,0)};
        TextBlock textSizeTitle = new TextBlock {Text = "Header text size: ", Classes = {"neon-text"}, FontSize = 40,  VerticalAlignment= VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left}; textSizeOption.Children.Add(textSizeTitle);

        var textSizeOptions = Enumerable.Range(1, 200).Select(i => i.ToString()).ToList();

        (Border textSizeDropDownElement, ComboBox textSizeDropDown) = DropdownElement.Create(textSizeOptions, textSizes.headerTextSize.ToString() ?? "", 50); textSizeOption.Children.Add(textSizeDropDownElement);
        textSizeDropDownElement.Margin = new Thickness(0,10,0,0);
        textSizeDropDown.SelectionChanged += (_, __) =>
        {
            UpdateTextSizesOptions(slide);
        };

        return (textSizeOption, textSizeDropDown);
    }

    private static (StackPanel textSizeOption, ComboBox textSizeDropDown) CreateSubTextSizePanelOption(QuizSlide slide)
    {
        StackPanel textSizeOption = new StackPanel{Orientation = Orientation.Vertical, VerticalAlignment = VerticalAlignment.Stretch, Margin = new Thickness(0,20,0,0)};
        TextBlock textSizeTitle = new TextBlock {Text = "Subtext text size: ", Classes = {"neon-text"}, FontSize = 40,  VerticalAlignment= VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left}; textSizeOption.Children.Add(textSizeTitle);

        var textSizeOptions = Enumerable.Range(1, 200).Select(i => i.ToString()).ToList();

        (Border textSizeDropDownElement, ComboBox textSizeDropDown) = DropdownElement.Create(textSizeOptions, textSizes.subtextTextSize.ToString() ?? "", 50); textSizeOption.Children.Add(textSizeDropDownElement);
        textSizeDropDownElement.Margin = new Thickness(0,10,0,0);
        textSizeDropDown.SelectionChanged += (_, __) =>
        {
            UpdateTextSizesOptions(slide);
        };

        return (textSizeOption, textSizeDropDown);
    }

    private static (StackPanel timeOption, ComboBox timeDropDown) CreateTimePanelOption(QuizSlide slide)
    {
        StackPanel timeOption = new StackPanel{Orientation = Orientation.Vertical, VerticalAlignment = VerticalAlignment.Stretch, Margin = new Thickness(0,20,0,0)};
        TextBlock timeTitle = new TextBlock {Text = "Time: ", Classes = {"neon-text"}, FontSize = 40,  VerticalAlignment= VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left}; timeOption.Children.Add(timeTitle);

        var timeOptions = Enumerable.Range(1, 100).Select(i => i.ToString()).ToList();
        currentTime = slide.Time ?? 0;

        (Border timeDropDownElement, ComboBox timeDropDown) = DropdownElement.Create(timeOptions, currentTime.ToString() ?? "", 50); timeOption.Children.Add(timeDropDownElement);
        timeDropDownElement.Margin = new Thickness(0,10,0,0);
        timeDropDown.SelectionChanged += (_, __) =>
        {
            if (timeDropDown.SelectedItem is string selected && int.TryParse(selected, out int value))
            {
                currentTime = value;
            }
        };

        return (timeOption, timeDropDown);
    }

    private static StackPanel CreateCorrectAwnserPanelOption(QuizSlide slide)
    {
        StackPanel correctAwnserOption = new StackPanel{Orientation = Orientation.Vertical, VerticalAlignment = VerticalAlignment.Stretch, Margin = new Thickness(0,20,0,0)};
        TextBlock awnserTitle = new TextBlock {Text = "Correct Answer: ", Classes = {"neon-text"}, FontSize = 40,  VerticalAlignment= VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left}; correctAwnserOption.Children.Add(awnserTitle);
        
        if (slide.Type == SlideTypes.MultipleChoiceQuestion)
        {
            List<string> options = [];
            foreach(var option in currentAwnserOptions ?? []) {options.Add(option.Text ?? "");}
            (Border awnserDropDownElement, currentCorrectAnswerDropDown) = DropdownElement.Create(options ?? [], currentCorrectAnswer ?? "", 50); correctAwnserOption.Children.Add(awnserDropDownElement);
            awnserDropDownElement.Margin = new Thickness(0,10,0,0);
            currentCorrectAnswerDropDown.SelectionChanged += (_, __) =>
            {
                if (currentCorrectAnswerDropDown.SelectedItem is string selected)
                {
                    currentCorrectAnswer = selected;
                    UpdateMultipleChoiceOptions(slide);
                }
            };
        }
        else if(slide.Type == SlideTypes.OpenQuestion)
        {
            currentCorrectAnswerTextBox = new TextBox { Classes = {"neon-input"}, Watermark = $"Anwser here", Text = currentCorrectAnswer, HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(20,0,20,0)};
            currentCorrectAnswerTextBox.TextChanged += (_, __) =>
            {
                currentCorrectAnswer = currentCorrectAnswerTextBox.Text;
            };
            
            correctAwnserOption.Children.Add(currentCorrectAnswerTextBox);
        }

        return correctAwnserOption;
    }

    private static StackPanel CreateOptionCountPanelOption(QuizSlide slide)
    {
        StackPanel optionCountOption = new StackPanel{Orientation = Orientation.Vertical, VerticalAlignment = VerticalAlignment.Stretch, Margin = new Thickness(0,20,0,0)};
        ComboBox optionCountDropDown = new ComboBox();
        
        TextBlock optionCOuntTitle = new TextBlock {Text = "Amount of Options: ", Classes = {"neon-text"}, FontSize = 40,  VerticalAlignment= VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left}; optionCountOption.Children.Add(optionCOuntTitle); 
        List<string> options = ["2", "4"];
        (Border optionCountDropDownElement, optionCountDropDown) = DropdownElement.Create(options ?? [], currentAmountOfAnswers ?? "", 50); optionCountOption.Children.Add(optionCountDropDownElement);
        optionCountDropDownElement.Margin = new Thickness(0,10,0,0);
        optionCountDropDown.SelectionChanged += (_, __) =>
        {
            if (optionCountDropDown.SelectedItem is string selected)
            {
                currentAmountOfAnswers = selected;

                UpdateMultipleChoiceOptions(slide);
                UpdateCorrectAnswerDropDown();
            }
        };
        
        return optionCountOption;
    }

    public static void AutoFitTextBox(TextBox textBox, bool wrapText)
    {
        textBox.TextWrapping = wrapText ? TextWrapping.Wrap : TextWrapping.NoWrap;

        // Horizontal + vertical centering
        textBox.TextAlignment = TextAlignment.Center;
        textBox.VerticalContentAlignment = VerticalAlignment.Center;

        void Resize()
        {
            if (textBox.Bounds.Width <= 0 || textBox.Bounds.Height <= 0)
                return;

            double min = 10;
            double max = 200;
            double best = min;

            double availableWidth = textBox.Bounds.Width - 20;
            double availableHeight = textBox.Bounds.Height - 20;

            string measureText =
                !string.IsNullOrWhiteSpace(textBox.Text)
                    ? textBox.Text
                    : textBox.Watermark?.ToString() ?? " ";

            while (min <= max)
            {
                double mid = (min + max) / 2;

                var ft = new FormattedText(
                    measureText,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(textBox.FontFamily),
                    mid,
                    Brushes.Black
                )
                {
                    TextAlignment = TextAlignment.Center
                };

                if (wrapText)
                {
                    ft.MaxTextWidth = availableWidth;
                }

                if (ft.Width <= availableWidth &&
                    ft.Height <= availableHeight)
                {
                    best = mid;
                    min = mid + 0.5;
                }
                else
                {
                    max = mid - 0.5;
                }
            }

            textBox.FontSize = best;
        }

        textBox.AcceptsReturn = wrapText;
        textBox.AttachedToVisualTree += (_, _) => Resize();
        textBox.SizeChanged += (_, _) => Resize();
        textBox.TextChanged += (_, _) => Resize();
    }

    private static void UpdateTextSizesOptions(QuizSlide slide)
    {
        if (currentHeaderText == null && currentSubtextText == null)
            return;

        // Header text size
        if (currentheaderTextSizeDropDown?.SelectedItem is string headerSelected &&
            int.TryParse(headerSelected, out int headerSize))
        {
            textSizes.headerTextSize = headerSize;

            if (currentHeaderText != null)
            {
                currentHeaderText.FontSize = headerSize;
            }
        }

        // Subtext text size
        if (currentsubTextSizeDropDown?.SelectedItem is string subSelected &&
            int.TryParse(subSelected, out int subSize))
        {
            textSizes.subtextTextSize = subSize;

            if (currentSubtextText != null)
            {
                currentSubtextText.FontSize = subSize;
            }
        }

        // Optional: persist to slide if needed
        if(slide.Header != null) slide.Header.FontSize = textSizes.headerTextSize;
        if(slide.SubText != null) slide.SubText.FontSize = textSizes.subtextTextSize;
    }


    private static void UpdateMultipleChoiceOptions(QuizSlide slide)
    {
        if (currentMultipleChoiceGrid == null) return;

        var parent = currentMultipleChoiceGrid.Parent as Panel;
        parent?.Children.Remove(currentMultipleChoiceGrid);

        int count = int.Parse(currentAmountOfAnswers ?? "2");

        slide.Answers ??= new List<string>();

        while (slide.Answers.Count < count)
            slide.Answers.Add("");

        while (slide.Answers.Count > count)
            slide.Answers.RemoveAt(slide.Answers.Count - 1);

        List<string> currentOptions = [];
        int oldOptionCount = 0;
        if (currentAwnserOptions != null) oldOptionCount = currentAwnserOptions.Count();
        int newOptionCount = int.Parse(currentAmountOfAnswers ?? "");

        for(int i = 0; i < newOptionCount; i++)
        {
            if(currentAwnserOptions != null)
            {
                if(newOptionCount > oldOptionCount)
                {
                    if(newOptionCount - oldOptionCount > i) currentOptions.Add(currentAwnserOptions[i].Text ?? "");
                    else currentOptions.Add("");
                }
                else currentOptions.Add(currentAwnserOptions[i].Text ?? "");
            }
        }

        (Grid newGrid, currentAwnserOptions) =
            MultipleChoiceOptionsElement.Create(
                currentOptions,
                ModifySlideElement.MultipleChoiceOptionColors,
                currentCorrectAnswer ?? "",
                currentAmountOfAnswers!,
                400
            );

        currentMultipleChoiceGrid = newGrid;
        parent?.Children.Add(newGrid);

        if (currentData != null)
        {
            currentData.Answers = currentAwnserOptions;
        }
    }

    private static void UpdateCorrectAnswerDropDown()
    {
        if (currentCorrectAnswerDropDown == null || currentAwnserOptions == null)
            return;

        var options = currentAwnserOptions
            .Select(t => t.Text ?? "")
            .ToList();

        currentCorrectAnswerDropDown.ItemsSource = options;

        if (options.Contains(currentCorrectAnswer ?? ""))
        {
            currentCorrectAnswerDropDown.SelectedItem = currentCorrectAnswer;
        }
        else
        {
            currentCorrectAnswerDropDown.SelectedItem = null;
            currentCorrectAnswer = null;
        }
    }
}
