//==========================================================
// Student Number  : S10267180D
// Student Name  : De Roza Ariel Therese
// Partner Name  : Peh Ya An
//==========================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10267801_PRG2Assignment
{
    internal class Airline : Flight
    {
        private string name;
        private string code;
        private Dictionary<string, Flight> flights;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public Dictionary<string, Flight> Flights
        {
            get { return flights; }
            set { flights = value; }
        }


        public Airline(string name, string code, Dictionary<string, Flight> flights) : base()
        {
            Name = name;
            Code = code;
            Flights = flights;
        }

        public bool AddFlight(Flight flight)
        {
            if (flight == null || flights.ContainsKey(FlightNumber))
                return false;

            flights[FlightNumber] = flight;
            return true;
        }

        public bool RemoveFlight(Flight flight)
        {
            return flights.Remove(FlightNumber);
        }

        public double CalculateFlight()
        {
            double totalFees = 0;
            foreach (var flight in flights.Values)
            {
                totalFees += flight.CalculateFees();
            }
            return totalFees;
        }

    public override string ToString()
        {
            return $"{FlightNumber}          {Name}       {Origin}               {Destination}          {ExpectedTime:dd/MM/yyyy hh:mm:ss tt}";
        }
    }

}
