﻿using RogueExile.Interfaces;

namespace RogueExile.Classes
{
    internal class Room
    {
        public int MaxWidth = 21;
        public int MaxHeight = 10;
        public int MinWidth = 7;
        public int MinHeight = 4;
        public int Width;
        public int Height;
        public Cell[,] RoomGrid;
        public Cell RoomCenter;
        private readonly Random random = new();
        public MapGenerator Map;
        public Room(MapGenerator mapGenerator)
        {
            Width = random.Next(MinWidth, MaxWidth + 1);
            Height = random.Next(MinHeight, MaxHeight + 1);
            RoomGrid = new Cell[Width, Height];
            Map = mapGenerator;
        }
        private bool GenerateBorders(int startX, int startY)
        {
            for (int col = 0; col < Width; col++)
            {
                for (int row = 0; row < Height; row++)
                {
                    Cell mapCell = Map.mapGrid[startX + col, startY + row];

                    if (mapCell.IsOccupied) // check if the cell we are trying to create already exsists
                    {
                        return false;
                    }

                    Cell cell = new(startX + col, startY + row);
                    switch (cell.X)
                    {
                        case int val when val == startX:
                            cell = cell.SetVal(cell.Y == startY ? '╔' : (cell.Y == startY + Height - 1 ? '╚' : '║'));
                            break;
                        case int val when val == startX + Width - 1:
                            cell = cell.SetVal(cell.Y == startY ? '╗' : (cell.Y == startY + Height - 1 ? '╝' : '║'));
                            break;
                        default:
                            cell = cell.SetVal(cell.Y == startY ? '═' : (cell.Y == startY + Height - 1 ? '═' : '·'));
                            break;
                    }
                    cell = cell.SetOccupied(true);
                    cell = cell.SetColor(ConsoleColor.DarkYellow);
                    if (cell.Val == '·')
                    {
                        cell = cell.SetColor(ConsoleColor.DarkGray);
                    }
                    RoomGrid[col, row] = cell;
                }
            }
            return true;
        }
        public bool FindRoomSpace()
        {
            int mapWidth = Map.mapGrid.GetLength(0);
            int mapHeight = Map.mapGrid.GetLength(1);
            int startX = 0;
            int startY = 0;
            bool spaceFound = false;

            for (int attempts = 0; !spaceFound; attempts++)
            {
                startX = random.Next(2, mapWidth - Width - 2);
                startY = random.Next(2, mapHeight - Height - 2);

                RoomCenter = Map.mapGrid[startX + (Width / 2), startY + (Height / 2)];

                spaceFound = GenerateBorders(startX, startY);

                if (attempts == 50)
                {
                    return false;
                }
            }

            for (int col = 0; col < Width; col++)
            {
                for (int row = 0; row < Height; row++)
                {
                    Cell currentCoord = RoomGrid[col, row];
                    Map.mapGrid[startX + col, startY + row] = currentCoord;
                }
            }
            return true;
        }
    }
}

/* https://theasciicode.com.ar/
╔══╩══╗
║     ║
╣     ╠
║     ║
╚══╦══╝
╬
░▒▓·■@
*/