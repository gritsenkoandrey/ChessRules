﻿namespace ChessRules
{
    public sealed class Chess
    {
        public string Fen => _board.Fen;

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
        }

        public Chess Move(string move)
        {
            FigureMoving fm = new FigureMoving(move);

            if (!_moves.CanMove(fm))
            {
                return this;
            }

            Board nextBoard = _board.Move(fm);
            
            Chess nextChess = new Chess(nextBoard);

            return nextChess;
        }

        public char GetFigure(int x, int y)
        {
            Cell cell = new Cell(x,y);

            Figure figure = _board.GetFigureAt(cell);
            
            return figure == Figure.None ? '.' : (char)figure;
        }
    }
}