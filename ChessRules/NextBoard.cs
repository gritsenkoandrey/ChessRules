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
            AddMoveNumber();
            UpdateMoveColor();
            GenerateFen();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void MoveFigures()
        {
            SetFigureAt(_fm.From, Figure.None);
            SetFigureAt(_fm.To, _fm.Promotion == Figure.None ? _fm.Figure : _fm.Promotion);
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