using System;

namespace MeiMing.Framework.Provider
{
    internal static class MaskPattern
    {
        public static bool Pattern1(int x, int y)
        {
            return (x + y) % 2 == 0;
        }

        public static bool Pattern2(int x, int y)
        {
            return y % 2 == 0;
        }

        public static bool Pattern3(int x, int y)
        {
            return x % 3 == 0;
        }

        public static bool Pattern4(int x, int y)
        {
            return (x + y) % 3 == 0;
        }

        public static bool Pattern5(int x, int y)
        {
            return (y / 2 + x / 3) % 2 == 0;
        }

        public static bool Pattern6(int x, int y)
        {
            return ((x * y) % 2) + ((x * y) % 3) == 0;
        }

        public static bool Pattern7(int x, int y)
        {
            return (((x * y) % 2) + ((x * y) % 3)) % 2 == 0;
        }

        public static bool Pattern8(int x, int y)
        {
            return (((x + y) % 2) + ((x * y) % 3)) % 2 == 0;
        }

        public static int Score(ref QrCode qrCode)
        {
            var score = 0;
            var size = qrCode.ModuleMatrix.Count;

            //Penalty 1                   
            for (int y = 0; y < size; y++)
            {
                var modInRow = 0;
                var modInColumn = 0;
                var lastValRow = qrCode.ModuleMatrix[y][0];
                var lastValColumn = qrCode.ModuleMatrix[0][y];
                for (int x = 0; x < size; x++)
                {
                    if (qrCode.ModuleMatrix[y][x] == lastValRow)
                        modInRow++;
                    else
                        modInRow = 1;
                    if (modInRow == 5)
                        score += 3;
                    else if (modInRow > 5)
                        score++;
                    lastValRow = qrCode.ModuleMatrix[y][x];


                    if (qrCode.ModuleMatrix[x][y] == lastValColumn)
                        modInColumn++;
                    else
                        modInColumn = 1;
                    if (modInColumn == 5)
                        score += 3;
                    else if (modInColumn > 5)
                        score++;
                    lastValColumn = qrCode.ModuleMatrix[x][y];
                }
            }


            //Penalty 2
            for (int y = 0; y < size - 1; y++)
            {
                for (int x = 0; x < size - 1; x++)
                {
                    if (qrCode.ModuleMatrix[y][x] == qrCode.ModuleMatrix[y][x + 1] &&
                        qrCode.ModuleMatrix[y][x] == qrCode.ModuleMatrix[y + 1][x] &&
                        qrCode.ModuleMatrix[y][x] == qrCode.ModuleMatrix[y + 1][x + 1])
                        score += 3;
                }
            }

            //Penalty 3
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size - 10; x++)
                {
                    if ((qrCode.ModuleMatrix[y][x] &&
                         !qrCode.ModuleMatrix[y][x + 1] &&
                         qrCode.ModuleMatrix[y][x + 2] &&
                         qrCode.ModuleMatrix[y][x + 3] &&
                         qrCode.ModuleMatrix[y][x + 4] &&
                         !qrCode.ModuleMatrix[y][x + 5] &&
                         qrCode.ModuleMatrix[y][x + 6] &&
                         !qrCode.ModuleMatrix[y][x + 7] &&
                         !qrCode.ModuleMatrix[y][x + 8] &&
                         !qrCode.ModuleMatrix[y][x + 9] &&
                         !qrCode.ModuleMatrix[y][x + 10]) ||
                        (!qrCode.ModuleMatrix[y][x] &&
                         !qrCode.ModuleMatrix[y][x + 1] &&
                         !qrCode.ModuleMatrix[y][x + 2] &&
                         !qrCode.ModuleMatrix[y][x + 3] &&
                         qrCode.ModuleMatrix[y][x + 4] &&
                         !qrCode.ModuleMatrix[y][x + 5] &&
                         qrCode.ModuleMatrix[y][x + 6] &&
                         qrCode.ModuleMatrix[y][x + 7] &&
                         qrCode.ModuleMatrix[y][x + 8] &&
                         !qrCode.ModuleMatrix[y][x + 9] &&
                         qrCode.ModuleMatrix[y][x + 10]))
                    {
                        score += 40;
                    }

                    if ((qrCode.ModuleMatrix[x][y] &&
                         !qrCode.ModuleMatrix[x + 1][y] &&
                         qrCode.ModuleMatrix[x + 2][y] &&
                         qrCode.ModuleMatrix[x + 3][y] &&
                         qrCode.ModuleMatrix[x + 4][y] &&
                         !qrCode.ModuleMatrix[x + 5][y] &&
                         qrCode.ModuleMatrix[x + 6][y] &&
                         !qrCode.ModuleMatrix[x + 7][y] &&
                         !qrCode.ModuleMatrix[x + 8][y] &&
                         !qrCode.ModuleMatrix[x + 9][y] &&
                         !qrCode.ModuleMatrix[x + 10][y]) ||
                        (!qrCode.ModuleMatrix[x][x] &&
                         !qrCode.ModuleMatrix[x + 1][y] &&
                         !qrCode.ModuleMatrix[x + 2][y] &&
                         !qrCode.ModuleMatrix[x + 3][y] &&
                         qrCode.ModuleMatrix[x + 4][y] &&
                         !qrCode.ModuleMatrix[x + 5][y] &&
                         qrCode.ModuleMatrix[x + 6][y] &&
                         qrCode.ModuleMatrix[x + 7][y] &&
                         qrCode.ModuleMatrix[x + 8][y] &&
                         !qrCode.ModuleMatrix[x + 9][y] &&
                         qrCode.ModuleMatrix[x + 10][y]))
                    {
                        score += 40;
                    }
                }
            }

            //Penalty 4
            var blackModules = 0;
            foreach (var row in qrCode.ModuleMatrix)
                foreach (bool bit in row)
                    if (bit)
                        blackModules++;

            var percent = (blackModules / (qrCode.ModuleMatrix.Count * qrCode.ModuleMatrix.Count)) * 100;
            if (percent % 5 == 0)
                score += Math.Min((Math.Abs(percent - 55) / 5), (Math.Abs(percent - 45) / 5)) * 10;
            else
                score += Math.Min((Math.Abs((int)Math.Floor((decimal)percent / 5) - 50) / 5), (Math.Abs(((int)Math.Floor((decimal)percent / 5) + 5) - 50) / 5)) * 10;

            return score;
        }
    }
}