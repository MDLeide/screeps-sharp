using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreepsUtil.Sandbox
{
    /// <summary>
    /// Provides methods to rotate 'maps' which are really just rectangular jagged arrays.
    /// </summary>
    class MapRotation
    {
        /// <summary>
        /// Makes a rectangular array of consecutive numbers.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public int[][] MakeArray(int width, int height)
        {
            var count = 0;
            var array = new int[width][];
            for (int x = 0; x < width; x++)
            {
                array[x] = new int[height];
                for (int y = 0; y < height; y++)
                {
                    array[x][y] = count++;
                }
            }
            return array;
        }

        /// <summary>
        /// Rotates a rectangular array clockwise.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public int[][] RotateClockwise(int[][] array)
        {
            var width = array.Length;
            var height = array[0].Length;

            var newArray = new int[height][];
            for (int i = 0; i < height; i++)
                newArray[i] = new int[width];

            for (int x = 0; x < array.Length; x++)
            {
                for (int y = 0; y < array[x].Length; y++)
                {
                    newArray[y][width - x - 1] = array[x][y];
                }
            }
            return newArray;
        }

        /// <summary>
        /// Rotates a rectangular array counter-clockwise.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public int[][] RotateCounterClockwise(int[][] array)
        {
            var width = array.Length;
            var height = array[0].Length;

            var newArray = new int[height][];
            for (int i = 0; i < height; i++)
                newArray[i] = new int[width];

            for (int x = 0; x < array.Length; x++)
            {
                for (int y = 0; y < array[x].Length; y++)
                {
                    newArray[height - y - 1][x] = array[x][y];
                }
            }
            return newArray;
        }
    }

    class MapRotationTest
    {
        public void Test(
            int width = 5,
            int height = 5,
            bool counterClockwise = false,
            int rotations = 3,
            bool wait = true)
        {
            var mapRotation = new MapRotation();

            var array = mapRotation.MakeArray(width, height);
            PrettyPrint(array, "** ORIGINAL **");

            if (wait)
                if (Wait("Press any key to start rotations..."))
                    return;

            for (int i = 0; i < rotations; i++)
            {
                string type = counterClockwise ? "COUNTER CLOCKWISE" : "CLOCKWISE";

                if (counterClockwise)
                    array = mapRotation.RotateCounterClockwise(array);
                else
                    array = mapRotation.RotateClockwise(array);

                PrettyPrint(array, $"** {type} ROTATION #{i + 1} **");

                if (i == rotations - 1)
                    Wait("Press any key to exit...", 0, ConsoleColor.Red);
                else if (wait)
                    if (Wait("Press any key for next rotation..."))
                        return;
            }
        }

        /// <summary>
        /// Displays a message to the user and waits for any key entry. If escape is pressed, returns true.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="postLines"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        bool Wait(string message, int postLines = 2, ConsoleColor color = ConsoleColor.DarkGreen)
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
                return true;
            for (int i = 0; i < postLines; i++)
                Console.WriteLine();
            Console.ForegroundColor = temp;
            return false;
        }

        /// <summary>
        /// Prints a header message followed by printing the jagged array with optional color support.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="header"></param>
        /// <param name="headerColor"></param>
        /// <param name="arrayColor"></param>
        void PrettyPrint(int[][] array, string header, ConsoleColor headerColor = ConsoleColor.Cyan, ConsoleColor arrayColor = ConsoleColor.Magenta)
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = headerColor;
            Console.WriteLine(header);
            Console.WriteLine();
            Console.ForegroundColor = arrayColor;
            Print(array);
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = temp;
        }

        /// <summary>
        /// Prints a rectangular array.
        /// </summary>
        /// <param name="array"></param>
        public void Print(int[][] array)
        {
            var maxLength = 0;

            for (int x = 0; x < array.Length; x++)
            {
                for (int y = 0; y < array[x].Length; y++)
                {
                    var len = array[x][y].ToString().Length;
                    if (len > maxLength)
                        maxLength = len;
                }
            }

            maxLength += 2;
            var left = (int)Math.Floor(maxLength / 2f);
            var lines = (int)Math.Ceiling(maxLength / 3f);

            var sb = new StringBuilder();

            for (int x = 0; x < array.Length; x++)
            {
                sb.Append("  ");
                for (int y = 0; y < array[x].Length; y++)
                {
                    sb.Append(array[x][y].ToString().PadLeft(left).PadRight(maxLength));
                }
                if (x != array.Length - 1)
                    for (int i = 0; i < lines; i++)
                        sb.AppendLine();
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
