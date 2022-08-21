namespace ChessRules
{
    public static class ColorExtension
    {
        public static Color FlipColor(this Color color)
        {
            switch (color)
            {
                case Color.White:
                    return Color.Black;
                case Color.Black:
                    return Color.White;
            }

            return color;
        }
    }
}