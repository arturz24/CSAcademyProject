using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CSAcademyProject.Drawables
{
    class DrawableBlock : IDrawable
    {
        public bool[][] Structure { get; }
        public Color BlockColor { get; }
        public SolidColorBrush BlockBrush { get; }
        public int SizeX { get; }
        public int SizeY { get; }
        public int Margin { get; }

        public DrawableBlock(bool[][] structure, int cellSizeX, int cellSizeY,int margin, Color color)
        {
            Structure = structure;
            BlockColor = color;
            BlockBrush = new SolidColorBrush(BlockColor);
            SizeX = cellSizeX;
            SizeY = cellSizeY;
            Margin = margin;
        }

        public UIElement GetDrawable()
        {
            Canvas canvas = new Canvas();

            for (int i = 0; i < Structure.Length; i++)
            {
                for (int j = 0; j < Structure[i].Length; j++)
                {
                    if (Structure[i][j] == true)
                    {
                        UIElement blockCell = (new DrawableCell(SizeX - 2*Margin, SizeY - 2*Margin, SizeX / 10, BlockColor)).GetDrawable();
                        Canvas.SetTop(blockCell, i * SizeX + Margin);
                        Canvas.SetLeft(blockCell, j * SizeY + Margin);
                        canvas.Children.Add(blockCell);
                    }
                }
            }
            canvas.IsEnabled = false;

            return canvas;
        }


    }
}
