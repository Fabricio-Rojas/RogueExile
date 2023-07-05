using RogueExile.Enums;
using RogueExile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueExile.Classes
{
    internal class PlayerCharacter : IRenderable, IMovable
    {
        private Cell CurrentLocation;
        private Cell NewLocation;
        private readonly Cell[,] MapGrid;
        private char newCharVal;
        private char oldCharVal;
        public PlayerCharacter(Cell spawnLocation, Cell[,] mapGrid)
        {
            CurrentLocation = spawnLocation;
            MapGrid = mapGrid;
        }
        public void Move(Direction direction)
        {
            NewLocation = direction switch
            {
                Direction.Up => CurrentLocation.MoveYBy(-1),
                Direction.Down => CurrentLocation.MoveYBy(1),
                Direction.Left => CurrentLocation.MoveXBy(-1),
                Direction.Right => CurrentLocation.MoveXBy(1),
                _ => CurrentLocation,
            };
        }
        public void Render()
        {
            switch (MapGrid[NewLocation.X, NewLocation.Y].Val)
            {
                case '█':
                case '║':
                case '═':
                    return;

                default:
                    oldCharVal = newCharVal;
                    newCharVal = MapGrid[NewLocation.X, NewLocation.Y].Val;
                    break;
            }

            Console.SetCursorPosition(CurrentLocation.X, CurrentLocation.Y);
            Console.Write(oldCharVal);

            CurrentLocation = (NewLocation.X > 0 && NewLocation.Y > 0) ? NewLocation : CurrentLocation;

            Console.SetCursorPosition(CurrentLocation.X, CurrentLocation.Y);
            Console.Write(CurrentLocation.Val);
        }
    }
}
