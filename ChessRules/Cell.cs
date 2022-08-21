namespace ChessRules
{
    public readonly struct Cell
    {
        public static Cell None = new Cell(-1, -1);
        
        public int X { get; }
        public int Y { get; }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Cell(string name)
        {
            if (name.Length == 2 && name[0] >= 'a' && name[0] <= 'h' && name[1] >= '1' && name[1] <= '8')
            {
                X = name[0] - 'a';
                Y = name[1] - '1';
            }
            else
            {
                this = None;
            }
        }

        public bool OnBoard()
        {
            return (X >= 0 && X < 8) && (Y >= 0 && Y < 8);
        }

        public string Name()
        {
            return (char)('a' + X) + (Y + 1).ToString();
        }
    }
}