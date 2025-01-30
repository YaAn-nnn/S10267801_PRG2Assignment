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
using System.Security.Cryptography.X509Certificates;

namespace S10267801_PRG2Assignment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CSV_Files");

            Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>();
            string[] lines = File.ReadAllLines(path + "/boardinggates.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(',');

                // Creating BoardingGate object and adding it to the dictionary
                BoardingGate boardingGate = new BoardingGate(data[0], bool.Parse(data[1]), bool.Parse(data[2]), bool.Parse(data[3]));
                boardingGates.Add(data[0].Trim(), boardingGate);  // Use Gate Name as the key
            }

            Dictionary<string, string> airlineCodes = new Dictionary<string, string>();
            string[] airlineLines = File.ReadAllLines(path + "/airlines.csv");
            for (int i = 1; i < airlineLines.Length; i++)
            {
                string[] data = airlineLines[i].Split(',');
                airlineCodes.Add(data[0].Trim(), data[1].Trim());
                string airlineName = data[0].Trim();
                string airlineCode = data[1].Trim();
                airlineCodes[airlineCode] = airlineName;
            }


            Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
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
                if (airlineCodes.ContainsKey(airlineCode))
                {
                    airlineName = airlineCodes[airlineCode];  // Use the dictionary to get the airline name
                }

                string status = "On Time";  // Default status for feature 5 (Option 3)

                // Add flight to the dictionary with flight number as the key
                Flight flight = new Flight(flightNumber, airlineName, origin, destination, expectedTime, status, specialRequestCode);
                flights.Add(flightNumber, flight);

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
                        foreach (var flight in flights.Values)
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
                        foreach (var boardingGate in boardingGates.Values)
                        {
                            Console.WriteLine(boardingGate.ToString());
                        }
                    }
                    if (option == 3)
                    {
                        while (true)
                        {
                            Console.WriteLine("=============================================");
                            Console.WriteLine("Assign a Boarding Gate to a Flight");
                            Console.WriteLine("=============================================");
                            Console.WriteLine("Enter Flight Number:");
                            string flightNumber = Console.ReadLine();
                            Flight selectedFlight = null;
                            if (flights.ContainsKey(flightNumber))
                            {
                                selectedFlight = flights[flightNumber];
                            }
                            else
                            {
                                Console.WriteLine($"Flight number {flightNumber} not found. Please try again.");
                            }

                            Console.WriteLine("Enter Boarding Gate Name:");
                            string boardingGate = Console.ReadLine();
                            BoardingGate selectedGate = null;
                            if (boardingGates.ContainsKey(boardingGate))
                            {
                                selectedGate = boardingGates[boardingGate];
                                Console.WriteLine($"Selected Gate: {selectedGate.GateName}");
                            }
                            else
                            {
                                Console.WriteLine("Boarding gate not found.");
                            }

                            // Check if the boarding gate is already assigned to another flight
                            if (selectedGate.Flight != null)
                            {
                                Console.WriteLine($"Boarding Gate {boardingGate} is already assigned to Flight {selectedGate.Flight.FlightNumber}. Please try again.");
                            }

                            // Assign flight to gate
                            selectedGate.Flight = selectedFlight;

                            Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
                            Console.WriteLine($"Origin: {selectedFlight.Origin}");
                            Console.WriteLine($"Destination: {selectedFlight.Destination}");
                            Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime}");
                            if (string.IsNullOrEmpty(selectedFlight.SpecialRequestCode))
                            {
                                selectedFlight.SpecialRequestCode = "None";
                            }
                            Console.WriteLine($"Special Request Code: {selectedFlight.SpecialRequestCode}");

                            Console.WriteLine($"Boarding Gate Name: {selectedGate.GateName}");
                            Console.WriteLine($"Supports DDJB: {selectedGate.SupportsDDJB}");
                            Console.WriteLine($"Supports CFFT: {selectedGate.SupportsCFFT}");
                            Console.WriteLine($"Supports LWTT: {selectedGate.SupportsLWTT}");

                            Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
                            string response = Console.ReadLine().ToUpper();
                            if (response == "Y")
                            {
                                Console.WriteLine("1. Delayed");
                                Console.WriteLine("2. Boarding");
                                Console.WriteLine("3. On Time");
                                Console.WriteLine("Please select the new status of the flight:");
                                int statusResponse = Convert.ToInt32(Console.ReadLine());
                                if (statusResponse == 1)
                                {
                                    selectedFlight.Status = "Delayed";
                                }
                                if (statusResponse == 2)
                                {
                                    selectedFlight.Status = "Boarding";
                                }
                                if (statusResponse == 3)
                                {
                                    selectedFlight.Status = "On Time";
                                }
                                else
                                {
                                    Console.WriteLine("Please enter a number from 1 to 3.");
                                }
                            }
                            if (response != "Y" || response != "N")
                            {
                                Console.WriteLine("Please enter Y or N.");
                            }
                            Console.WriteLine($"Flight {selectedFlight.FlightNumber} has been assigned to Boarding Gate {selectedGate.GateName}!");
                            break;
                        }


                    }
                    if (option == 4)
                    {
                        while (true)
                        {
                            Console.Write("Enter Flight Number: ");
                            string flightNumber = Console.ReadLine();

                            Console.Write("Enter Origin: ");
                            string origin = Console.ReadLine();

                            Console.Write("Enter Destination: ");
                            string destination = Console.ReadLine();

                            Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                            DateTime expectedTime;
                            while (!DateTime.TryParseExact(Console.ReadLine(), "d/M/yyyy H:mm", null, System.Globalization.DateTimeStyles.None, out expectedTime))
                            {
                                Console.WriteLine("Invalid format. Please enter the date and time in (dd/mm/yyyy hh:mm) format:");
                            }

                            Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                            string specialRequestCode = Console.ReadLine();
                            if (specialRequestCode == "None")
                            {
                                specialRequestCode = null;
                            }


                            // Get airline code (first two characters of flight number)
                            string airlineCode = flightNumber.Substring(0, 2);

                            string airlineName = "Unknown";
                            if (airlineCodes.ContainsKey(airlineCode))
                            {
                                airlineName = airlineCodes[airlineCode];
                            }
                            Flight newFlight = new Flight(flightNumber, airlineName, origin, destination, expectedTime, "On Time", specialRequestCode);
                            flights[flightNumber] = newFlight;

                            using (StreamWriter sw = new StreamWriter(path+"/flights.csv", true))
                            {
                                sw.WriteLine($"{flightNumber},{origin},{destination},{expectedTime:HH:mm tt},{specialRequestCode}");
                            }
                            Console.WriteLine($"Flight {flightNumber} has been added!");

                            Console.WriteLine("Would you like to add another flight? (Y/N)");
                            string response = Console.ReadLine().ToUpper();

                            if (response == "N")
                            {
                                break;
                            }
                        }
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

