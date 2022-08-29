using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChessRules
{
    public class Board
    {
        public string Fen { get; protected set; }
        public Cell EnPassant { get; protected set; }
        public Color MoveColor { get; protected set; }
        public bool CanCastleA1 { get; protected set; } //Q
        public bool CanCastleH1 { get; protected set; } //K
        public bool CanCastleA8 { get; protected set; } //q
        public bool CanCastleH8 { get; protected set; } //k
        protected int DrawNumber { get; private set; }
        protected int MoveNumber { get; set; }
        
        protected readonly Figure[,] Figures;

        public Board(string fen)
        {
            Fen = fen;

            Figures = new Figure[8, 8];
            
            Init();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Board Move(FigureMoving fm)
        {
            return new NextBoard(Fen, fm);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Figure GetFigureAt(Cell cell)
        {
            if (cell.OnBoard())
            {
                return Figures[cell.X, cell.Y];
            }

            return Figure.None;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<FigureOnCell> YieldFigureOnCell()
        {
            foreach (Cell cell in Cell.YieldBoardCell())
            {
                if (GetFigureAt(cell).GetColor() == MoveColor)
                {
                    yield return new FigureOnCell(GetFigureAt(cell), cell);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsCheck()
        {
            return IsCheckAfter(FigureMoving.None);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsCheckAfter(FigureMoving fm)
        {
            Board after = Move(fm);

            return after.CanEatKing();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanEatKing()
        {
            Cell badKing = FindBadKing();
            Moves moves = new Moves(this);

            foreach (FigureOnCell fc in YieldFigureOnCell())
            {
                if (moves.CanMove(new FigureMoving(fc, badKing)))
                {
                    return true;
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Cell FindBadKing()
        {
            Figure badKing = MoveColor == Color.White ? Figure.BlackKing : Figure.WhiteKing;

            foreach (Cell cell in Cell.YieldBoardCell())
            {
                if (GetFigureAt(cell) == badKing)
                {
                    return cell;
                }
            }
            
            return Cell.None;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Init()
        {
            //rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
            //0                                           1 2    3 4 5

            string[] parts = Fen.Split();
            
            InitFigures(parts[0]);
            InitMoveColor(parts[1]);
            InitCastleFlags(parts[2]);
            InitEnPassant(parts[3]);
            InitDrawNumber(parts[4]);
            InitMoveNumber(parts[5]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitFigures(string part)
        {
            // 8->71->611->5111->41111->311111->2111111->11111111
            
            for (int j = 8; j >= 2; j--)
            {
                part = part.Replace(j.ToString(), j - 1 + "1");
            }

            part = part.Replace('1', (char)Figure.None);
            
            string[] lines = part.Split('/');

            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    Figures[x, y] = (Figure)lines[7 - y][x];
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitMoveColor(string part)
        {
            MoveColor = part == "b" ? Color.Black : Color.White;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitCastleFlags(string part)
        {
            CanCastleA1 = part.Contains("Q");
            CanCastleH1 = part.Contains("K");
            CanCastleA8 = part.Contains("q");
            CanCastleH8 = part.Contains("k");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitEnPassant(string part)
        {
            EnPassant = new Cell(part);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitDrawNumber(string part)
        {
            DrawNumber = int.Parse(part);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitMoveNumber(string part)
        {
            MoveNumber = int.Parse(part);
        }
    }
}