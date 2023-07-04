using System.Drawing;

namespace RogueExile.Classes
{
    internal readonly struct Cell
    {
        public int X { get; }
        public int Y { get; }
        public char Val { get; }
        public ConsoleColor Color { get; }
        public bool IsOccupied { get; }
        public Cell(int x, int y, char val = ' ', ConsoleColor color = ConsoleColor.White, bool isOccupied = false)
        {
            X = x; // X is distance from the right margin
            Y = y; // Y is distance from the top margin
            Val = val; // Val is the character value to be printed in the specific coordinate
            Color = color; // The color to be used when printing the coordinate character
            IsOccupied = isOccupied;
        }
        public Cell MoveXBy(int x)
        {
            return new Cell(X + x, Y, Val, Color, IsOccupied);
        }
        public Cell MoveYBy(int y)
        {
            return new Cell(X, Y + y, Val, Color, IsOccupied);
        }
        public Cell SetVal(char chrVal)
        {
            return new Cell(X, Y, chrVal, Color, IsOccupied);
        }
        public Cell SetColor(ConsoleColor color)
        {
            return new Cell(X, Y, Val, color, IsOccupied);
        }
        public Cell SetOccupied(bool isOccupied)
        {
            return new Cell(X, Y, Val, Color, isOccupied);
        }
    }
}
