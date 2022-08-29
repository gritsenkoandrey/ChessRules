using System.Runtime.CompilerServices;
using System.Text;

namespace ChessRules
{
    public sealed class NextBoard : Board
    {
        private readonly FigureMoving _fm;
        
        public NextBoard(string fen, FigureMoving fm) : base(fen)
        {
            _fm = fm;
            
            MoveFigures();
            DropEnPassant();
            SetEnPassant();
            MoveCastleRock();
            UpdateCastleFlags();
            AddMoveNumber();
            UpdateMoveColor();
            GenerateFen();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void MoveFigures()
        {
            SetFigureAt(_fm.From, Figure.None);
            SetFigureAt(_fm.To, _fm.PlacedFigure);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DropEnPassant()
        {
            if (_fm.To == EnPassant)
            {
                if (_fm.Figure == Figure.WhitePawn || _fm.Figure == Figure.BlackPawn)
                {
                    SetFigureAt(new Cell(_fm.To.X, _fm.From.Y), Figure.None);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetEnPassant()
        {
            EnPassant = Cell.None;

            if (_fm.Figure == Figure.WhitePawn)
            {
                if (_fm.From.Y == 1 && _fm.To.Y == 3)
                {
                    EnPassant = new Cell(_fm.From.X, 2);
                }
            }
            else if (_fm.Figure == Figure.BlackPawn)
            {
                if (_fm.From.Y == 6 && _fm.To.Y == 4)
                {
                    EnPassant = new Cell(_fm.From.X, 5);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void MoveCastleRock()
        {
            if (_fm.Figure == Figure.WhiteKing)
            {
                if (_fm.From == new Cell("e1"))
                {
                    if (_fm.To == new Cell("g1"))
                    {
                        SetFigureAt(new Cell("h1"), Figure.None);
                        SetFigureAt(new Cell("f1"), Figure.WhiteRock);
                    }
                    else if (_fm.To == new Cell("c1"))
                    {
                        SetFigureAt(new Cell("a1"), Figure.None);
                        SetFigureAt(new Cell("d1"), Figure.WhiteRock);
                    }
                }
            }
            else if (_fm.Figure == Figure.BlackKing)
            {
                if (_fm.From == new Cell("e8"))
                {
                    if (_fm.To == new Cell("g8"))
                    {
                        SetFigureAt(new Cell("h8"), Figure.None);
                        SetFigureAt(new Cell("f8"), Figure.BlackRock);
                    }
                    else if (_fm.To == new Cell("c8"))
                    {
                        SetFigureAt(new Cell("a8"), Figure.None);
                        SetFigureAt(new Cell("d8"), Figure.BlackRock);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateCastleFlags()
        {
            switch (_fm.Figure)
            {
                case Figure.WhiteKing:
                    CanCastleA1 = false;
                    CanCastleH1 = false;
                    return;
                case Figure.WhiteRock:
                    if (_fm.From == new Cell("a1"))
                    {
                        CanCastleA1 = false;
                    }
                    else if (_fm.From == new Cell("h1"))
                    {
                        CanCastleH1 = false;
                    }
                    return;
                case Figure.BlackKing:
                    CanCastleA8 = false;
                    CanCastleH8 = false;
                    return;
                case Figure.BlackRock:
                    if (_fm.From == new Cell("a8"))
                    {
                        CanCastleA8 = false;
                    }
                    else if (_fm.From == new Cell("h8"))
                    {
                        CanCastleH8 = false;
                    }
                    return;
                
                default: return;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetFigureAt(Cell cell, Figure figure)
        {
            if (cell.OnBoard())
            {
                Figures[cell.X, cell.Y] = figure;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddMoveNumber()
        {
            if (MoveColor == Color.Black)
            {
                MoveNumber++;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateMoveColor()
        {
            MoveColor = MoveColor.FlipColor();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GenerateFen()
        {
            Fen = FenFigures() + " " + 
                  FenMoveColor() + " " + 
                  FenCastleFlags() + " " + 
                  FenEnPassant() + " " +
                  FenDrawNumber() + " " + 
                  FenMoveNumber();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string FenFigures()
        {
            StringBuilder sb = new StringBuilder();
            
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    sb.Append(Figures[x,y] == Figure.None ? '1' : (char)Figures[x, y]);
                }

                if (y > 0)
                {
                    sb.Append("/");
                }
            }

            string eight = "11111111";

            for (int j = 8; j >= 2; j--)
            {
                sb = sb.Replace(eight.Substring(0, j), j.ToString());
            }

            return sb.ToString();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string FenMoveColor()
        {
            return MoveColor == Color.White ? "w" : "b";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string FenCastleFlags()
        {
            string flags = (CanCastleH1 ? "K" : "") +
                           (CanCastleA1 ? "Q" : "") +
                           (CanCastleH8 ? "k" : "") +
                           (CanCastleA8 ? "q" : "") ;

            return flags == "" ? "-" : flags;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string FenEnPassant()
        {
            return EnPassant.Name();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string FenDrawNumber()
        {
            return DrawNumber.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string FenMoveNumber()
        {
            return MoveNumber.ToString();
        }
    }
}