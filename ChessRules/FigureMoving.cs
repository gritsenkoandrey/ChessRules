namespace ChessRules
{
    public sealed class FigureMoving
    {
        public Figure Figure { get; }
        public Cell From { get; }
        public Cell To { get; }
        public Figure Promotion { get; }

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
    }
}