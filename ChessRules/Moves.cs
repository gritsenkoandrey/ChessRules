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

            return CanMoveFrom() && CanMoveTo();
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
    }
}