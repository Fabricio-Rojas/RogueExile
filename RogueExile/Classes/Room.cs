using RogueExile.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueExile.Classes
{
    internal class Room : IRenderable
    {
        public int MaxWidth = 12;
        public int MaxHeight = 12;
        public int MinWidth = 5;
        public int MinHeight = 5;
        public int Width;
        public int Height;
        public Cell[,] RoomGrid;
        Random random = new();
        public Room()
        {
            Width = random.Next(MinWidth, MaxWidth + 1);
            Height = random.Next(MinHeight, MaxHeight + 1);
            RoomGrid = new Cell[Width, Height];
        }
        private void GenerateBorders()
        {
            for (int col = 0; col < Width; col++)
            {
                for (int row = 0; row < Height; row++)
                {
                    Cell cell = new(col, row);
                    if (cell.X == 0 || cell.X == Width - 1 || cell.Y == 0 || cell.Y == Height - 1)
                    {
                        cell = cell.SetVal('█'); // https://theasciicode.com.ar/
                        cell = cell.SetColor(ConsoleColor.Yellow);
                    }
                    RoomGrid[col, row] = cell;
                }
            }
        }
        public void Render()
        {

        }
    }
}
