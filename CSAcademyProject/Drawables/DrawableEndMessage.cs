using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CSAcademyProject.Drawables
{
    class DrawableEndMessage : IDrawable
    {

        public static int WIDTH = 320;
        public static int HEIGHT = 200;

        public string Message { get; }
        public RoutedEventHandler ButtonHandler { get; }
        public string ButtonText;

        public DrawableEndMessage(string message, string buttonText, RoutedEventHandler buttonHandler)
        {
            Message = message;
            ButtonHandler = buttonHandler;
            ButtonText = buttonText;
        }

        public UIElement GetDrawable()
        {
            Canvas messageBox = new Canvas();
            messageBox.Width = WIDTH;
            messageBox.Height = HEIGHT;
            messageBox.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            
            TextBlock gameOverLabel = new TextBlock();
            gameOverLabel.FontSize = 28;
            gameOverLabel.FontFamily = new FontFamily("Candara");
            gameOverLabel.Text = Message;// "Game Over" + Environment.NewLine + "Your Score:" + CurrentPoints;
            gameOverLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            gameOverLabel.TextAlignment = TextAlignment.Center;

            gameOverLabel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Canvas.SetLeft(gameOverLabel, (messageBox.Width - gameOverLabel.DesiredSize.Width) / 2);
            Canvas.SetTop(gameOverLabel, 10);

            Button playAgainButton = new Button();
            playAgainButton.Content = ButtonText;
            playAgainButton.FontSize = 28;
            playAgainButton.Width = 200;
            playAgainButton.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            playAgainButton.FontFamily = new FontFamily("Candara");

            playAgainButton.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Canvas.SetLeft(playAgainButton, (messageBox.Width - playAgainButton.DesiredSize.Width) / 2);
            Canvas.SetTop(playAgainButton, 130);
            playAgainButton.Click += ButtonHandler; 

            messageBox.Children.Add(gameOverLabel);
            messageBox.Children.Add(playAgainButton);

            return messageBox;
        }
    }
}
