using System.Runtime.CompilerServices;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanMove(FigureMoving fm)
        {
            _fm = fm;

            return CanMoveFrom() && CanMoveTo() && CanFigureMove();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanMoveFrom()
        {
            return _fm.From.OnBoard() && 
                   _fm.Figure.GetColor() == _board.MoveColor && 
                   _board.GetFigureAt(_fm.From) == _fm.Figure;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanMoveTo()
        {
            return _fm.To.OnBoard() && 
                   _board.GetFigureAt(_fm.To).GetColor() != _board.MoveColor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanFigureMove()
        {
            switch (_fm.Figure)
            {
                case Figure.WhiteKing:
                case Figure.BlackKing:
                    return CanKingMove() || CanKingCastle();
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
                    return CanKnightMove() || CanKingCastle();
                case Figure.WhitePawn:
                case Figure.BlackPawn:
                    return CanPawnMove();
                default:
                    return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanKingMove()
        {
            return _fm.AbsDeltaX <= 1 && _fm.AbsDeltaY <= 1;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanKingCastle()
        {
            if (_fm.Figure == Figure.WhiteKing)
            {
                if (_fm.From == new Cell("e1"))
                {
                    if (_fm.To == new Cell("g1"))
                    {
                        if (_board.CanCastleH1)
                            if (_board.GetFigureAt(new Cell("h1")) == Figure.WhiteRock)
                                if (_board.GetFigureAt(new Cell("f1")) == Figure.None)
                                    if (_board.GetFigureAt(new Cell("g1")) == Figure.None)
                                        if (!_board.IsCheck())
                                            if (!_board.IsCheckAfter(new FigureMoving("Ke1f1")))
                                                    return true;
                    }
                    else if (_fm.To == new Cell("c1"))
                    {
                        if (_board.CanCastleA1) 
                            if (_board.GetFigureAt(new Cell("a1")) == Figure.WhiteRock)
                                if (_board.GetFigureAt(new Cell("b1")) == Figure.None)
                                    if (_board.GetFigureAt(new Cell("c1")) == Figure.None)
                                        if (_board.GetFigureAt(new Cell("d1")) == Figure.None)
                                            if (!_board.IsCheck())
                                                if (!_board.IsCheckAfter(new FigureMoving("Ke1d1")))
                                                    return true;
                    }
                }
            }
            else if (_fm.Figure == Figure.BlackKing)
            {
                if (_fm.From == new Cell("e8"))
                {
                    if (_fm.To == new Cell("g8"))
                    {
                        if (_board.CanCastleH8)
                            if (_board.GetFigureAt(new Cell("h8")) == Figure.BlackRock)
                                if (_board.GetFigureAt(new Cell("f8")) == Figure.None)
                                    if (_board.GetFigureAt(new Cell("g8")) == Figure.None)
                                        if (!_board.IsCheck())
                                            if (!_board.IsCheckAfter(new FigureMoving("ke8f8")))
                                                return true;
                    }
                    else if (_fm.To == new Cell("c8"))
                    {
                        if (_board.CanCastleA8)
                            if (_board.GetFigureAt(new Cell("a8")) == Figure.BlackRock)
                                if (_board.GetFigureAt(new Cell("b8")) == Figure.None)
                                    if (_board.GetFigureAt(new Cell("c8")) == Figure.None)
                                        if (_board.GetFigureAt(new Cell("d8")) == Figure.None)
                                            if (!_board.IsCheck())
                                                if (!_board.IsCheckAfter(new FigureMoving("ke8d8")))
                                                    return true;
                    }
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanKnightMove()
        {
            return _fm.AbsDeltaX == 1 && _fm.AbsDeltaY == 2 || _fm.AbsDeltaX == 2 && _fm.AbsDeltaY == 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanPawnMove()
        {
            if (_fm.From.Y < 1 || _fm.From.Y > 6)
            {
                return false;
            }

            int stepY = _fm.Figure.GetColor() == Color.White ? +1 : -1;

            return CanPawnGo(stepY) || CanPawnJump(stepY) || CanPawnEat(stepY) || CanPawnEnPassant(stepY);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanPawnGo(int stepY)
        {
            if (_board.GetFigureAt(_fm.To) == Figure.None)
            {
                if (_fm.DeltaX == 0)
                {
                    if (_fm.DeltaY == stepY)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanPawnJump(int stepY)
        {
            if (_board.GetFigureAt(_fm.To) == Figure.None)
            {
                if ((_fm.From.Y == 1 && stepY == +1) || (_fm.From.Y == 6 && stepY == -1))
                {
                    if (_fm.DeltaX == 0)
                    {
                        if (_fm.DeltaY == 2 * stepY)
                        {
                            if (_board.GetFigureAt(new Cell(_fm.From.X, _fm.From.Y + stepY)) == Figure.None)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanPawnEat(int stepY)
        {
            if (_board.GetFigureAt(_fm.To) != Figure.None)
            {
                if (_fm.AbsDeltaX == 1)
                {
                    if (_fm.DeltaY == stepY)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanPawnEnPassant(int stepY)
        {
            if (_fm.To == _board.EnPassant)
            {
                if (_board.GetFigureAt(_fm.To) == Figure.None)
                {
                    if (_fm.DeltaY == stepY)
                    {
                        if (_fm.AbsDeltaX == 1)
                        {
                            if ((stepY == +1 && _fm.From.Y == 4) || (stepY == -1 && _fm.From.Y == 3))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}