﻿using CSAcademyProject.Drawables;
using CSAcademyProject.Loaders;
using CSAcademyProject.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CSAcademyProject
{
    class BlockListOperator : AbstractOperator
    {
        public const int BLOCK_SLOTS = 3;
        public const int BLOCK_SLOT_WIDTH = 120;
        public const int BLOCK_SLOT_HEIGHT = 120;

        public const int WIDTH = BLOCK_SLOT_WIDTH * BLOCK_SLOTS;
        public const int HEIGHT = BLOCK_SLOT_HEIGHT;

        public int PositionX { get; }
        public int PositionY { get; }
        public DrawableBlock[] CurrentBlocks { get; }
        public bool[] CurrentBlokcsAvailability { get; private set; }

        public GameEngine RefToGameEngine { get; }

        public BlockListOperator(GameEngine gameOperator, int positionX, int positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
            CurrentBlocks = new DrawableBlock[BLOCK_SLOTS];
            CurrentBlokcsAvailability = new bool[BLOCK_SLOTS];
            ChooseCurrentBlocks();
            
            RefToGameEngine = gameOperator;
        }

        public void RemoveCurrentBlock()
        {
            for (int i = 0; i < BLOCK_SLOTS; i++)
            {
                if (CurrentBlokcsAvailability[i] == false && CurrentBlocks[i] != null)
                    CurrentBlocks[i] = null;
            }

            //checking if this is last block
            for (int i = 0; i < BLOCK_SLOTS; i++)
            {
                if (CurrentBlocks[i] != null)
                    return;
            }
            ChooseCurrentBlocks();
        }

        public void RestoreCurrentBlock()
        {
            for (int i = 0; i < BLOCK_SLOTS; i++)
            {
                if (CurrentBlokcsAvailability[i] == false && CurrentBlocks[i] != null)
                    CurrentBlokcsAvailability[i] = true;
            }
        }

        private void ChooseCurrentBlocks()
        {
            for (int i = 0; i < BLOCK_SLOTS; i++)
            {
                CurrentBlocks[i] = BlockLoader.Instance.GetRandomBlock(20, 20,1);
                CurrentBlokcsAvailability[i] = true;
            }
        }


        public override void Draw(Canvas drawingArea)
        {
            UIElement grid = new DrawableGrid(1, BLOCK_SLOTS, BLOCK_SLOT_WIDTH, BLOCK_SLOT_HEIGHT, ColorLoader.Instance.Black).GetDrawable();

            Canvas.SetLeft(grid, PositionX);
            Canvas.SetTop(grid, PositionY);
            drawingArea.Children.Add(grid);

            for (int i = 0; i < BLOCK_SLOTS; i++)
            {
                if (CurrentBlokcsAvailability[i] == false)
                    continue;

                int startX = (BLOCK_SLOT_WIDTH - CurrentBlocks[i].Structure[0].Length * CurrentBlocks[i].SizeX) / 2;
                int startY = (BLOCK_SLOT_WIDTH - CurrentBlocks[i].Structure.Length * CurrentBlocks[i].SizeY) / 2;

                UIElement drawableBlock = CurrentBlocks[i].GetDrawable();
                Canvas.SetLeft(drawableBlock, PositionX + startX + i * BLOCK_SLOT_WIDTH);
                Canvas.SetTop(drawableBlock, startY + PositionY);

                drawingArea.Children.Add(drawableBlock);
            }

        }

        public override void HandleMouseDown(int x, int y)
        {
            for (int i = 0; i < BLOCK_SLOTS; i++)
            {
                if (CurrentBlokcsAvailability[i] == false)
                    continue;

                if (x>=i* BLOCK_SLOT_WIDTH && x < (i + 1) * BLOCK_SLOT_WIDTH)
                {
                    RefToGameEngine.Notify(NotificationMessage.BLOCK_IS_SELECTED,
                        new DrawableBlock(CurrentBlocks[i].Structure, MainGridOperator.ELEMENT_WIDTH,
                        MainGridOperator.ELEMENT_HEIGHT, MainGridOperator.BLOCK_MARGIN, CurrentBlocks[i].BlockColor));
                    CurrentBlokcsAvailability[i] = false;
                    break;
                }
            }
        }

        public override void HandleMouseUp(int x, int y)
        {
            //throw new NotImplementedException();
        }

    }
}
