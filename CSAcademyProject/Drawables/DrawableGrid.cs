using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CSAcademyProject
{
    class DrawableGrid : IDrawable
    {
        public int RowsCount { get; }
        public int ColumnCount { get; }
        public int ElementWidth { get; }
        public int ElementHeight { get; }
        public Color GridColor { get; }

        public SolidColorBrush GridBrush { get; }
        public Canvas Grid { get; private set; }

        public DrawableGrid(int rowCount, int columnCount, int elementWidth,
            int elementHeight, Color gridColor)
        {
            RowsCount = rowCount;
            ColumnCount = columnCount;
            ElementHeight = elementHeight;
            ElementWidth = elementWidth;
            GridColor = gridColor;
            GridBrush = new SolidColorBrush(GridColor);
            Grid = new Canvas();
            Grid.IsEnabled = false;
            InitializeLines();
        }

        private void InitializeLines()
        {
            for (int i = 0; i <= ColumnCount; i++)
            {
                Line line = new Line();
                line.Stroke = GridBrush;
                line.X1 = line.X2 = i * ElementWidth;
                line.Y1 = 0;
                line.Y2 = RowsCount * ElementWidth;
                line.IsEnabled = false;
                Grid.Children.Add(line);
            }

            for (int i = 0; i <= RowsCount; i++)
            {
                Line line = new Line();
                line.Stroke = GridBrush;
                line.X1 = 0;
                line.X2 = ColumnCount * ElementWidth;
                line.Y1 = line.Y2 = i * ElementHeight;
                line.IsEnabled = false;
                Grid.Children.Add(line);
            }

        }

        public UIElement GetDrawable()
        {
            return Grid;
        }
    }
}
