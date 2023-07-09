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
        private char _icon;
        private ConsoleColor _color;
        private Cell CurrentLocation;
        private Cell NewLocation;
        private readonly Cell[,] MapGrid;
        private Cell NewCell;
        private Cell CellUnder;
        public PlayerCharacter(Cell spawnLocation, Cell[,] mapGrid)
        {
            _icon = '@';
            _color = ConsoleColor.Cyan;
            CurrentLocation = spawnLocation.SetVal(_icon).SetColor(_color);
            NewLocation = mapGrid[CurrentLocation.X, CurrentLocation.Y];
            MapGrid = mapGrid;
        }
        public void Move(Direction direction)
        {
            Cell NextLocation = direction switch
            {
                Direction.Up => CurrentLocation.MoveYBy(-1),
                Direction.Down => CurrentLocation.MoveYBy(1),
                Direction.Left => CurrentLocation.MoveXBy(-1),
                Direction.Right => CurrentLocation.MoveXBy(1),
                _ => CurrentLocation,
            };
            NewLocation = MapGrid[NextLocation.X, NextLocation.Y];
        }
        public void Render()
        {
            switch (NewLocation.Val)
            {
                case '█':
                case '║':
                case '═':
                case ' ':
                    return;

                default:
                    CellUnder = NewCell;
                    NewCell = NewLocation;
                    break;
            }

            if (CellUnder != null)
            {
                Console.SetCursorPosition(CurrentLocation.X, CurrentLocation.Y);
                Console.ForegroundColor = CellUnder.Color;
                Console.Write(CellUnder.Val);
            }

            CurrentLocation = (NewLocation.X > 0 && NewLocation.Y > 0) ? NewLocation : CurrentLocation;

            Console.SetCursorPosition(CurrentLocation.X, CurrentLocation.Y);
            Console.ForegroundColor = _color;
            Console.Write(_icon);
            Console.ResetColor();
        }
    }
}
