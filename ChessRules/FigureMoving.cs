using System;
using System.Runtime.CompilerServices;

namespace ChessRules
{
    public sealed class FigureMoving
    {
        public Figure Figure { get; }
        public Cell From { get; }
        public Cell To { get; }
        public Figure Promotion { get; }
        public int AbsDeltaX => Math.Abs(DeltaX);
        public int AbsDeltaY => Math.Abs(DeltaY);
        public int DeltaX => To.X - From.X;
        public int DeltaY => To.Y - From.Y;
        public int SignX => Math.Sign(To.X - From.X);
        public int SignY => Math.Sign(To.Y - From.Y);
        public Figure PlacedFigure => Promotion == Figure.None ? Figure : Promotion;

        public static readonly FigureMoving None = new FigureMoving();

        public FigureMoving(FigureOnCell fc, Cell to, Figure promotion = Figure.None)
        {
            Figure = fc.Figure;
            From = fc.Cell;
            To = to;
            Promotion = promotion;
        }

        public FigureMoving(string move)
        {
            Figure = (Figure)move[0];
            From = new Cell(move.Substring(1, 2));
            To = new Cell(move.Substring(3, 2));
            Promotion = move.Length == 6 ? (Figure)move[5] : Figure.None;
        }

        private FigureMoving()
        {
            Figure = Figure.None;
            From = Cell.None;
            To = Cell.None;
            Promotion = Figure.None;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return (char)Figure + 
                   From.Name() + 
                   To.Name() + 
                   (Promotion == Figure.None ? "" : ((char)Promotion).ToString());
        }
    }
}