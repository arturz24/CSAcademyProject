using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSAcademyProject
{

    public partial class MainWindow : Window
    {
        private GameEngine gameOperator;

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            Label pointsLabel = new Label();
            pointsLabel.FontSize = 30;
            pointsLabel.FontFamily = new FontFamily("Candara");
            pointsLabel.Content = "Points: 0";

            pointsLabel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Canvas.SetLeft(pointsLabel, (WindowParameters.WIDTH  - pointsLabel.DesiredSize.Width) / 2);
            Canvas.SetTop(pointsLabel, 10);
           
            
            Canvas drawingArea = new Canvas();
            gameOperator = new GameEngine(windowContent,drawingArea,pointsLabel);

            drawingArea.Width = WindowParameters.WIDTH;
            drawingArea.Height = WindowParameters.HEIGHT;
            drawingArea.Background = new SolidColorBrush(Color.FromRgb(255,255,255));
            Canvas.SetTop(drawingArea, 50);
            drawingArea.MouseDown += gameOperator.HandleMouseDown;
            drawingArea.MouseUp += gameOperator.HandleMouseUp;
            drawingArea.MouseMove += gameOperator.HandleMouseMove;

            windowContent.Children.Add(pointsLabel);
            windowContent.Children.Add(drawingArea);
            
        }

    }
}
