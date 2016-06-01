using CSAcademyProject.Drawables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSAcademyProject
{

    class LinesToRemove
    {
        public List<int> RowIndexes { get;}
        public List<int> ColumnIndexes { get; }

        public LinesToRemove()
        {
            RowIndexes = new List<int>();
            ColumnIndexes = new List<int>();
        }
    }

    class GameEvaluator
    {
        public static LinesToRemove GetLinesToRemove(DrawableCell[][] gridCells, int sizeX, int sizeY)
        {
            LinesToRemove linesToRemove = new LinesToRemove();

            //getting rows
            for (int rowIndex = 0; rowIndex < sizeY; rowIndex++)
            {
                bool isSet = true;
                for (int j = 0; j < sizeX; j++)
                {
                    if (gridCells[rowIndex][j] == null)
                        isSet = false;
                }
                if (isSet == true)
                    linesToRemove.RowIndexes.Add(rowIndex);
            }

            //getting columns
            for (int columnIndex = 0; columnIndex < sizeX; columnIndex++)
            {
                bool isSet = true;
                for (int j = 0; j < sizeY; j++)
                {
                    if (gridCells[j][columnIndex] == null)
                        isSet = false;
                }
                if (isSet == true)
                    linesToRemove.ColumnIndexes.Add(columnIndex);
            }

            return linesToRemove;
        }


        public static bool IsMoveLeft(DrawableCell[][] gridCells, int sizeX, int sizeY, DrawableBlock[] availableBlocks)
        {
            for (int i = 0; i < availableBlocks.Length; i++)
            {
                if (availableBlocks[i] == null)
                    continue;

                for (int y = 0; y < sizeY; y++)
                {
                    for (int x = 0; x < sizeX; x++)
                    {
                        if (CanBlockBePlaced(gridCells, x, y, availableBlocks[i]) == true)
                            return true;
                    }
                }

            }

            return false;
        }

        public static bool CanBlockBePlaced(DrawableCell[][] gridCells, int startX, int startY, DrawableBlock block)
        {
            bool[][] structure = block.Structure;

            int blockStartX = 0;
            for (blockStartX = 0; blockStartX < structure[0].Length; blockStartX++)
                if (structure[0][blockStartX] == true)
                    break;

            startX = startX - blockStartX;

            for (int i = 0; i < structure.Length; i++)
            {
                for (int j = 0; j < structure[i].Length; j++)
                {
                    if (structure[i][j] == false)
                        continue;
                    if (gridCells.Length <= startY + i || gridCells[0].Length <= startX + j ||
                        startY + i < 0 || startX + j < 0)
                        return false;
                    if (gridCells[startY + i][startX + j] != null)
                        return false;
                }
            }
            return true;
        }

        public static int GetBlockPoints(DrawableBlock block)
        {
            int points = 0;
            for (int i = 0; i < block.Structure.Length; i++)
            {
                for (int j = 0; j < block.Structure[i].Length; j++)
                {
                    if (block.Structure[i][j] == true)
                        points += 1;
                }
            }

            return points;
        }

        public static int EvaluateGrid(GameEngine refToGameEngine, DrawableCell[][] gridCells, LinesToRemove linesToRemove)
        {
            if (linesToRemove.ColumnIndexes.Count == 0 && linesToRemove.RowIndexes.Count == 0)
                return 0;

            for (int i = 0; i < MainGridOperator.COLUMN_NUMBER; i++)
            {
                int columnIndex = 0;
                int rowIndex = 0;
                while (rowIndex < linesToRemove.RowIndexes.Count || columnIndex < linesToRemove.ColumnIndexes.Count)
                {
                    if (rowIndex < linesToRemove.RowIndexes.Count)
                        gridCells[linesToRemove.RowIndexes[rowIndex++]][i] = null;

                    if (columnIndex < linesToRemove.ColumnIndexes.Count)
                        gridCells[i][linesToRemove.ColumnIndexes[columnIndex++]] = null;

                    refToGameEngine.Notify(NotificationMessage.REFRESH_BOARD, null);
                }
                Thread.Sleep(20);
            }

            return 10 * linesToRemove.ColumnIndexes.Count + 10 * linesToRemove.RowIndexes.Count;
        }

    }
}
