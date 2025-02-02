//==========================================================
// Student Number  : S10267801
// Student Name  : Peh Ya An
// Partner Name  : De Roza Ariel Therese
//==========================================================

using Microsoft.SqlServer.Server;
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
            int SQ = 0; int MH = 0; int JL = 0; int CX = 0; int QF = 0; int TR = 0; int EK = 0; int BA = 0;
            List<string> numbers = new List<string>();
            List<Array> flighting = new List<Array>();
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
                if (airlineCode == "SQ") // To store how many flights in each airline
                {SQ += 1;} else if (airlineCode == "MH")
                {MH += 1;} else if (airlineCode == "JL")
                {JL += 1;} else if (airlineCode == "CX")
                {CX += 1;} else if (airlineCode == "QF")
                {QF += 1;} else if (airlineCode == "TR")
                {TR += 1;} else if (airlineCode == "EK")
                {EK += 1;} else if (airlineCode == "BA")
                {BA += 1;}

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
                String times = Convert.ToString(expectedTime);
                string[] flighter = { flightNumber, airlineName, origin, destination, times, status, specialRequestCode, assignedGate };
                flighting.Add(flighter);
            }

            Terminal terminal = new Terminal("Terminal 5",
                new Dictionary<string, Airline>(),
                flights,
                boardingGates,
                new Dictionary<string, double>());
            Flight flightings = new Flight(); // To access the base Fee
            Queue<Flight> unassignedFlights = new Queue<Flight>();
            int unassignedFlightCount;
            int unassignedBoardingGateCount;
            CFFTFlight cFFT = new CFFTFlight(); // To access CalculateFees in the class
            DDJBFlight dDJB = new DDJBFlight();
            LWTTFlight lWTT = new LWTTFlight();
            Dictionary<string,int> airliners = new Dictionary<string,int>();
            airliners.Add("SQ", SQ); airliners.Add("MH", MH); airliners.Add("JL", JL); airliners.Add("CX", CX); airliners.Add("QF", QF); airliners.Add("TR", TR); airliners.Add("EK", EK); airliners.Add("BA", BA); 
            // To assign the value of how many flights there are in each airline to its own key
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
                    Console.WriteLine("9. Display Total Fee per Airline");
                    Console.WriteLine("0. Exit");
                    Console.Write("Please select an option: ");
                    int option = Convert.ToInt32(Console.ReadLine());

                    if (option < 0 || option > 9)
                    {
                        Console.WriteLine("Invalid option. Please select a number between 0 and 9.");
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
                            while (!flights.ContainsKey(flightNumber))
                            {
                                Console.WriteLine($"Flight number {flightNumber} not found. Please try again.");
                                Console.WriteLine("Enter Flight Number:");
                                flightNumber = Console.ReadLine();
                            }
                            selectedFlight = flights[flightNumber];



                            Console.WriteLine("Enter Boarding Gate Name:");
                            string boardingGate = Console.ReadLine();
                            BoardingGate selectedGate = null;
                            while (!boardingGates.ContainsKey(boardingGate))
                            {
                                Console.WriteLine("Boarding gate not found.");
                                Console.WriteLine("Enter Boarding Gate Name:");
                                boardingGate = Console.ReadLine();
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
                                // Check if there is a special request code
                                if (string.IsNullOrEmpty(selectedFlight.SpecialRequestCode))
                                {
                                    // If no special request, assign the gate
                                    selectedGate.Flight = selectedFlight;
                                    selectedFlight.AssignedGate = selectedGate.GateName;
                                }
                                else
                                {
                                    string[] specialRequestCodes = { "CFFT", "DDJB", "LWTT" };

                                    // Check if the SpecialRequestCode matches one of the special codes
                                    bool isSpecialCodeValid = false;
                                    foreach (var code in specialRequestCodes)
                                    {
                                        if (selectedFlight.SpecialRequestCode == code)
                                        {
                                            if ((code == "CFFT" && selectedGate.SupportsCFFT) ||
                                                (code == "DDJB" && selectedGate.SupportsDDJB) ||
                                                (code == "LWTT" && selectedGate.SupportsLWTT))
                                            {
                                                isSpecialCodeValid = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (isSpecialCodeValid)
                                    {
                                        // If the special request is valid and gate supports it, assign the gate
                                        selectedGate.Flight = selectedFlight;
                                        selectedFlight.AssignedGate = selectedGate.GateName;
                                    }
                                    else
                                    {
                                        // Handle the case where the special request code isn't valid
                                        Console.WriteLine($"Special request {selectedFlight.SpecialRequestCode} not supported by Gate {selectedGate.GateName}. Please choose a different gate.");
                                        continue;
                                    }
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
                                else if (statusResponse == 2)
                                {
                                    selectedFlight.Status = "Boarding";
                                }
                                else if (statusResponse == 3)
                                {
                                    selectedFlight.Status = "On Time";
                                }
                                else
                                {
                                    Console.WriteLine("Please enter a number from 1 to 3.");
                                }
                            }
                            else if (response == "N")
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

                            // List of valid prefixes
                            string[] validPrefixes = { "SQ", "MH", "JL", "CX", "QF", "TR", "EK", "BA" };

                            // Keep asking until a valid flight number is entered
                            while (!validPrefixes.Contains(flightNumber.Substring(0, 2)) || flights.ContainsKey(flightNumber))
                            {
                                if (!validPrefixes.Contains(flightNumber.Substring(0, 2)))
                                {
                                    Console.WriteLine("Invalid flight number prefix. It must start with SQ, MH, JL, CX, QF, TR, EK, or BA.");
                                }
                                else if (flights.ContainsKey(flightNumber))
                                {
                                    Console.WriteLine("This flight number already exists. Please try again.");
                                }

                                Console.Write("Enter Flight Number: ");
                                flightNumber = Console.ReadLine();
                            }


                            Console.Write("Enter Origin: ");
                            string origin = Console.ReadLine();

                            Console.Write("Enter Destination: ");
                            string destination = Console.ReadLine();

                            while (origin == destination)
                            {
                                Console.WriteLine("Origin and destination cannot be the same. Please enter again.");

                                Console.Write("Enter Origin: ");
                                origin = Console.ReadLine();

                                Console.Write("Enter Destination: ");
                                destination = Console.ReadLine();
                            }

                            Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                            DateTime expectedTime;
                            while (!DateTime.TryParseExact(Console.ReadLine(), "d/M/yyyy H:mm", null, System.Globalization.DateTimeStyles.None, out expectedTime))
                            {
                                Console.WriteLine("Invalid format. Please enter the date and time in (dd/mm/yyyy hh:mm) format:");
                            }

                            Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                            string specialRequestCode = Console.ReadLine();
                            while (specialRequestCode != "LWTT" || specialRequestCode != "CFFT" || specialRequestCode != "DDJB" || specialRequestCode != "None")
                            {
                                Console.WriteLine("That is an invalid input. Please input either 'LWTT', 'CFFT', 'DDJB' or 'None'.");
                                Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                                specialRequestCode = Console.ReadLine();
                            }
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
                            Console.WriteLine($"{codes[i],-16}{names[i],-12}"); // Prints the airline code and airline name
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
                                entercode = i; // Finds the airline name that corresponds to the airline code
                            }
                        }
                        Console.WriteLine($"=============================================\r\nList of Flights for {names[entercode]}\r\n=============================================");
                        Console.WriteLine("Flight Number   Airline Name             Origin                    Destination              Expected Departure/Arrival Time");
                        List<Flight> fligh = new List<Flight>();
                        foreach (var code in flights.Keys)
                        {
                            string airlinenamecode = code.Substring(0, 2); // Makes sure that the flight number airline code is only the one input
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
                            Console.WriteLine($"{codes[i],-16}{names[i],-12}"); // Prints the airline code and airline name
                        }
                        Console.Write("Enter Airline Code: ");
                        string entername = Console.ReadLine();
                        int check = 0;
                        for (int i = 0; i < codes.Count; i++) // Validation
                        {
                            if (codes[i] == entername)
                            {
                                check = 1; // If code entered is not in the list of codes, the check will forever remain 0
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
                            airlinenamecode = code.Substring(0, 2); // To get just the Airline Code
                            if (airlinenamecode == entername)
                            {
                                fligh.Add(flights[code]); // Adds only the flights with matching flight number into the list
                            }
                        }
                        foreach (var flight in fligh)
                        {
                            Console.WriteLine(flight);
                        }
                        Console.Write("Choose an existing Flight to modify or delete: ");
                        string choice = Console.ReadLine();
                        int check2 = 0;
                        foreach (var code in flights.Keys)
                        {
                            if (choice == code)
                            {
                                check2 = 1;
                            }
                        }
                        while (check2 == 0)
                        {
                            Console.WriteLine("This is not a valid flight number. Please try again");
                            Console.Write("Choose an existing Flight to modify or delete: ");
                            choice = Console.ReadLine();
                            foreach (var code in flights.Keys)
                            {
                                if (choice == code)
                                {
                                    check2 = 1;
                                }
                            }
                        }
                        Console.WriteLine("1. Modify Flight \r\n2. Delete Flight");
                        Console.Write("Choose an Option: ");
                        int choice2 = Convert.ToInt32(Console.ReadLine());
                        while (choice2 > 2 || choice2 < 1)
                        {
                            Console.WriteLine("That is not a valid choice. Pleae try again.");
                            Console.WriteLine("1. Modify Flight \r\n2. Delete Flight");
                            Console.Write("Choose an Option: ");
                            choice2 = Convert.ToInt32(Console.ReadLine());
                        }
                        if (choice2 == 1)
                        {
                            List<string> list = new List<string>();
                            int checker = 0;
                            int checkedif = 0;
                            foreach (var flight in flighting)
                            {
                                string code = Convert.ToString(flight.GetValue(0));
                                if (code == choice)
                                {
                                    checkedif = checker; // Contains the value/index at which the Flight number input is in the List 
                                    for (int i = 0; i < flight.Length; i++)
                                    {
                                        list.Add(Convert.ToString(flight.GetValue(i)));
                                        Console.WriteLine(flight.GetValue(i));
                                    }
                                }
                                checker++; 
                            }
                            Console.WriteLine("1. Modify Basic Information\r\n2. Modify Status\r\n3. Modify Special Request Code\r\n4. Modify Boarding Gate");
                            Console.Write("Choose an Option: ");
                            int choice3 = Convert.ToInt32(Console.ReadLine());
                            while (choice3 > 4 || choice3 < 1) // Validation
                            {
                                Console.WriteLine("That is not a valid choice. Pleae try again.");
                                Console.WriteLine("1. Modify Basic Information\r\n2. Modify Status\r\n3. Modify Special Request Code\r\n4. Modify Boarding Gate");
                                Console.Write("Choose an Option: ");
                                choice3 = Convert.ToInt32(Console.ReadLine());
                            }
                            if (choice3 == 1)
                            {
                                Console.Write("Enter new Origin: ");
                                string originnew = Console.ReadLine();
                                Console.Write("Enter new Destination: ");
                                string destinationnew = Console.ReadLine();
                                Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                                string EDATnew = Console.ReadLine();

                                list[2] = originnew;
                                list[3] = destinationnew;
                                list[4] = EDATnew; // Replaces previous origin, destination and estimated deaparture/arrival time with updated ones
                                flighting[checkedif] = list.ToArray();
                                Console.WriteLine("Flight Updated!");
                                Console.WriteLine($"Flight Number: {list[0]}\r\nAirline Name: {list[1]}\r\nOrigin: {list[2]}\r\nDestination: {list[3]}\r\nExpected Departure/Arrival Time: {Convert.ToDateTime(list[4])}\r\nStatus: {list[5]}\r\nSpecial Request Code: {list[6]}\r\nBoarding Gate: {list[7]}");
                            }
                            else if (choice3 == 2)
                            {
                                Console.Write("Enter new Status: ");
                                string statusnew = Console.ReadLine();
                                while (statusnew != "On Time" || statusnew != "Boarding" || statusnew != "Delayed")
                                {
                                    Console.WriteLine("That is an invalid input. Please input either 'Boarding', 'On Time' or 'Delayed'.");
                                    Console.Write("Enter new Status: ");
                                    statusnew = Console.ReadLine();
                                }
                                list[5] = statusnew; // Replaces previous status with updated one
                                flighting[checkedif] = list.ToArray();
                                Console.WriteLine("Flight Updated!");
                                Console.WriteLine($"Flight Number: {list[0]}\r\nAirline Name: {list[1]}\r\nOrigin: {list[2]}\r\nDestination: {list[3]}\r\nExpected Departure/Arrival Time: {Convert.ToDateTime(list[4])}\r\nStatus: {list[5]}\r\nSpecial Request Code: {list[6]}\r\nBoarding Gate: {list[7]}");
                            }
                            else if (choice3 == 3)
                            {
                                Console.Write("Enter new Special Request Code (If there is none, please input 'None'): ");
                                string SRCnew = Console.ReadLine();
                                while (SRCnew != "LWTT" || SRCnew != "CFFT" || SRCnew != "DDJB" || SRCnew != "None")
                                {
                                    Console.WriteLine("That is an invalid input. Please input either 'LWTT', 'CFFT', 'DDJB' or 'Non'.");
                                    Console.Write("Enter new Special Request Code (If there is none, please input 'None'): ");
                                    SRCnew = Console.ReadLine();
                                }
                                if (SRCnew == "None")
                                {
                                    SRCnew = null; // Makes the Special Request Code null if user input is None
                                }
                                list[6] = SRCnew; // Replaces previous Special Request Code with updated one
                                flighting[checkedif] = list.ToArray();
                                Console.WriteLine("Flight Updated!");
                                Console.WriteLine($"Flight Number: {list[0]}\r\nAirline Name: {list[1]}\r\nOrigin: {list[2]}\r\nDestination: {list[3]}\r\nExpected Departure/Arrival Time: {Convert.ToDateTime(list[4])}\r\nStatus: {list[5]}\r\nSpecial Request Code: {list[6]}\r\nBoarding Gate: {list[7]}");
                            }
                            else if (choice3 == 4)
                            {
                                Console.Write("Enter new Boarding Gate: ");
                                string BGnew = Console.ReadLine();
                                while (BGnew.Length >3 || BGnew.Length < 2)
                                {
                                    Console.WriteLine("That is an invalid input. Please input a boarding gate with at least two characters and up to three characters");
                                    Console.Write("Enter new Boarding Gate: ");
                                    BGnew = Console.ReadLine();
                                }
                                while (BGnew[0] != 'A' || BGnew[0] != 'B' || BGnew[0] != 'C')
                                {
                                    Console.WriteLine("That is an invalid boarding gate. Please only input Gates that start with 'A', 'B' or 'C'.");
                                    Console.Write("Enter new Boarding Gate: ");
                                    BGnew = Console.ReadLine();
                                }
                                List<string> integerchecker = new List<string>();
                                for (int i = 0; i < BGnew.Length; i++) 
                                {
                                    if (i == 1)
                                    {
                                        integerchecker.Add(Convert.ToString(BGnew[i])); // Adds the the number of the boarding gate to the list
                                    }
                                    if (i == 2)
                                    {
                                        integerchecker[0] += Convert.ToString(BGnew[i]); // If the number is two digits, add the second number to the first number
                                    }
                                }
                                
                                while (Convert.ToInt32(integerchecker[0]) < 1 || Convert.ToInt32(integerchecker[0]) > 22) // Checks if the number of the boarding gate is between 1 and 22
                                {
                                    Console.WriteLine("That is an invalid boarding gate. Please only input Gates that end with the numbers in between 1 and 22.");
                                    Console.Write("Enter new Boarding Gate: ");
                                    BGnew = Console.ReadLine();
                                }
                                list[7] = BGnew; // Replaces previous boarding gate with updated one
                                flighting[checkedif] = list.ToArray();
                                Console.WriteLine("Flight Updated!");
                                Console.WriteLine($"Flight Number: {list[0]}\r\nAirline Name: {list[1]}\r\nOrigin: {list[2]}\r\nDestination: {list[3]}\r\nExpected Departure/Arrival Time: {Convert.ToDateTime(list[4])}\r\nStatus: {list[5]}\r\nSpecial Request Code: {list[6]}\r\nBoarding Gate: {list[7]}");
                            }
                        }
                        else if (choice2 == 2)
                        {
                            foreach (var code in flights.Keys)
                            {
                                if (code == choice) // Finds flightnumber in dictionary
                                {
                                    flights.Remove(code);
                                    Console.WriteLine("Flight successfully removed.");
                                }
                            }
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
                        
                        //Count unassigned flights and enqueue them for processing
                        foreach (var flight in terminal.Flights.Values)
                        {
                            if (flight.AssignedGate == "Unassigned")
                            {
                                unassignedFlights.Enqueue(flight);
                                unassignedFlightCount++;
                            }
                        }
                        Console.WriteLine($"Total number of Flights without a Boarding Gate assigned: {unassignedFlightCount}");
                        
                        //Count unassigned boarding gates
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

                        // Process each unassigned flight and try to find a suitable gate
                        while (unassignedFlights.Count > 0)
                        {
                            Flight currentFlight = unassignedFlights.Dequeue();
                            bool hasSpecialRequest = !string.IsNullOrEmpty(currentFlight.SpecialRequestCode);
                            BoardingGate assignedGate = null;

                            // Iterate through available gates to find a suitable match
                            foreach (var gate in terminal.BoardingGates.Values)
                            {
                                bool isGateUnassigned = true;

                                // Check if the gate is already assigned
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
                                    // If the flight has a special request, ensure the gate supports it
                                    if (hasSpecialRequest)
                                    {
                                        string[] specialRequestCodes = { "CFFT", "DDJB", "LWTT" };
                                        foreach (var code in specialRequestCodes)
                                        {
                                            if (currentFlight.SpecialRequestCode == code)
                                            {
                                                // Check if the gate supports the required special request
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
                                        // If no special request, assign any available gate
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
                                // Assign the selected gate if a match was found
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
                        double flightPercentage = (double)processedFlights / totalFlights * 100;
                        double gatePercentage = (double)processedGates / totalGates * 100;
                        Console.WriteLine($"Percentage of Flights processed automatically: {flightPercentage:F2}%");
                        Console.WriteLine($"Percentage of Boarding Gates processed automatically: {gatePercentage:F2}%");
                    }
                    if (option == 9)
                    {
                        double totalfirst = 0; // To get the original subtotal altogether with no discounts
                        double totaldiscount = 0; // To get the discount per flight
                        double subtotaldiscount = 0; // To get the discount total through all flights
                        double total = flightings.CalculateFees(); // To get the base total
                        foreach (var flight in terminal.Flights.Values)
                        {
                            if (flight.AssignedGate == "Unassigned") 
                            {
                                Console.WriteLine("There are still flights that are unassigned. Please run option 8 before running this option again");
                                break; // Forces user to run Option 8 before running this option
                            }
                            else
                            {

                                List<string> list = new List<string>(); // To make accessing the objects easier
                                foreach (var flightd in flighting)
                                {
                                    for (int i = 0; i < flightd.Length; i++)
                                    {
                                        list.Add(Convert.ToString(flightd.GetValue(i)));
                                    }
                                }
                                if (list[6] == "CFFT")
                                {
                                    cFFT.CalculateFees();
                                }
                                else if (list[6] == "DDJB")
                                {
                                    dDJB.CalculateFees();
                                }
                                else if (list[6] == "LWTT")
                                {
                                    lWTT.CalculateFees();
                                }
                                if (list[2] == "Singapore (SIN)")
                                {
                                    total += 800;
                                }
                                if (list[3] == "Singapore (SIN)")
                                {
                                    total += 500;
                                }
                                totalfirst += total; // Gets the subtotal before discounts

                                
                                foreach (var code in airliners.Keys)
                                {
                                    if (list[0].Substring(0, 2) == code && airliners[code] > 5) // Finds if the airline has more than 5 flights departing/arriving
                                    {
                                        totaldiscount += total * 0.03;
                                        total = total * 0.97;

                                    }
                                    if (list[0].Substring(0, 2) == code && airliners[code] >= 3) // Finds if the airline has at least 3 flights
                                    {
                                        int discountcheck1 = Convert.ToInt32(Math.Floor(Convert.ToDecimal(airliners[code] / 3)));
                                        total -= discountcheck1 * 350;
                                        totaldiscount += discountcheck1 * 350;
                                    }
                                }
                            }
                        }
                        TimeSpan startTime = new TimeSpan(11, 0, 0); // 11 am
                        TimeSpan endTime = new TimeSpan(21, 0, 0); //9 pm

                        foreach (var flighted in flighting)
                        {
                            string discountcheck2 = Convert.ToString(flighted.GetValue(4)); 
                            DateTime dateTime = Convert.ToDateTime(discountcheck2);
                            TimeSpan discountchecker2 = dateTime.TimeOfDay;
                            if (discountchecker2 >= startTime && discountchecker2 <= endTime) // Checks if the Time of Departure/Arrival is before 11am and after 9pm
                            {
                                total += 0;
                            }
                            else
                            {
                                total -= 110;
                                totaldiscount += 110;
                            }
                            string discountcheck3 = Convert.ToString(flighted.GetValue(2));
                            if (discountcheck3 == "Dubai (DXB)" || discountcheck3 == "Bangkok (BKK)" || discountcheck3 == "Tokyo (NRT)") // Checks if Origin is Dubai, Bangkok or Tokyo
                            {
                                total -= 25;
                                totaldiscount += 25;
                            }
                            else
                            {
                                total += 0;
                                totaldiscount += 0;
                            }
                            if (flighted.GetValue(7) == null) // Checks if there a flight has a Special Request code or not
                            {
                                total -= 50;
                                totaldiscount += 50;
                            }
                            else
                            {
                                total += 0;
                                totaldiscount += 0;
                            }

                        }
                        subtotaldiscount += totaldiscount; // Gets subtotal of discounts
                        double percent = (subtotaldiscount / (totalfirst - subtotaldiscount)) * 100; // Gets percentage of total discount over final subtotal
                        string percentage = percent.ToString("#.##");
                        Console.WriteLine($"The original total was ${totalfirst}");
                        Console.WriteLine($"Subtotal of discounts is ${subtotaldiscount}");
                        Console.WriteLine($"The final total of Airline fees that Terminal 5 will collect is ${totalfirst -subtotaldiscount}");
                        Console.WriteLine($"The percentage of the subtotal discounts over the final total of fees is {percentage}%");
                    }
                }

                catch (FormatException ex)
                {
                    Console.WriteLine("Invalid input. Please enter a number." + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }
            }
        }
    }
}

