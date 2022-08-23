using System;
using System.Text;
using ChessRules;

namespace ChessDemo
{
    internal sealed class Program
    {
        public static void Main(string[] args)
        {
            Chess chess = new Chess();

            while (true)
            {
                Console.WriteLine(chess.Fen);
                
                Print(ChessToAscii(chess));

                foreach (string validMove in chess.YieldValidMoves())
                {
                    Console.WriteLine(validMove);
                }

                string move = Console.ReadLine();

                if (move == "")
                {
                    break;
                }

                chess = chess.Move(move);
            }
        }

        private static string ChessToAscii(Chess chess)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("  +-----------------+");

            for (int y = 7; y >= 0; y--)
            {
                sb.Append(y + 1);
                sb.Append(" | ");
                
                for (int x = 0; x < 8; x++)
                {
                    sb.Append(chess.GetFigure(x, y) + " ");
                }

                sb.AppendLine("|");
            }

            sb.AppendLine("  +-----------------+");
            sb.AppendLine("    a b c d e f g h  ");

            return sb.ToString();
        }

        private static void Print(string text)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            
            foreach (char x in text)
            {
                if (x >= 'a' && x <= 'z')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (x >= 'A' && x <= 'Z')
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                
                Console.Write(x);
            }

            Console.ForegroundColor = oldColor;
        }
    }
}