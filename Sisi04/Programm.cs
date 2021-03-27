using System;
using System.Threading;
using System.Text;
using System.Collections.Generic;

namespace Sisi04
{
    class Programm
    {
        public static char[,] matrix;
        public static List<int> xSnake;
        public static List<int> ySnake;

        public static char direction;
        public static int addCounter = 1;
        public static int speed = 600;
        public static int level = 1;
        public static int points = 0;

        static void Main()
        {
            while (!Console.KeyAvailable)
            {
                Console.SetCursorPosition(0, 0);
                Console.CursorVisible = false;

                Console.WriteLine();
                Console.WriteLine("  Navigate with arrows for food!");
                Console.WriteLine();
                Console.WriteLine("  Press any key to continue ...");

                Thread.Sleep(speed);
            }

            Console.Clear();

            matrix = new char[20, 20];
            xSnake = new List<int>();
            ySnake = new List<int>();

            int exitNumber = 0;

            while (true)
            {

                if (level > 4)
                {
                    Console.WriteLine("Success!");
                    break;
                }

                if (exitNumber > 0)
                {
                    break;
                }

                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        matrix[i, j] = ' ';
                    }
                }

                direction = 'R';

                xSnake.Add(0);
                ySnake.Add(0);

                Print();

                RandomFood();

                while (true)
                {
                    if (CheckBorders())
                    {
                        exitNumber++;
                        break;
                    }

                    Thread.Sleep(speed);

                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey().Key;

                        if (key == ConsoleKey.LeftArrow)
                        {
                            if (direction != 'R')
                            {
                                direction = 'L';
                            }
                        }
                        else if (key == ConsoleKey.RightArrow)
                        {
                            if (direction != 'L')
                            {
                                direction = 'R';
                            }
                        }
                        else if (key == ConsoleKey.UpArrow)
                        {
                            if (direction != 'D')
                            {
                                direction = 'U';
                            }
                        }
                        else if (key == ConsoleKey.DownArrow)
                        {
                            if (direction != 'U')
                            {
                                direction = 'D';
                            }
                        }
                        else if (key == ConsoleKey.Escape)
                        {
                            exitNumber++;
                            break;
                        }
                    }

                    Move();

                    if (points >= 100)
                    {
                        points = 0;
                        addCounter = 1;
                        xSnake.Clear();
                        ySnake.Clear();
                        level++;
                        speed -= 50;
                        break;
                    }
                }
            }

            Console.WriteLine("Game over!");

            while (!Console.KeyAvailable)
            {
                Console.SetCursorPosition(0, 25);
                Console.WriteLine("Press any key to close ...");
            }
        }

        private static bool CheckBorders()
        {
            if (xSnake[0] < 0 || ySnake[0] < 0
                    || xSnake[0] >= 20 || ySnake[0] >= 20
                    || matrix[xSnake[0], ySnake[0]] == '*')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void Move()
        {
            switch (direction)
            {
                case 'L':

                    DirectSnake();

                    ySnake[0]--;

                    if (ySnake[0] >= 0)
                    {
                        CheckFood();
                        Print();
                    }

                    break;
                case 'R':

                    DirectSnake();

                    ySnake[0]++;

                    if (ySnake[0] < 20)
                    {
                        CheckFood();
                        Print();
                    }

                    break;
                case 'U':

                    DirectSnake();

                    xSnake[0]--;

                    if (xSnake[0] >= 0)
                    {
                        CheckFood();
                        Print();
                    }

                    break;
                case 'D':

                    DirectSnake();

                    xSnake[0]++;

                    if (xSnake[0] < 20)
                    {
                        CheckFood();
                        Print();
                    }

                    break;
            }
        }

        private static void CheckFood()
        {
            if (matrix[xSnake[0], ySnake[0]] == 'X')
            {
                addCounter = 2;
                RandomFood();
                points += 10;
            }
        }

        private static void RandomFood()
        {
            Random rnd = new Random();

            int x = rnd.Next(0, 20);
            int y = rnd.Next(0, 20);

            while (true)
            {
                if (xSnake.Contains(x) || ySnake.Contains(y))
                {
                    x = rnd.Next(0, 20);
                    y = rnd.Next(0, 20);
                }
                else
                {
                    break;
                }
            }
            
            matrix[x, y] = 'X';
        }

        private static void DirectSnake()
        {
            if (addCounter < 4)
            {
                xSnake.Add(xSnake[xSnake.Count - 1]);
                ySnake.Add(ySnake[ySnake.Count - 1]);
                addCounter++;
            }
            else
            {
                matrix[xSnake[xSnake.Count - 1], ySnake[ySnake.Count - 1]] = ' ';               
            }

            for (int i = xSnake.Count - 1; i > 0; i--)
            {
                xSnake[i] = xSnake[i - 1];
                ySnake[i] = ySnake[i - 1];
            }

        }

        private static void Print()
        {
            matrix[xSnake[0], ySnake[0]] = '+';

            for (int i = 1; i < xSnake.Count; i++)
            {
                matrix[xSnake[i], ySnake[i]] = '*';
            }

            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;

            for (int i = 0; i < 22; i++)
            {
                StringBuilder line = new StringBuilder();

                for (int j = 0; j < 22; j++)
                {
                    if (i == 0 || i == 21)
                    {
                        line.Append('I');

                        if (i == 0 && j == 21)
                        {
                            if (xSnake.Count < 10)
                            {
                                line.Append("  Lenght:   " + xSnake.Count.ToString());
                            }
                            else
                            {
                                line.Append("  Lenght:  " + xSnake.Count.ToString());
                            }
                        }
                    }
                    else
                    {
                        if (j == 0 || j == 21)
                        {
                            line.Append('I');

                            if (i == 2 && j == 21)
                            {
                                line.Append("  Level: " + level.ToString());
                            }

                            if (i == 4 && j == 21)
                            {
                                if (points < 10)
                                {
                                    line.Append("  Points:   " + points.ToString());
                                }
                                else if(points >= 10 && points < 100)
                                {
                                    line.Append("  Points:  " + points.ToString());
                                }
                                else
                                {
                                    line.Append("  Points: " + points.ToString());
                                }
                            }
                        }
                        else
                        {
                            line.Append(matrix[i - 1, j - 1]);
                        }
                    }
                }

                Console.WriteLine(line);
            }
        }
    }
}
