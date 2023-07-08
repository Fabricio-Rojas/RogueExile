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
            mapH = Console.LargestWindowHeight - 2;
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

                // check why this is not printing
                if (firstTarget != null)
                {
                    List<Cell> path1 = GetCellsInPath(startingPosition, firstTarget.RoomCenter);
                    for (int i = 0; i < path1.Count; i++)
                    {
                        if (path1[i].IsOccupied)
                        {
                            if (path1[i].Val == '║' || path1[i].Val == '═')
                            {
                                path1[i] = path1[i].SetVal('░');
                            }
                            continue;
                        }
                        path1[i] = path1[i].SetOccupied(true);
                        path1[i] = path1[i].SetVal('░');
                    }
                }

                if (secondTarget != null)
                {
                    List<Cell> path2 = GetCellsInPath(startingPosition, secondTarget.RoomCenter);
                    for (int i = 0; i < path2.Count; i++)
                    {
                        if (path2[i].IsOccupied)
                        {
                            if (path2[i].Val == '║' || path2[i].Val == '═')
                            {
                                path2[i] = path2[i].SetVal('░');
                            }
                            continue;
                        }
                        path2[i] = path2[i].SetOccupied(true);
                        path2[i] = path2[i].SetVal('░');
                    }
                }
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
                if (err2 < dx)
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
