using RogueExile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueExile.Classes.MapGen
{
    internal class MapGenerator : IRenderable
    {
        public static int mapW;
        public static int mapH;
        public Cell[,] mapGrid;
        public List<Room> rooms;
        public MapGenerator()
        {
            mapW = Console.LargestWindowWidth - 2;
            mapH = Console.LargestWindowHeight - 2;
            mapGrid = new Cell[mapW, mapH];
            rooms = new List<Room>();
        }
        public void Generate(int level)
        {
            GenerateBorders();
            AddRooms(level);
            AddPaths();
            Render();
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
        private void AddRooms(int level)
        {
            int roomLimit = 0;

            if (level < 15)
            {
                roomLimit = 2 * level;
            }
            else
            {
                roomLimit = int.MaxValue;
            }

            int roomsGenerated = 0;

            while (roomsGenerated < roomLimit)
            {
                Room newRoom = new(this);
                bool roomPossible = newRoom.FindRoomSpace();

                if (roomPossible)
                {
                    rooms.Add(newRoom);
                    roomsGenerated++;
                }
                else
                {
                    break;
                }
            }
        }
        private void AddPaths()
        {
            foreach (Room room in rooms)
            {
                Room startingRoom = room;
                Room? firstTarget = null;
                Room? secondTarget = null;
                int shortestDistance = int.MaxValue;

                foreach (Room targetRoom in rooms)
                {
                    if (targetRoom == room || startingRoom.LinkedRooms.Contains(targetRoom) || targetRoom.LinkedRooms.Contains(startingRoom))
                        continue;

                    int distance = Math.Abs(targetRoom.RoomCenter.X - startingRoom.RoomCenter.X) + Math.Abs(targetRoom.RoomCenter.Y - startingRoom.RoomCenter.Y);

                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        secondTarget = firstTarget;
                        firstTarget = targetRoom;
                    }
                }
                GeneratePath(startingRoom, firstTarget);
                GeneratePath(startingRoom, secondTarget);
            }
        }
        private void GeneratePath(Room startingRoom, Room? targetRoom)
        {
            if (targetRoom == null)
                return;

            startingRoom.LinkedRooms.Add(targetRoom);
            targetRoom.LinkedRooms.Add(targetRoom);

            List<Cell> path = AStar.FindPath(mapGrid, startingRoom, targetRoom);

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
        public void Render()
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
        }
    }
}
