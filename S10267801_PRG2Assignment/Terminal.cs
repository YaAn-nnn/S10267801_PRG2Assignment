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
        public string TerminalName { get { return terminalName; } set { terminalName = value; } }
        public Dictionary<string, Airline> Airlines {get; set;}
        private Dictionary<string, Flight> Flights {get; set;}
        public Dictionary<string, BoardingGate> BoardingGates { get; set;}  
        public Dictionary<string, double> GateFees { get; set;}
        public Terminal(string term, Dictionary<string, Airline> air, Dictionary<string, Flight> fly, Dictionary<string, BoardingGate> board, Dictionary<string, double> gates)
        {
            term = TerminalName;
            air = Airlines;
            fly = Flights;
            board = BoardingGates;
            gates = GateFees;
        }
        public bool AddAirline(Airline)
        {

        }
        public bool AddBoaringGate(BoardingGate)
        {

        }
        public Airline GetAirlineFromFlight(Flight)
        {

        }
        public void PrintAirlineFees()
        {

        }
        public string ToString()
        {
            return "{0,-150} {1,-150} {2,-150} {3,-150} {4,-150} {5,-150}";
        }
    }
}
