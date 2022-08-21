namespace ChessRules
{
    public sealed class FigureOnCell
    {
        public Figure Figure { get; }
        public Cell Cell { get; }

        public FigureOnCell(Figure figure, Cell cell)
        {
            Figure = figure;
            Cell = cell;
        }
    }
}