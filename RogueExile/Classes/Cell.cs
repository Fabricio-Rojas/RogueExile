namespace RogueExile.Classes
{
    internal readonly struct Cell
    {
        public int X { get; }
        public int Y { get; }
        public char Val { get; }
        public ConsoleColor Color { get; }
        public Cell(int x, int y, char val = ' ', ConsoleColor color = ConsoleColor.White)
        {
            X = x; // X is distance from the right margin
            Y = y; // Y is distance from the top margin
            Val = val; // Val is the character value to be printed in the specific coordinate
            Color = color; // The color to be used when printing the coordinate character
        }
        public Cell MoveXBy(int x)
        {
            return new Cell(X + x, Y, Val, Color);
        }
        public Cell MoveYBy(int y)
        {
            return new Cell(X, Y + y, Val, Color);
        }
        public Cell SetVal(char chrVal)
        {
            return new Cell(X, Y, chrVal, Color);
        }
        public Cell SetColor(ConsoleColor color)
        {
            return new Cell(X, Y, Val, color);
        }
    }
}
