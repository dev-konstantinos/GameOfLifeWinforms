using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace EvolutionGame
{
    /// <summary>
    /// GameEngine class implements the logic of the Game of Life.
    /// </summary>
    public class GameEngine
    {
        public uint CurrentGeneration { get; private set; } = 0;
        private bool[,] field;
        private readonly int rows;
        private readonly int cols;

        public GameEngine(int rows, int cols, int density)
        {
            this.rows = rows;
            this.cols = cols;
            field = new bool[cols, rows];
            Random random = new Random();
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next(100) < ((int)density * 5);
                }
            }
        }
        /// <summary>
        /// Advances the game to the next generation: returns a new field and counts the number of cells
        /// </summary>
        public void NextGen()
        {
            bool[,] newField = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int cellsCount = CountCells(x, y);
                    bool hasLife = field[x, y];

                    if (!hasLife && cellsCount == 3)
                    {
                        newField[x, y] = true;
                    }
                    else if (hasLife && (cellsCount < 2 || cellsCount > 3))
                    {
                        newField[x, y] = false;
                    }
                    else
                    {
                        newField[x, y] = field[x, y];
                    }
                }
            }
            field = newField;
            CurrentGeneration++;
        }
        /// <summary>
        /// Returns the current generation of cells in the field: returns a 2D array of bools
        /// </summary>
        public bool[,] GetCurrentGeneration()
        {
            bool[,] result = new bool[cols, rows];
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    result[x, y] = field[x, y];
                }
            }
            return result;
        }
        /// <summary>
        /// Counts the number of alive cells around a given cell at (X, Y): returns an integer
        /// </summary>
        private int CountCells(int x, int y)
        {
            int count = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int col = (x + i + cols) % cols;
                    int row = (y + j + rows) % rows;

                    bool isSelf = (col == x && row == y);
                    bool hasLife = field[col, row];

                    if (hasLife && !isSelf)
                        count++;
                }
            }
            return count;
        }
        /// <summary>
        /// Validates the position of a cell in the field: returns a boolean
        /// </summary>
        private bool ValidateCellPosition(int x, int y)
        {
            return (x >= 0 && x < cols && y >= 0 && y < rows);
        }
        /// <summary>
        /// Updates the state of a cell in the field at (X, Y)
        /// </summary>
        private void UpdateCellState(int x, int y, bool state)
        {
            if (ValidateCellPosition(x, y))
            {
                field[x, y] = state;
            }
        }
        /// <summary>
        /// Adds a cell to the field at (X, Y)
        /// </summary>
        public void AddCell(int x, int y)
        {
            UpdateCellState(x, y, true);
        }
        /// <summary>
        /// Removes a cell from the field at (X, Y)
        /// </summary>
        public void RemoveCell(int x, int y)
        {
            UpdateCellState(x, y, false);
        }
    }
}
