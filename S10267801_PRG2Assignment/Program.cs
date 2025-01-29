﻿//==========================================================
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
using System.Security.Cryptography.X509Certificates;

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

            Dictionary<string, string> airlineCodes = new Dictionary<string, string>();
            string[] airlineLines = File.ReadAllLines(path + "/airlines.csv");
            for (int i = 1; i < airlineLines.Length; i++)
            {
                string[] data = airlineLines[i].Split(',');
                string airlineName = data[0].Trim();
                string airlineCode = data[1].Trim();
                airlineCodes[airlineCode] = airlineName;

            }


            List<Flight> flights = new List<Flight>();
            string[] flightLines = File.ReadAllLines(path + "/flights.csv");
            for (int i = 1; i < flightLines.Length; i++)
            {
                string[] data = flightLines[i].Split(',');
                string flightNumber = data[0].Trim();
                string origin = data[1].Trim();
                string destination = data[2].Trim();
                DateTime expectedTime = DateTime.Parse(data[3].Trim());
                string specialRequestCode = data[4].Trim();

                // Get airline code (first two characters of flight number)
                string airlineCode = flightNumber.Substring(0, 2);

                // Find airline name using the code
                string airlineName = "Unknown";
                foreach (var entry in airlineCodes)
                {
                    if (entry.Key == airlineCode)
                    {
                        airlineName = entry.Value; // Set the airline name if the code matches
                        break; // Exit the loop once the match is found
                    }
                }

                string status = "On Time"; // Default status

                // Add flight to the list
                flights.Add(new Flight(flightNumber, airlineName, origin, destination, expectedTime, status, specialRequestCode));
            }


            while (true)
            {
                try
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

                    if (option < 0 || option > 7)
                    {
                        Console.WriteLine("Invalid option. Please select a number between 0 and 7.");
                        continue;
                    }
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
                        Console.WriteLine("Flight Number   Airline Name             Origin                    Destination              Expected Departure/Arrival Time");
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
                    if (option == 3)
                    {
                        Console.WriteLine("=============================================");
                        Console.WriteLine("Assign a Boarding Gate to a Flight");
                        Console.WriteLine("=============================================");
                        Console.WriteLine("Enter Flight Number:");
                        string flightNumber = Console.ReadLine();
                        Flight selectedFlight = null;
                        foreach (var flight in flights)
                        {
                            if (flight.FlightNumber == flightNumber)
                            {
                                selectedFlight = flight;
                                break;
                            }
                        }

                        Console.WriteLine("Enter Boarding Gate Name:");
                        string boardingGate = Console.ReadLine();
                        BoardingGate selectedGate = null;
                        foreach (var gate in boardingGates)
                        {
                            if (gate.GateName == boardingGate)
                            {
                                selectedGate = gate;
                                break;
                            }
                        }

                        Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
                        Console.WriteLine($"Origin: {selectedFlight.Origin}");
                        Console.WriteLine($"Destination: {selectedFlight.Destination}");
                        Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime}");
                        if (string.IsNullOrEmpty(selectedFlight.SpecialRequestCode))
                        {
                            selectedFlight.SpecialRequestCode = "None";
                        }
                        Console.WriteLine($"Special Request Code: {selectedFlight.SpecialRequestCode}");

                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }
            }
        }
    }
}

