using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace Exam
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
                var line = Console.ReadLine().Split().Select(char.Parse).ToArray();
                matrix[i] = line;
            }

            while (true)
            {
                var command = Console.ReadLine();
                if (command == "Gond")
                {
                    break;
                }

                var tockens = command.Split();
                if (tockens[0] == "Find")
                {
                    var row = int.Parse(tockens[1]) - 1;
                    var col = int.Parse(tockens[2]) - 1;

                    if (row > n || row < 0 || col < 0 || matrix[row].Length < col)
                    {
                        break;
                    }
                    if (matrix[row][col] != 'T' && matrix[row][col] != '-')
                    {
                        continue;
                    }

                    if (matrix[row][col] == 'T')
                    {
                        matrix[row][col] = '-';
                        collectedT++;
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (tockens[0] == "Opponent")
                {
                    var row = int.Parse(tockens[1]) - 1;
                    var col = int.Parse(tockens[2]) - 1;
                    var direction = tockens[3];

                    if (row >= 0 && row < matrix.GetLength(0) && col >= 0 && col < matrix[row].Length)
                    {
                        continue;
                    }
                    if (matrix[row][col] != 'T' && matrix[row][col] != '-')
                    {
                        continue;
                    }

                    if (matrix[row][col] == 'T')
                    {
                        matrix[row][col] = '-';
                        opponentsT++;
                    }

                    if (direction == "up")
                    {
                        for (int i = 1; i <= 3; i++)
                        {
                            if (matrix[row - i].Length < col || (row - i) <= 0)
                            {
                                break;
                            }
                            
                            if (matrix[row - i][col] != 'T' && matrix[row - i][col] != '-')
                            {
                                break;
                            }

                            if (matrix[row - i][col] == 'T')
                            {
                                matrix[row - i][col] = '-';
                                opponentsT++;
                            }
                        }
                    }
                    else if (direction == "down")
                    {
                        for (int i = 1; i <= 3; i++)
                        {
                            if (matrix[row + i][col] != 'T' || matrix[row + i][col] != '-')
                            {
                                break;
                            }

                            if (matrix[row + i][col] == 'T')
                            {
                                matrix[row + i][col] = '-';
                                opponentsT++;
                            }
                        }
                    }
                    else if (direction == "left")
                    {
                        for (int i = 1; i <= 3; i++)
                        {
                            if (matrix[row][col - i] != 'T' || matrix[row][col - i] != '-')
                            {
                                break;
                            }

                            if (matrix[row][col - i] == 'T')
                            {
                                matrix[row][col - i] = '-';
                                opponentsT++;
                            }
                        }
                    }
                    else if (direction == "right")
                    {
                        for (int i = 1; i <= 3; i++)
                        {
                            if (matrix[row][col + i] != 'T' || matrix[row][col + i] != '-')
                            {
                                break;
                            }

                            if (matrix[row][col + i] == 'T')
                            {
                                matrix[row][col + i] = '-';
                                opponentsT++;
                            }
                        }
                    }
                }
            }

            foreach (var item in matrix)
            {
                Console.WriteLine(string.Join(" ", item));
            }
        }
    }
}
