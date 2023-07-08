using RogueExile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueExile.Classes
{
    internal class MapGenerator : IRenderable
    {
        public static int mapW;
        public static int mapH;
        public Cell[,] mapGrid;
        public HashSet<Room> rooms;
        public MapGenerator()
        {
            mapW = Console.LargestWindowWidth - 2;
            mapH = Console.LargestWindowHeight;
            mapGrid = new Cell[mapW, mapH];
            rooms = new HashSet<Room>();
        }
        private void GenerateBorders()
        {
            for (int col = 0; col < mapW; col++)
            {
                for (int row = 0; row < mapH; row++)
                {
                    Cell coord = new(col, row);
                    if (coord.X == 0 || coord.X == mapW - 1 || coord.Y == 0 || coord.Y == mapH - 1)
                    {
                        coord = coord.SetVal('█');
                        coord = coord.SetColor(ConsoleColor.Yellow);
                        coord = coord.SetOccupied(true);
                    }
                    mapGrid[col, row] = coord;
                }
            }
        }
        private void PrintMap()
        {
            Console.CursorVisible = false;
            for (int col = 0; col < mapW; col++)
            {
                for (int row = 0; row < mapH; row++)
                {
                    Cell currentCoord = mapGrid[col, row];
                    Console.SetCursorPosition(currentCoord.X, currentCoord.Y);
                    Console.ForegroundColor = currentCoord.Color;
                    Console.Write(currentCoord.Val);
                    Console.ResetColor();
                }
            }
            Console.SetCursorPosition(0, mapH);
        }
        private void AddRooms()
        {
            bool roomPossible = true;
            while (roomPossible)
            {
                Room newRoom = new(this);
                roomPossible = newRoom.FindRoomSpace();
                rooms.Add(newRoom);
            }
        }
        private void AddPaths()
        {
            foreach (Room room in rooms)
            {
                Cell startingPosition = room.RoomCenter;
                Room? firstTarget = null;
                Room? secondTarget = null;
                int shortestDistance = int.MaxValue;

                foreach (Room targetRoom in rooms)
                {
                    if (targetRoom == room)
                        continue;

                    int distance = Math.Abs(targetRoom.RoomCenter.X - startingPosition.X) + Math.Abs(targetRoom.RoomCenter.Y - startingPosition.Y);

                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        secondTarget = firstTarget;
                        firstTarget = targetRoom;
                    }
                }
                GeneratePath(startingPosition, firstTarget?.RoomCenter);
                GeneratePath(startingPosition, secondTarget?.RoomCenter);
            }
        }
        private void GeneratePath(Cell start, Cell? target)
        {
            if (target == null)
                return;

            List<Cell> path = GetCellsInPath(start, target);

            for (int i = 0; i < path.Count; i++)
            {
                if (path[i].IsOccupied)
                {
                    bool isWall = path[i].Val switch
                    {
                        '║' => true,
                        '═' => true,
                        '╔' => true,
                        '╚' => true,
                        '╗' => true,
                        '╝' => true,
                        _ => false
                    };
                    if (isWall)
                    {
                        path[i] = path[i].SetVal('#');
                    }
                }
                else
                {
                    path[i] = path[i].SetOccupied(true);
                    path[i] = path[i].SetVal('░');
                }
                mapGrid[path[i].X, path[i].Y] = path[i];
            }
        }
        private List<Cell> GetCellsInPath(Cell start, Cell end)
        {
            List<Cell> cells = new List<Cell>();

            int x0 = start.X;
            int y0 = start.Y;
            int x1 = end.X;
            int y1 = end.Y;

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                cells.Add(mapGrid[x0, y0]);

                if (x0 == x1 && y0 == y1)
                    break;

                int err2 = 2 * err;

                if (err2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                else if (err2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
            return cells;
        }
        public void Render()
        {
            GenerateBorders();
            AddRooms();
            AddPaths();
            PrintMap();
        }
    }
}
