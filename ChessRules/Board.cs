﻿namespace ChessRules
{
    public sealed class Board
    {
        public string Fen { get; }
        public Color MoveColor { get; private set; }
        public bool CanCastleA1 { get; private set; } //Q
        public bool CanCastleH1 { get; private set; } //K
        public bool CanCastleA8 { get; private set; } //q
        public bool CanCastleH8 { get; private set; } //k
        public Cell EnPassant { get; private set; }
        public int DrawNumber { get; private set; }
        public int MoveNumber { get; private set; }
        
        private readonly Figure[,] _figures;

        public Board(string fen)
        {
            Fen = fen;

            _figures = new Figure[8, 8];
            
            Init();
        }

        public Board Move(FigureMoving fm)
        {
            Board next = new Board(Fen);
            
            next.SetFigureAt(fm.From, Figure.None);
            next.SetFigureAt(fm.To, fm.Figure);
            next.MoveColor = MoveColor.FlipColor();

            return next;
        }

        public Figure GetFigureAt(Cell cell)
        {
            if (cell.OnBoard())
            {
                return _figures[cell.X, cell.Y];
            }

            return Figure.None;
        }

        private void SetFigureAt(Cell cell, Figure figure)
        {
            if (cell.OnBoard())
            {
                _figures[cell.X, cell.Y] = figure;
            }
        }

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
                    _figures[x, y] = (Figure)lines[7 - y][x];
                }
            }
        }

        private void InitMoveColor(string part)
        {
            MoveColor = part == "b" ? Color.Black : Color.White;
        }

        private void InitCastleFlags(string part)
        {
            CanCastleA1 = part.Contains("Q");
            CanCastleH1 = part.Contains("K");
            CanCastleA8 = part.Contains("q");
            CanCastleH8 = part.Contains("k");
        }

        private void InitEnPassant(string part)
        {
            EnPassant = new Cell(part);
        }

        private void InitDrawNumber(string part)
        {
            DrawNumber = int.Parse(part);
        }

        private void InitMoveNumber(string part)
        {
            MoveNumber = int.Parse(part);
        }
    }
}