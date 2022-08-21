namespace ChessRules
{
    public static class FigureExtension
    {
        public static Color GetColor(this Figure figure)
        {
            switch (figure)
            {
                case Figure.None: return Color.None;
                case Figure.WhiteKing:
                case Figure.WhiteQueen:
                case Figure.WhiteRock:
                case Figure.WhiteBishop:
                case Figure.WhiteKnight:
                case Figure.WhitePawn: return Color.White;
                case Figure.BlackKing:
                case Figure.BlackQueen:
                case Figure.BlackRock:
                case Figure.BlackBishop:
                case Figure.BlackKnight:
                case Figure.BlackPawn: return Color.Black;
                default: return Color.None;
            }
        }
    }
}