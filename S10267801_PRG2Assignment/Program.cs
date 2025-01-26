//==========================================================
// Student Number  : S10267801
// Student Name  : Peh Ya An
// Partner Name  : De Roza Ariel Therese
//==========================================================

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10267801_PRG2Assignment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CSV_Files");
           
            List<BoardingGate> boardingGates = new List<BoardingGate>();
            string[] lines = File.ReadAllLines(path + "/boardinggates.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(',');
                BoardingGate boardingGate = new BoardingGate(data[0], bool.Parse(data[1]), bool.Parse(data[2]), bool.Parse(data[3]));
                boardingGates.Add(boardingGate);
            }
            
            
            List<Flight> flights = new List<Flight>();
            string[] lined = File.ReadAllLines(path + "/flights.csv");
            for (int i = 1; i < lined.Length; i++)
            {
                string[] datas = lined[i].Split(',');
                Flight flight = new Flight(datas[0], datas[1], datas[2], DateTime.Parse(datas[3]), datas[4]);
                flights.Add(flight);
            }

            while (true)
            {
                Console.WriteLine("=============================================");
                Console.WriteLine("Welcome to Changi Airport Terminal 5");
                Console.WriteLine("=============================================");
                Console.WriteLine("1. List All Flights");
                Console.WriteLine("2. List Boarding Gates");
                Console.WriteLine("3. Assign a Boarding Gate to a Flight");
                Console.WriteLine("4. Create Flight");
                Console.WriteLine("5. Display Airline Flights");
                Console.WriteLine("6. Modify Flight Details");
                Console.WriteLine("7. Display Flight Schedule");
                Console.WriteLine("0. Exit");
                Console.Write("Please select an option: ");
                int option = Convert.ToInt32(Console.ReadLine());
                if (option == 0)
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }
                if (option == 1)
                {
                    Console.WriteLine("=============================================");
                    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
                    Console.WriteLine("=============================================");
                    foreach (var flight in flights)
                    {
                        Console.WriteLine(flight.ToString());
                    }

                }
                if (option == 2)
                {
                    Console.WriteLine("=============================================");
                    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
                    Console.WriteLine("=============================================");
                    Console.WriteLine("Gate Name       DDJB                   CFFT                   LWTT");
                    foreach (var boardingGate in boardingGates)
                    {
                        Console.WriteLine(boardingGate.ToString());
                    }
                }
            }
        }
    }
}

