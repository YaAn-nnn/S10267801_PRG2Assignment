//==========================================================
// Student Number  : S10267180D
// Student Name  : De Roza Ariel Therese
// Partner Name  : Peh Ya An
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGASSIGNMENT_S10267180D_ARIELDEROZA
{
    internal class Terminal
    {
        private string terminalName;
        private Dictionary<string, Airline> airlines;
        private Dictionary<string, Flight> flights;
        private Dictionary<string, BoardingGate> boardingGates;
        private Dictionary<string, double> gateFees;

        public string TerminalName { get; set;}
        public Dictionary<string, Airline> Airlines { get; set;}
        public Dictionary<string, Flight> Flights { get; set;}
        public Dictionary<string, BoardingGate> BoardingGates { get; set;}
        public Dictionary<string, double> GateFees { get; set;}

        public Terminal(string term, Dictionary<string,Airline>air, Dictionary<string,Flight>fly, Dictionary<string,BoardingGate> board, Dictionary<string, double> gates)
        {
            TerminalName = term;
            Airlines = air;
            Flights = fly;
            BoardingGates = board;
            GateFees = gates;
        }

        // Adds a new airline to the terminal
        public bool AddAirline(Airline airline)
        {
            if (airlines.ContainsKey(airline.Code))
                return false;

            airlines[airline.Code] = airline;
            return true;
        }

        // Adds a new boarding gate to the terminal
        public bool AddBoardingGate(BoardingGate gate)
        {
            if (boardingGates.ContainsKey(gate.GateName))
                return false;

            boardingGates[gate.GateName] = gate;
            return true;
        }

        // Retrieves the airline associated with a specific flight
        public Airline GetAirlineFromFlight(Flight flight)
        {
            foreach (var airline in airlines.Values)
            {
                if (airline.Flights.ContainsKey(flight.FlightNumber))
                    return airline;
            }
            return null;
        }

        // Prints the fees associated with each airline
        public void PrintAirlineFees()
        {
            foreach (var airline in airlines.Values)
            {
                double totalFees = airline.CalculateFlight();
                Console.WriteLine($"Airline: {airline.Name}, Total Fees: {totalFees:C}");
            }
        }

        // Overrides the ToString method for terminal details
        public override string ToString()
        {
            return $"Terminal Name: {TerminalName}, Airlines: {airlines.Count}, Flights: {flights.Count}, " +
                   $"Boarding Gates: {boardingGates.Count}";
        }
    }
}

