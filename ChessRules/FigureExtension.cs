using System.Collections.Generic;

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

        public static IEnumerable<Figure> YieldPromotions(this Figure figure, Cell to)
        {
            if (figure == Figure.WhitePawn && to.Y == 7)
            {
                yield return Figure.WhiteQueen;
                yield return Figure.WhiteRock;
                yield return Figure.WhiteBishop;
                yield return Figure.WhiteKnight;
            }
            else if (figure == Figure.BlackPawn && to.Y == 0)
            {
                yield return Figure.BlackQueen;
                yield return Figure.BlackRock;
                yield return Figure.BlackBishop;
                yield return Figure.BlackKnight;
            }
            else
            {
                yield return Figure.None;
            }
        }
    }
}