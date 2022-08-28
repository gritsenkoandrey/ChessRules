using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool OnBoard()
        {
            return (X >= 0 && X < 8) && (Y >= 0 && Y < 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string Name()
        {
            if (OnBoard())
            {
                return (char)('a' + X) + (Y + 1).ToString();
            }
            
            return "-";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Cell> YieldBoardCell()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    yield return new Cell(x, y);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Cell a, Cell b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Cell a, Cell b)
        {
            return !(a == b);
        }
    }
}