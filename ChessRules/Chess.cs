using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChessRules
{
    public sealed class Chess
    {
        public string Fen => _board.Fen;
        
        public bool IsCheck { get; private set; }
        public bool IsCheckmate { get; private set; }
        public bool IsStalemate { get; private set; }

        private readonly Board _board;
        private readonly Moves _moves;

        public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            _board = new Board(fen);
            _moves = new Moves(_board);
        }

        private Chess(Board board)
        {
            _board = board;
            _moves = new Moves(board);
            
            SetCheckFlags();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Chess Move(string move)
        {
            if (!IsValidMove(move))
            {
                return this;
            }

            FigureMoving fm = new FigureMoving(move);

            Board nextBoard = _board.Move(fm);
            
            Chess nextChess = new Chess(nextBoard);

            return nextChess;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char GetFigure(int x, int y)
        {
            Cell cell = new Cell(x,y);

            Figure figure = _board.GetFigureAt(cell);
            
            return figure == Figure.None ? '.' : (char)figure;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char GetFigure(string name)
        {
            Cell cell = new Cell(name);

            Figure figure = _board.GetFigureAt(cell);
            
            return figure == Figure.None ? '.' : (char)figure;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<string> YieldValidMoves()
        {
            foreach (FigureOnCell fc in _board.YieldFigureOnCell())
            {
                foreach (Cell to in Cell.YieldBoardCell())
                {
                    foreach (Figure promotion in fc.Figure.YieldPromotions(to))
                    {
                        FigureMoving fm = new FigureMoving(fc, to, promotion);

                        if (_moves.CanMove(fm))
                        {
                            if (!_board.IsCheckAfter(fm))
                            {
                                yield return fm.ToString();
                            }
                        }
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsValidMove(string move)
        {
            FigureMoving fm = new FigureMoving(move);

            if (!_moves.CanMove(fm))
            {
                return false;
            }

            if (_board.IsCheckAfter(fm))
            {
                return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetCheckFlags()
        {
            IsCheck = _board.IsCheck();
            IsCheckmate = false;
            IsStalemate = false;

            foreach (string moves in YieldValidMoves())
            {
                return;
            }

            if (IsCheck)
            {
                IsCheckmate = true;
            }
            else
            {
                IsStalemate = true;
            }
        }
    }
}