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
        public static readonly int mapW = Console.LargestWindowWidth - 2;
        public static readonly int mapH = Console.LargestWindowHeight - 2;
        public Cell[,] mapGrid = new Cell[mapW, mapH];
        public MapGenerator()
        {

        }
        void GenerateBorders()
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
        void PrintMap()
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
        void AddRooms()
        {
            for (int i = 0; i < 30; i++)
            {
                Room newRoom = new Room(this);
                newRoom.Render();
            }
        }
        public void Render()
        {
            GenerateBorders();
            PrintMap();
            AddRooms();
        }
    }
}
