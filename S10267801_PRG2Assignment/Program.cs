//==========================================================
// Student Number  : S10267801
// Student Name  : Peh Ya An
// Partner Name  : De Roza Ariel Therese
//==========================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                BoardingGate boardingGate = new BoardingGate(data[0], bool.Parse(data[2]), bool.Parse(data[1]), bool.Parse(data[3]));
                boardingGates.Add(data[0].Trim(), boardingGate);  // Use Gate Name as the key
            }

            Dictionary<string, string> airlineCodes = new Dictionary<string, string>();
            string[] airlineLines = File.ReadAllLines(path + "/airlines.csv");
            List<string> codes = new List<string>();
            List<string> names = new List<string>();
            for (int i = 1; i < airlineLines.Length; i++)
            {
                string[] data = airlineLines[i].Split(',');
                airlineCodes.Add(data[0].Trim(), data[1].Trim());
                string airlineName = data[0].Trim();
                names.Add(airlineName);
                string airlineCode = data[1].Trim();
                codes.Add(airlineCode);
                airlineCodes[airlineCode] = airlineName;
            }

            List<string> numbers = new List<string>();
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
                numbers.Add(flightNumber);
                // Get airline code (first two characters of flight number)
                string airlineCode = flightNumber.Substring(0, 2);

                // Find airline name using the code
                string airlineName = "Unknown";
                if (airlineCodes.ContainsKey(airlineCode))
                {
                    airlineName = airlineCodes[airlineCode];  // Use the dictionary to get the airline name
                }
                string status = "Scheduled";
                string assignedGate = "Unassigned";

                // Add flight to the dictionary with flight number as the key
                Flight flight = new Flight(flightNumber, airlineName, origin, destination, expectedTime, status, specialRequestCode, assignedGate);
                flights.Add(flightNumber, flight);
            }

            Terminal terminal = new Terminal("Terminal 5",
                new Dictionary<string, Airline>(),
                flights,
                boardingGates,
                new Dictionary<string, double>());

            Queue<Flight> unassignedFlights = new Queue<Flight>();
            int unassignedFlightCount;
            int unassignedBoardingGateCount;

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
                    Console.WriteLine("8. Process All Unassigned Flights to Boarding Gates");
                    Console.WriteLine("0. Exit");
                    Console.Write("Please select an option: ");
                    int option = Convert.ToInt32(Console.ReadLine());

                    if (option < 0 || option > 8)
                    {
                        Console.WriteLine("Invalid option. Please select a number between 0 and 8.");
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
                            while (flights.ContainsKey(flightNumber))
                            {
                                Console.WriteLine($"Flight number {flightNumber} not found. Please try again.");
                                Console.WriteLine("Enter Flight Number:");
                                flightNumber = Console.ReadLine();
                                selectedFlight = null;
                            }
                            selectedFlight = flights[flightNumber];



                            Console.WriteLine("Enter Boarding Gate Name:");
                            string boardingGate = Console.ReadLine();
                            BoardingGate selectedGate = null;
                            while (boardingGates.ContainsKey(boardingGate))
                            {
                                Console.WriteLine("Boarding gate not found.");
                                Console.WriteLine("Enter Boarding Gate Name:");
                                boardingGate = Console.ReadLine();
                                selectedGate = null;
                            }
                            selectedGate = boardingGates[boardingGate];
                            Console.WriteLine($"Selected Gate: {selectedGate.GateName}");

                            // Check if the boarding gate is already assigned to another flight
                            if (selectedGate.Flight != null)
                            {
                                Console.WriteLine($"Boarding Gate {boardingGate} is already assigned to Flight {selectedGate.Flight.FlightNumber}. Please try again.");
                            }
                            else
                            {
                                selectedGate.Flight = selectedFlight;
                                selectedFlight.AssignedGate = selectedGate.GateName;
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
                            if (response == "N")
                            {
                                selectedFlight.Status = "On Time";
                            }
                            else
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
                            Flight newFlight = new Flight(flightNumber, airlineName, origin, destination, expectedTime, "Scheduled", specialRequestCode, "Unassigned");
                            flights[flightNumber] = newFlight;

                            using (StreamWriter sw = new StreamWriter(path + "/flights.csv", true))
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
                    if (option == 5)
                    {
                        Console.WriteLine("=============================================");
                        Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
                        Console.WriteLine("=============================================");
                        Console.WriteLine("Airline Code    Airline Name");
                        for (int i = 0; i < codes.Count; i++)
                        {
                            Console.WriteLine($"{codes[i],-16}{names[i],-12}");
                        }
                        Console.Write("Enter Airline Code: ");
                        string entername = Console.ReadLine();
                        int check = 0;
                        for (int i = 0; i < codes.Count; i++)
                        {
                            if (codes[i] == entername)
                            {
                                check = 1;
                            }
                        }
                        while (check == 0 || entername.Length != 2)
                        {
                            if (check == 0)
                            {
                                Console.WriteLine("This is not a valid airline. Please try again");
                                Console.Write("Enter Airline Code: ");
                                entername = Console.ReadLine();
                                for (int i = 0; i < codes.Count; i++)
                                {
                                    if (codes[i] == entername)
                                    {
                                        check = 1;
                                    }
                                }
                            }
                            if (entername.Length != 2)
                            {
                                Console.WriteLine("This is not a 2 character string. Please try again");
                                Console.Write("Enter Airline Code: ");
                                entername = Console.ReadLine();
                            }
                        }
                        int entercode = 0;
                        for (int i = 0; i < codes.Count; i++)
                        {
                            if (codes[i] == entername)
                            {
                                entercode = i;
                            }
                        }
                        Console.WriteLine($"=============================================\r\nList of Flights for {names[entercode]}\r\n=============================================");
                        Console.WriteLine("Flight Number   Airline Name             Origin                    Destination              Expected Departure/Arrival Time");
                        string airlinenamecode = "";
                        List<Flight> fligh = new List<Flight>();
                        foreach (var code in flights.Keys)
                        {
                            airlinenamecode = code.Substring(0, 2);
                            if (airlinenamecode == entername)
                            {
                                fligh.Add(flights[code]);
                            }
                        }
                        foreach (var flight in fligh)
                        {
                            Console.WriteLine(flight);
                        }
                    }
                    if (option == 6)
                    {
                        Console.WriteLine("=============================================");
                        Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
                        Console.WriteLine("=============================================");
                        Console.WriteLine("Airline Code    Airline Name");
                        for (int i = 0; i < codes.Count; i++)
                        {
                            Console.WriteLine($"{codes[i],-16}{names[i],-12}");
                        }
                        Console.Write("Enter Airline Code: ");
                        string entername = Console.ReadLine();
                        int entercode = 0;
                        for (int i = 0; i < codes.Count; i++)
                        {
                            if (codes[i] == entername)
                            {
                                entercode = i;
                            }
                        }
                        Console.WriteLine($"=============================================\r\nList of Flights for {names[entercode]}\r\n=============================================");
                        Console.WriteLine("Flight Number   Airline Name             Origin                    Destination              Expected Departure/Arrival Time");
                        string airlinenamecode = "";
                        List<Flight> fligh = new List<Flight>();
                        foreach (var code in flights.Keys)
                        {
                            airlinenamecode = code.Substring(0, 2);
                            if (airlinenamecode == entername)
                            {
                                fligh.Add(flights[code]);
                            }
                        }
                        foreach (var flight in fligh)
                        {
                            Console.WriteLine(flight);
                        }
                    }
                    if (option == 7)
                    {
                        Console.WriteLine("=============================================");
                        Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
                        Console.WriteLine("=============================================");
                        Console.WriteLine("Flight Number   Airline Name           Origin                 Destination            Expected Departure/Arrival Time     Status          Boarding Gate");
                        List<Flight> sortedFlights = flights.Values.ToList();
                        sortedFlights.Sort();

                        foreach (var flight in sortedFlights)
                        {
                            Console.WriteLine($"{flight.FlightNumber,-15} {flight.AirlineName,-22} {flight.Origin,-22} {flight.Destination,-22} {flight.ExpectedTime,-35} {flight.Status,-15} {flight.AssignedGate}");
                        }
                    }
                    if (option == 8)
                    {
                        unassignedFlightCount = 0;
                        unassignedBoardingGateCount = 0;

                        foreach (var flight in terminal.Flights.Values)
                        {
                            if (flight.AssignedGate == "Unassigned")
                            {
                                unassignedFlights.Enqueue(flight);
                                unassignedFlightCount++;
                            }
                        }
                        Console.WriteLine($"Total number of Flights without a Boarding Gate assigned: {unassignedFlightCount}");

                        foreach (var gate in terminal.BoardingGates.Values)
                        {
                            bool isGateAssigned = false;

                            foreach (var flight in terminal.Flights.Values)
                            {
                                if (flight.AssignedGate == gate.GateName)
                                {
                                    isGateAssigned = true;
                                    break;
                                }
                            }
                            if (!isGateAssigned)
                            {
                                unassignedBoardingGateCount++;
                            }
                        }
                        Console.WriteLine($"Total number of Boarding Gates without a Flight assigned: {unassignedBoardingGateCount}");

                        int processedFlights = 0;
                        int processedGates = 0;
                        Console.WriteLine("=============================================");
                        Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
                        Console.WriteLine("=============================================");
                        Console.WriteLine("Flight Number   Airline Name           Origin                 Destination            Expected Departure/Arrival Time     Status          Boarding Gate");
                        while (unassignedFlights.Count > 0)
                        {
                            Flight currentFlight = unassignedFlights.Dequeue();
                            bool hasSpecialRequest = string.IsNullOrEmpty(currentFlight.SpecialRequestCode);
                            BoardingGate assignedGate = null;

                            foreach (var gate in terminal.BoardingGates.Values)
                            {
                                bool isGateUnassigned = true;
                                foreach (var flight in terminal.Flights.Values)
                                {
                                    if (flight.AssignedGate == gate.GateName)
                                    {
                                        isGateUnassigned = false;
                                        break;
                                    }
                                }

                                if (isGateUnassigned)
                                {
                                    if (hasSpecialRequest)
                                    {
                                        string[] specialRequestCodes = { "CFFT", "DDJB", "LWTT" };
                                        foreach (var code in specialRequestCodes)
                                        {
                                            if (currentFlight.SpecialRequestCode == code)
                                            {
                                                if ((code == "CFFT" && gate.SupportsCFFT) ||
                                                    (code == "DDJB" && gate.SupportsDDJB) ||
                                                    (code == "LWTT" && gate.SupportsLWTT))
                                                {
                                                    assignedGate = gate;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(gate.SpecialRequestCode))
                                        {
                                            assignedGate = gate;
                                        }
                                    }

                                    if (assignedGate != null)
                                    {
                                        break;
                                    }
                                }
                            }

                            if (assignedGate != null)
                            {
                                currentFlight.AssignedGate = assignedGate.GateName;
                                processedFlights++;
                                processedGates++;
                                Console.WriteLine($"{currentFlight.FlightNumber,-15} {currentFlight.AirlineName,-22} {currentFlight.Origin,-22} {currentFlight.Destination,-22} {currentFlight.ExpectedTime,-35} {currentFlight.Status,-15} {currentFlight.AssignedGate}");
                            }
                        }

                        Console.WriteLine($"Total number of Flights processed and assigned: {processedFlights}");
                        Console.WriteLine($"Total number of Boarding Gates processed and assigned: {processedGates}");
                        int totalFlights = terminal.Flights.Count;
                        int totalGates = terminal.BoardingGates.Count;

                        double flightPercentage = processedFlights / totalFlights * 100;
                        double gatePercentage = processedGates / totalGates * 100;
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

