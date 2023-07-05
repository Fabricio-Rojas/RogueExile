using RogueExile.Interfaces;

namespace RogueExile.Classes
{
    internal class Room
    {
        public int MaxWidth = 25;
        public int MaxHeight = 11;
        public int MinWidth = 5;
        public int MinHeight = 4;
        public int Width;
        public int Height;
        public Cell[,] RoomGrid;
        public Cell RoomCenter;
        Random random = new();
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
                    cell = cell.SetOccupied(true);
                    RoomGrid[col, row] = cell;
                }
            }
            return true;
        }
        public bool PrintRoom()
        {
            int mapWidth = Map.mapGrid.GetLength(0);
            int mapHeight = Map.mapGrid.GetLength(1);
            int startX = 0;
            int startY = 0;
            bool spaceFound = false;

            for (int attempts = 0; attempts < 10 && !spaceFound; attempts++)
            {
                startX = random.Next(2, mapWidth - Width - 2);
                startY = random.Next(2, mapHeight - Height - 2);

                RoomCenter = Map.mapGrid[startX + (Width / 2), startY + (Height / 2)];

                spaceFound = GenerateBorders(startX, startY);

                if (attempts == 9)
                {
                    return false;
                }
            }

            // make the code scan out the cell for the potential room, if any of the cells have already been occupied, then retry and look
            // for a new room location with a new room size, if you find appropriate space, create and print the room and apply
            // is occupied = true to all cells of the room

            // instead of creating doors manually, create a path from one room center to another, if cell is occupied and empty, continue,
            // else if occupied and wall, create door, if not occupied, create corridor

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
            return true;
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