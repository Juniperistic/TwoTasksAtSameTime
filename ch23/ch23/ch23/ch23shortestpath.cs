using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************/
/* Program Name:     Ch23.cs                                         	  */
/* Date:             03/22/2022                                        	  */
/* Programmer:       Miranda Morris                                 	  */
/* Class:            CSCI 234                               	          */
/*                                                             		      */
/* Program Description: The purpose of this program is to have two tasks  */
/* occur at the same time such as shortPath and having the user input data*/
/* such as their name.                                                    */
/*                                                                        */
/* Input: the user's name                                                 */
/*                                                                        */
/* Output: "Hello (user's name), welcome to the shortest path program";   */
/*         Distance table for the cities;                                 */
/*         Start Time;                                                    */
/*         Stop Time;                                                     */
/*         Elapsed Time;                                                  */
/*         Minimum Distance;                                              */
/*         Shortest Path;                                                 */
/*                                                                        */
/* Givens: None								                              */
/*                                                                        */
/* Testing Data:                                                          */
/*                                                                        */
/* List the Testing Input Data: miranda;                                  */
/*                              1234;                                     */
/*                              mir123;                                   */
/*                                                                        */
/* List the Testing Output Data:                                          */
/*                             mirandaoutput.jpg;                         */
/*                             1234output.jpg;                            */
/*                             mir123output.jpg                           */
/* (these screeenshots can be found in the zipped project file)           */
/**************************************************************************/


namespace ch23
{
    class ch23shortestpath
    {
        const int CITY_COUNT = 12;
        const int MIN_DIST = 10;
        const int MAX_DIST = 200;
        private readonly static int[,] table = new int[CITY_COUNT + 1, CITY_COUNT + 1];
        static int MinimumDistance;
        private readonly static int[] ShortestPath = new int[CITY_COUNT];

        static void PrintOutPath(int[] path)
        {
            Console.Write("Shortest Path = ");
            Console.Write("{0,3} ", 0);
            PrintOutArray(path);
            Console.Write("{0,3} ", 0);
            Console.WriteLine();
        }

        static void CopyArray(int[] DestArray, int[] SrcArray)
        {
            int i;

            for (i = 0; i < SrcArray.Length; i++)
                DestArray[i] = SrcArray[i];
        }

        static void PrintOutArray(int[] array)
        {
            int i;

            for (i = 0; i < array.Length; i++)
                Console.Write("{0,3} ", array[i]);
        }

        static int CalcDistance(params int[] cities)
        {
            int i;
            int distance;

            distance = table[0, cities[0]];
            for (i = 1; i < cities.Length; i++)
                distance += table[cities[i - 1], cities[i]];
            distance += table[cities[i - 1], cities[0]];

            return distance;
        }

        static void ChkDistance(int[] cities)
        {
            int distance;

            distance = CalcDistance(cities);
            if (distance < MinimumDistance)
            {
                MinimumDistance = distance;
                CopyArray(ShortestPath, cities);
            }
        }

        static int GetRndDist(Random rnd)
        {
            return rnd.Next(MIN_DIST, MAX_DIST + 1);
        }

        static void CreateDistanceTable(int[,] table)
        {
            int r, c;
            Random rnd = new Random();

            for (r = 0; r < table.GetLength(0); r++)
                for (c = 0; c < table.GetLength(1); c++)
                    if (r == c)
                        table[r, c] = 0;
                    else if (r < c)
                        table[r, c] = GetRndDist(rnd);
                    else
                        table[r, c] = table[c, r];
        }

        static void PrintOutTable(int[,] table)
        {
            int r, c;

            Console.WriteLine("Distance table for cities");
            Console.Write("{0,3}", ' ');
            for (c = 0; c < table.GetLength(1); c++)
                Console.Write("{0,3} ", c);
            Console.WriteLine();

            Console.Write("  +");
            for (c = 0; c < table.GetLength(1) * 4 - 1; c++)
                Console.Write("-");
            Console.WriteLine();

            for (r = 0; r < table.GetLength(0); r++)
            {
                Console.Write("{0,2}|", r);
                for (c = 0; c < table.GetLength(1); c++)
                    Console.Write("{0,3} ", table[r, c]);
                Console.WriteLine();
            }
        }

        static void Swap(int[] array, int n1, int n2)
        {
            int t;

            t = array[n1];
            array[n1] = array[n2];
            array[n2] = t;
        }

        static void Permutations(int[] array, int size, int i)
        {
            int j;

            if (i == size)
            {
                ChkDistance(array);
            }
            else
            {
                for (j = i; j < size; j++)
                {
                    Swap(array, i, j);
                    Permutations(array, size, i + 1);
                    Swap(array, i, j);
                }
            }
        }

        static void FindTheShortestPath()
        {
            int[] cities = new int[CITY_COUNT];
            DateTime start;
            DateTime stop;
            TimeSpan ElapsedTime;
            int i;

            start = DateTime.Now;

            CreateDistanceTable(table);

            for (i = 0; i < cities.Length; i++) cities[i] = i + 1;

            CopyArray(ShortestPath, cities);

            MinimumDistance = CalcDistance(cities);

            Permutations(cities, cities.Length, 0);

            stop = DateTime.Now;

            PrintOutTable(table);
            Console.WriteLine();

            Console.WriteLine("Start Time = {0} ", start);
            Console.WriteLine();

            Console.WriteLine("Stop Time = {0} ", stop);
            Console.WriteLine();

            ElapsedTime = stop.Subtract(start);
            Console.WriteLine("Elapsed Time = {0} ", ElapsedTime);
            Console.WriteLine();

            Console.Write("Minimum Distance = {0} miles", MinimumDistance);
            Console.WriteLine();

            PrintOutPath(ShortestPath);
            Console.WriteLine();
            Console.ReadLine();
        }

        static void NameofUser()
        {
            string Insert;
            Console.WriteLine("Please enter your name:");
            Insert = Convert.ToString(Console.ReadLine());

            Console.WriteLine("Hello {0}, welcome to the shortest path program!", Insert);
        }
        static void Main(string[] args)
        {
            Task taskA = new Task(() => FindTheShortestPath()); 
            taskA.Start();

            NameofUser(); 

            taskA.Wait();
        }
    }
}
