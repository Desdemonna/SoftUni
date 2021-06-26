using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var matrix = new char[n][];
            var collectedT = 0;
            var opponentsT = 0;

            for (int i = 0; i < n; i++)
            {
                var line = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(char.Parse).ToArray();
                matrix[i] = line;
            }

            while (true)
            {
                var command = Console.ReadLine();
                if (command == "Gong")
                {
                    break;
                }

                var tockens = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (tockens[0] == "Find")
                {
                    var row = int.Parse(tockens[1]);
                    var col = int.Parse(tockens[2]);

                    if (IsInArea(row, col, matrix))
                    {
                        if (matrix[row][col] == 'T')
                        {
                            matrix[row][col] = '-';
                            collectedT++;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (tockens[0] == "Opponent")
                {
                    var row = int.Parse(tockens[1]);
                    var col = int.Parse(tockens[2]);
                    var direction = tockens[3];

                    if (IsInArea(row, col, matrix))
                    {
                        if (matrix[row][col] == 'T')
                        {
                            matrix[row][col] = '-';
                            opponentsT++;
                        }
                    }
                    else
                    {
                        continue;
                    }

                    if (direction == "up")
                    {
                        for (int i = 1; i <= 3; i++)
                        {
                            if (IsInArea(row - i, col, matrix))
                            {
                                if (matrix[row - i][col] == 'T')
                                {
                                    matrix[row - i][col] = '-';
                                    opponentsT++;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (direction == "down")
                    {
                        for (int i = 1; i <= 3; i++)
                        {
                            if (IsInArea(row + i, col, matrix))
                            {
                                if (matrix[row + i][col] == 'T')
                                {
                                    matrix[row + i][col] = '-';
                                    opponentsT++;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (direction == "left")
                    {
                        for (int i = 1; i <= 3; i++)
                        {
                            if (IsInArea(row, col - i, matrix))
                            {
                                if (matrix[row][col - i] == 'T')
                                {
                                    matrix[row][col - i] = '-';
                                    opponentsT++;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (direction == "right")
                    {
                        for (int i = 1; i <= 3; i++)
                        {
                            if (IsInArea(row, col + i, matrix))
                            {
                                if (matrix[row][col + i] == 'T')
                                {
                                    matrix[row][col + i] = '-';
                                    opponentsT++;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            for (int row = 0; row < matrix.Length; row++)
            {
                for (int col = 0; col < matrix[row].Length; col++)
                {
                    Console.Write(matrix[row][col] + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine($"Collected tokens: {collectedT}");
            Console.WriteLine($"Opponent's tokens: {opponentsT}");
        }

        private static bool IsInArea(int row, int col, char[][] matrix)
        {
            if (row >= 0 && row < matrix.GetLength(0) && col >= 0 && col < matrix[row].Length)
            {
                return true;
            }
            return false;
        }
    }
}
