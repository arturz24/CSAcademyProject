using CSAcademyProject.Loaders;
using CSAcademyProject.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CSAcademyProject
{
    class MainGridOperator : AbstractOperator
    {
        public const int ROW_NUMBER = 10;
        public const int COLUMN_NUMBER = 10;
        public const int ELEMENT_WIDTH = 40;
        public const int ELEMENT_HEIGHT = 40;
        
        public const int WIDTH = COLUMN_NUMBER * ELEMENT_WIDTH;
        public const int HEIGHT = ROW_NUMBER * ELEMENT_HEIGHT;
        public const int BLOCK_MARGIN = 2;

        public DrawableGrid Grid { get; }
        public DrawableCell[][] Cells { get; private set; }
        public int PositionX { get; }
        public int PositionY { get; }

        public GameEngine RefToGameEngine { get; }

        public MainGridOperator(GameEngine gameOperator, int positionX, int positionY)
        {
            Grid = new DrawableGrid(ROW_NUMBER, COLUMN_NUMBER, ELEMENT_WIDTH,
                ELEMENT_HEIGHT, ColorLoader.Instance.Black);

            SetCells();

            PositionX = positionX;
            PositionY = positionY;
            RefToGameEngine = gameOperator;
        }

        public void SetCells()
        {
            Cells = new DrawableCell[ROW_NUMBER][];
            for (int i = 0; i < ROW_NUMBER; i++)
                Cells[i] = new DrawableCell[COLUMN_NUMBER];
        }

        public override void Draw(Canvas drawingArea)
        {
            UIElement drawableGrid = Grid.GetDrawable();
            Canvas.SetLeft(drawableGrid, PositionX);
            Canvas.SetTop(drawableGrid, PositionY);

            for (int i = 0; i < ROW_NUMBER; i++)
            {
                for (int j = 0; j < COLUMN_NUMBER; j++)
                {
                    if (Cells[i][j] != null)
                    {
                        UIElement drawableCell = Cells[i][j].GetDrawable();
                        Canvas.SetLeft(drawableCell, j * ELEMENT_WIDTH + PositionX + BLOCK_MARGIN);
                        Canvas.SetTop(drawableCell, i * ELEMENT_HEIGHT + PositionY + BLOCK_MARGIN);
                        drawingArea.Children.Add(drawableCell);
                    }
                }
            }

            drawingArea.Children.Add(drawableGrid);
        }

        public override void HandleMouseDown(int x, int y)
        {
            //throw new NotImplementedException();
        }

        public override void HandleMouseUp(int x, int y)
        {
            int startX = x / ELEMENT_WIDTH;
            int startY = y / ELEMENT_HEIGHT;
            if (RefToGameEngine.CurrentSelectedBlock != null)
            {
                if (GameEvaluator.CanBlockBePlaced(Cells, startX, startY, RefToGameEngine.CurrentSelectedBlock) == true)
                {
                    PlaceSelectedBlock(x / ELEMENT_WIDTH, y / ELEMENT_HEIGHT);
                    int points = GameEvaluator.GetBlockPoints(RefToGameEngine.CurrentSelectedBlock);
                    RefToGameEngine.Notify(NotificationMessage.BLOCK_IS_PLACED, null);
                    LinesToRemove linesToRemove = GameEvaluator.GetLinesToRemove(Cells, ROW_NUMBER, COLUMN_NUMBER);
                    points = points + GameEvaluator.EvaluateGrid(RefToGameEngine, Cells, linesToRemove);
                    RefToGameEngine.Notify(NotificationMessage.UPDATE_POINTS, points);

                    if (GameEvaluator.IsMoveLeft(Cells, COLUMN_NUMBER, ROW_NUMBER, RefToGameEngine.GetBlockList()) == false)
                        RefToGameEngine.Notify(NotificationMessage.GAME_OVER, null);
                }
            }
        }


        private void PlaceSelectedBlock(int startX, int startY)
        {
            bool[][] structure = RefToGameEngine.CurrentSelectedBlock.Structure;
            int blockStartX = 0;
            for (blockStartX = 0; blockStartX < structure[0].Length; blockStartX++)
                if (structure[0][blockStartX] == true)
                    break;

            startX = startX - blockStartX;

            for (int i = 0; i < structure.Length; i++)
            {
                for (int j = 0; j < structure[i].Length; j++)
                {
                    if (structure[i][j] == true)
                        Cells[startY + i][startX + j] = new DrawableCell(ELEMENT_WIDTH - 2 * BLOCK_MARGIN,
                            ELEMENT_HEIGHT - 2 * BLOCK_MARGIN, ELEMENT_WIDTH / 10,
                            RefToGameEngine.CurrentSelectedBlock.BlockColor);
                }
            }
        }

    }
}

