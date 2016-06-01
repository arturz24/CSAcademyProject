using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CSAcademyProject
{
    class DrawableCell : IDrawable
    {
        public int SizeX { get; }
        public int SizeY { get; }
        public Color CellColor { get; }
        public SolidColorBrush CellSolidBrush { get; }
        public int Radius { get; }

        public DrawableCell(int sizeX, int sizeY,int radius,Color color)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            CellColor = color;
            CellSolidBrush = new SolidColorBrush(CellColor);
            Radius = radius;
        }

        public UIElement GetDrawable()
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Width = SizeX;
            rectangle.Height = SizeY;
            rectangle.RadiusX = Radius;
            rectangle.RadiusY = Radius;
            rectangle.Fill = CellSolidBrush;
            rectangle.IsEnabled = false;

            return rectangle;
        }

    }
}
    