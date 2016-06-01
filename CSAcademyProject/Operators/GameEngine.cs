using CSAcademyProject.Drawables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CSAcademyProject
{
    enum GameControlType { MAIN_GRID, BLOCK_LIST, NO_ELEMENT }
    enum NotificationMessage { BLOCK_IS_SELECTED, BLOCK_IS_PLACED, REFRESH_BOARD, UPDATE_POINTS, GAME_OVER }

    class GameEngine
    {
        public Canvas DrawingArea { get; }
        public Canvas WindowContent { get; }
        public Label PointsLabel { get; }
        public MainGridOperator MainGrid { get; private set; }
        public BlockListOperator BlockList { get; private set; }

        public DrawableBlock CurrentSelectedBlock { get; private set; }
        public int CursorPositionX;
        public int CursorPositionY;
        public int CurrentPoints { get; private set; }
        public bool GameOver { get; private set; }

        private static Action EmptyDelegate = delegate () { };

        public GameEngine(Canvas window, Canvas drawingArea, Label pointsLabel)
        {
            DrawingArea = drawingArea;
            PointsLabel = pointsLabel;
            WindowContent = window;
            StartNewGame();
        }

        public void Notify(NotificationMessage message, Object args)
        {
            if (message == NotificationMessage.BLOCK_IS_SELECTED)
            {
                CurrentSelectedBlock = (DrawableBlock)args;
            }
            else if (message == NotificationMessage.BLOCK_IS_PLACED)
            {
                BlockList.RemoveCurrentBlock();
                CurrentSelectedBlock = null;
                Draw(false);
            }
            else if (message == NotificationMessage.REFRESH_BOARD)
            {
                Draw(true);
            }
            else if (message == NotificationMessage.UPDATE_POINTS)
            {
                CurrentPoints += (int)args;
                PointsLabel.Content = "Points: " + CurrentPoints;
                WindowContent.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
                Canvas.SetLeft(PointsLabel, (WindowParameters.WIDTH - PointsLabel.DesiredSize.Width) / 2);
            }
            else if (message == NotificationMessage.GAME_OVER)
            {
                GameOver = true;
            }
        }


        private void HandlePlayAgainButtonClick(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        private void StartNewGame()
        {
            MainGrid = new MainGridOperator(this, (WindowParameters.WIDTH - MainGridOperator.WIDTH) / 2, 20);
            BlockList = new BlockListOperator(this, (WindowParameters.WIDTH - BlockListOperator.WIDTH) / 2, 450);
            CurrentPoints = 0;
            PointsLabel.Content = "Points:0";
            GameOver = false;
            Draw(false);
        }

        public void HandleMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (GameOver == true)
                return;

            int positionX = (int)e.GetPosition(DrawingArea).X;
            int positionY = (int)e.GetPosition(DrawingArea).Y;

            if (GetControlType(positionX, positionY) == GameControlType.BLOCK_LIST)
            {
                BlockList.HandleMouseDown(positionX - BlockList.PositionX, positionY - BlockList.PositionY);
                Draw(false);
            }
        }

        public void HandleMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (GameOver == true)
                return;

            int positionX = (int)e.GetPosition(DrawingArea).X;
            int positionY = (int)e.GetPosition(DrawingArea).Y;

            if (GetControlType(positionX, positionY) == GameControlType.MAIN_GRID)
            {
                MainGrid.HandleMouseUp(positionX - MainGrid.PositionX, positionY - MainGrid.PositionY);
            }
            BlockList.RestoreCurrentBlock();
            CurrentSelectedBlock = null;
            Draw(false);
        }

        public void HandleMouseMove(object sender, MouseEventArgs e)
        {
            if (GameOver == true)
                return;
            CursorPositionX = (int)e.GetPosition(DrawingArea).X;
            CursorPositionY = (int)e.GetPosition(DrawingArea).Y;

            if (CurrentSelectedBlock != null)
                Draw(false);
        }

        public DrawableBlock[] GetBlockList()
        {
            return BlockList.CurrentBlocks;
        }

        private GameControlType GetControlType(int x, int y)
        {
            if ((x >= MainGrid.PositionX && x < MainGrid.PositionX + MainGridOperator.WIDTH)
                && (y >= MainGrid.PositionY && y < MainGrid.PositionY + MainGridOperator.HEIGHT))
            {
                return GameControlType.MAIN_GRID;
            }
            else if ((x >= BlockList.PositionX && x < BlockList.PositionX + BlockListOperator.WIDTH)
                && (y >= BlockList.PositionY && y < BlockList.PositionY + BlockListOperator.HEIGHT))
            {
                return GameControlType.BLOCK_LIST;
            }

            return GameControlType.NO_ELEMENT;
        }



        public void Draw(bool instantDraw)
        {
            DrawingArea.Children.Clear();

            MainGrid.Draw(DrawingArea);
            BlockList.Draw(DrawingArea);

            if (CurrentSelectedBlock != null)
            {
                DisplayCurrentBlock();
            }
            if (GameOver == true)
            {
                DisplayGameOverMessage();
            }

            if (instantDraw == true)
                DrawingArea.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }

        private void DisplayGameOverMessage()
        {
            DrawableEndMessage endMessage = new DrawableEndMessage("Game Over\nScore:" + CurrentPoints, "Try Again", this.HandlePlayAgainButtonClick);
            UIElement drawableEndMessage = endMessage.GetDrawable();

            Canvas.SetLeft(drawableEndMessage, (WindowParameters.WIDTH - DrawableEndMessage.WIDTH) / 2);
            Canvas.SetTop(drawableEndMessage, (WindowParameters.HEIGHT - DrawableEndMessage.HEIGHT) / 2 - 100);

            DrawingArea.Children.Add(drawableEndMessage);
        }

        private void DisplayCurrentBlock()
        {
            UIElement drawableBlock = CurrentSelectedBlock.GetDrawable();
            int startX = 0;
            for (startX = 0; startX < CurrentSelectedBlock.Structure[0].Length; startX++)
                if (CurrentSelectedBlock.Structure[0][startX] == true)
                    break;
            startX *= MainGridOperator.ELEMENT_WIDTH;
            Canvas.SetTop(drawableBlock, CursorPositionY - MainGridOperator.ELEMENT_HEIGHT / 2);
            Canvas.SetLeft(drawableBlock, CursorPositionX - startX - MainGridOperator.ELEMENT_WIDTH / 2);
            drawableBlock.IsEnabled = false;
            DrawingArea.Children.Add(drawableBlock);
        }

    }
}
