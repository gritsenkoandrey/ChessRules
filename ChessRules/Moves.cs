namespace ChessRules
{
    public sealed class Moves
    {
        private readonly Board _board;
        
        private FigureMoving _fm;

        public Moves(Board board)
        {
            _board = board;
        }

        public bool CanMove(FigureMoving fm)
        {
            _fm = fm;

            return CanMoveFrom() && CanMoveTo() && CanFigureMove();
        }

        private bool CanMoveFrom()
        {
            return _fm.From.OnBoard() && 
                   _fm.Figure.GetColor() == _board.MoveColor && 
                   _board.GetFigureAt(_fm.From) == _fm.Figure;
        }

        private bool CanMoveTo()
        {
            return _fm.To.OnBoard() && 
                   _board.GetFigureAt(_fm.To).GetColor() != _board.MoveColor;
        }

        private bool CanFigureMove()
        {
            switch (_fm.Figure)
            {
                case Figure.WhiteKing:
                case Figure.BlackKing:
                    return CanKingMove();
                case Figure.WhiteQueen:
                case Figure.BlackQueen:
                    return CanStraightMove();
                case Figure.WhiteRock:
                case Figure.BlackRock:
                    return (_fm.SignX == 0 || _fm.SignY == 0) && CanStraightMove();
                case Figure.WhiteBishop:
                case Figure.BlackBishop:
                    return (_fm.SignX != 0 && _fm.SignY != 0) && CanStraightMove();
                case Figure.WhiteKnight:
                case Figure.BlackKnight:
                    return CanKnightMove();
                case Figure.WhitePawn:
                case Figure.BlackPawn:
                    return false;
                default:
                    return false;
            }
        }

        private bool CanStraightMove()
        {
            Cell at = _fm.From;

            do
            {
                at = new Cell(at.X + _fm.SignX, at.Y + _fm.SignY);

                if (at == _fm.To)
                {
                    return true;
                }

            } while (at.OnBoard() && _board.GetFigureAt(at) == Figure.None);

            return false;
        }

        private bool CanKnightMove()
        {
            return _fm.DeltaX == 1 && _fm.DeltaY == 2 || _fm.DeltaX == 2 && _fm.DeltaY == 1;
        }

        private bool CanKingMove()
        {
            return _fm.DeltaX <= 1 && _fm.DeltaY <= 1;
        }
    }
}