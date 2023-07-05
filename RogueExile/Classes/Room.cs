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
        public int MaxWidth = 25;
        public int MaxHeight = 11;
        public int MinWidth = 5;
        public int MinHeight = 4;
        public int Width;
        public int Height;
        public Cell[,] RoomGrid;
        Random random = new();
        public MapGenerator Map;
        public Room(MapGenerator mapGenerator)
        {
            Width = random.Next(MinWidth, MaxWidth + 1);
            Height = random.Next(MinHeight, MaxHeight + 1);
            RoomGrid = new Cell[Width, Height];
            Map = mapGenerator;
        }
        private void GenerateBorders(int startX, int startY)
        {
            for (int col = 0; col < Width; col++)
            {
                for (int row = 0; row < Height; row++)
                {
                    Cell cell = new(startX + col, startY + row);
                    switch (cell.X)
                    {
                        case int val when val == startX:
                            cell = cell.SetVal(cell.Y == startY ? '╔' : (cell.Y == startY + Height - 1 ? '╚' : (cell.Y == startY + (Height / 2) ? '╣' : '║')));
                            break;
                        case int val when val == startX + Width - 1:
                            cell = cell.SetVal(cell.Y == startY ? '╗' : (cell.Y == startY + Height - 1 ? '╝' : (cell.Y == startY + (Height / 2) ? '╠' : '║')));
                            break;
                        case int val when val == startX + (Width / 2):
                            cell = cell.SetVal(cell.Y == startY ? '╩' : (cell.Y == startY + Height - 1 ? '╦' : ' '));
                            break;
                        default:
                            cell = cell.SetVal(cell.Y == startY ? '═' : (cell.Y == startY + Height - 1 ? '═' : ' '));
                            break;
                    }
                    RoomGrid[col, row] = cell;
                }
            }
        }
        private void PrintRoom()
        {
            int mapWidth = Map.mapGrid.GetLength(0);
            int mapHeight = Map.mapGrid.GetLength(1);
            int startX = random.Next(2, mapWidth - Width - 2);
            int startY = random.Next(2, mapHeight - Height - 2);
            Cell roomCenter = Map.mapGrid[startX + (Width / 2), startY + (Height / 2)];

            GenerateBorders(startX, startY); 

            // make the code scan out the cell for the potential room, if any of the cells have already been occupied, then retry and look
            // for a new room location with a new room size, if you find appropriate space, create and print the room and apply
            // is occupied = true to all cells of the room

            for (int col = 0; col < Width; col++)
            {
                for (int row = 0; row < Height; row++)
                {
                    Cell currentCoord = RoomGrid[col, row];
                    Map.mapGrid[startX + col, startY + row] = currentCoord;
                    Console.SetCursorPosition(startX + col, startY + row);
                    Console.ForegroundColor = currentCoord.Color;
                    Console.Write(currentCoord.Val);
                    Console.ResetColor();
                }
            }
        }
        public void Render()
        {
            PrintRoom();
        }
    }
}

/* https://theasciicode.com.ar/
╔══╩══╗
║     ║·■@
╣     ╠
║     ║
╚══╦══╝
*/