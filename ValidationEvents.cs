using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KcptunManager
{
    public partial class MainWindow : Window
    {
        private void EndpointValidation(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            var text = textBox?.Text;

            if (string.IsNullOrWhiteSpace(text))
            {
                textBox?.ClearValue(TextBox.BackgroundProperty);
                return;
            }

            var endpointRule = new Regex("([0-9]{0,3}.[0-9]{0,3}.[0-9]{0,3}.[0-9]{0,3}):([0-9]{1,5})");

            if (endpointRule.IsMatch(text))
            {
                var matches = endpointRule.Matches(text);

                if (matches[0].Groups.Count < 2)
                {
                    textBox.Background = new SolidColorBrush(Color.FromArgb(203, 255, 0, 0));
                }

                // matches[0].Groups[0], endpoint 192.168.1.1:1
                // matches[0].Groups[1], ip address 192.168.1.1
                // matches[0].Groups[2], ip port 1

                var portValue = matches[0].Groups[2].Value;
                if (!int.TryParse(portValue, out var port))
                {
                    textBox.Background = new SolidColorBrush(Color.FromArgb(203, 255, 0, 0));
                }

                if (port > 0 && port < 65536)
                {
                    textBox.Background = new SolidColorBrush(Color.FromArgb(203, 0, 255, 0));
                }
                else
                {
                    textBox.Background = new SolidColorBrush(Color.FromArgb(203, 255, 0, 0));
                }
            }
            else
            {
                textBox.Background = new SolidColorBrush(Color.FromArgb(203, 255, 0, 0));
            }
        }

        private void NumericValidation(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            var text = textBox?.Text;

            if (string.IsNullOrWhiteSpace(text))
            {
                textBox?.ClearValue(TextBox.BackgroundProperty);
                return;
            }

            var nonDigitalRule = new Regex("\\D");
            if (nonDigitalRule.IsMatch(text))
            {
                textBox.Background = new SolidColorBrush(Color.FromArgb(203, 255, 0, 0));
            }
            else
            {
                textBox.Background = new SolidColorBrush(Color.FromArgb(203, 0, 255, 0));
            }
        }
    }
}