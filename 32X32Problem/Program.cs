using System;

namespace _32X32Problem
{
    class Program
    {
        private static int _countGcfpxa;
        private static int _countGcfpxaH;

        static void Main(string[] args)
        {
            int width = 100;
            int height = 100;
            int[,] inputArray = new int[height, width];
            int i, j;
            int value = 1;
            for (i = 0; i < height; i++)
            {
                for (int x = 0; x < width; x++)
                {
                    inputArray[i, x] = value;
                    value++;
                }
            }

            int[,] newArray = Create32x32Blocks(width, height, inputArray);
            int[,] solvedArray = Solve32X32Blocks(width, height, newArray);
            for (i = 0; i < height; i ++)
            {
                for (j = 0; j < width; j++)
                {
                    if (solvedArray[i, j] != inputArray[i, j])
                    {
                        break;
                    }
                }
                if (j < width)
                {
                    break;
                }
            }

            if (i == height)
            {
                // YOU HAVE DONE IT
                Console.WriteLine("Yay! it works");
            }
        }

        public static int[,] Create32x32Blocks(int width, int height, int[,] arrayOld)
        {
            // YOUR CODE HERE
            int[,] arrayNew = new int[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int totIdx = x + y * width;

                    int yBlockId = totIdx / (32 * width);
                    totIdx -= yBlockId * (32 * width);
                    int xBlockId;
                    if (yBlockId == (height - 1) / 32)
                    {
                        xBlockId = totIdx / (32 * (height % 32 > 0 ? height % 32 : 32));
                        totIdx -= xBlockId * (32 * (height % 32 > 0 ? height % 32 : 32));
                    }
                    else
                    {
                        xBlockId = totIdx / (32 * 32);
                        totIdx -= xBlockId * (32 * 32);
                    }

                    int xOffset, yOffset;
                    if (xBlockId == (width - 1) / 32)
                    {
                        yOffset = totIdx / (width % 32 > 0 ? width % 32 : 32);
                        xOffset = totIdx % (width % 32 > 0 ? width % 32 : 32);
                    }
                    else
                    {
                        yOffset = totIdx / 32;
                        xOffset = totIdx % 32;
                    }

                    int mapPosX = xBlockId * 32 + xOffset;
                    int mapPosY = yBlockId * 32 + yOffset;
                    arrayNew[y, x] = arrayOld[mapPosY, mapPosX];
                }
            }

            return arrayNew;
        }

        public static int[,] Solve32X32Blocks(int width, int height, int[,] arrayOld)
        {
            var modWidth = width % 32;
            var timeWidth = (width - modWidth) / 32;

            var modHeight = height % 32;
            var timeHeight = (height - modHeight) / 32;

            int[,] arrayNew = new int[height, width];

            for (var timeH = 0; timeH < timeHeight + 1; timeH++)
            {
                var offsetX = 0;
                var offsetY = 0;

                var lineH = 32;

                if (timeH == timeHeight)
                    lineH = modHeight;

                for (var time = 0; time < timeWidth; time++)
                    for (var positionY = 0; positionY < lineH; positionY++)
                        for (var positionX = 0; positionX < 32; positionX++)
                        {
                            offsetX = time * 32;
                            offsetY = timeH * 32;
                            arrayNew[positionY + offsetY, positionX + offsetX] = GetColorFromPxArray(arrayOld, width);
                        }

                for (var positionY = 0; positionY < lineH; positionY++)
                    for (var positionX = 0; positionX < modWidth; positionX++)
                    {
                        offsetX = timeWidth * 32;
                        offsetY = timeH * 32;
                        arrayNew[positionY + offsetY, positionX + offsetX] = GetColorFromPxArray(arrayOld, width);
                    }
            }

            return arrayNew;
        }

        public static int GetColorFromPxArray(int[,] pixelArrayOLD, int width)
        {
            if (_countGcfpxa > pixelArrayOLD.GetLength(0) - 1)
            {
                _countGcfpxa = 0;
                _countGcfpxaH += 1;
            }

            int goodColor = pixelArrayOLD[_countGcfpxaH, _countGcfpxa];

            _countGcfpxa += 1;

            return goodColor;
        }
    }
}