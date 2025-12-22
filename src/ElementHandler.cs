using Avalonia.Controls;
using Avalonia.Media;
using Avalonia;
using Avalonia.Layout;

namespace DesktopApp;

public static class ElementHander
{
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

            while (min <= max)
            {
                double mid = (min + max) / 2;

                var ft = new FormattedText(
                    string.IsNullOrWhiteSpace(textBox.Text) ? " " : textBox.Text,
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

        textBox.AttachedToVisualTree += (_, _) => Resize();
        textBox.SizeChanged += (_, _) => Resize();
        textBox.TextChanged += (_, _) => Resize();
    }
}
