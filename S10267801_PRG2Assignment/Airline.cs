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
    internal class Airline
    {
        private string name;
        private string code;
        private Dictionary<string, Flight> flights;

        public string Name { get; set; }
        public string Code { get { return code; } set { code = value; } }

        public Airline(string name, string code, Dictionary<string, Flight> flights):base()
        {
            this.Name = name;
            this.Code = code;
        }

        public bool AddFlight(Flight)
        {
            if (flight == null || flights.ContainsKey(Flight.flightNumber))
                return false;

            flights[Flight.flightNumber] = flight;
            return true;
        }

        public bool RemoveFlight(Flight)
        {
            return flights.Remove(Flight.flightNumber);
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
            return $"Airline Name: {Name}, Code: {Code}, Total Flights: {flights.Count}";
        }
    }

}
