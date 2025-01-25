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
        public string Name { get; set; }
        public string Code { get { return code; } set { code = value; } }
        public Airline(string na, string co, Dictionary<string, Flight> fly) : base()
        {
            na = Name;
            co = Code;
        }
        public bool AddFlight(Flight)
        {

        }
        public double CalculateFlight()
        {

        }
        public bool RemoveFlight(Flight)
        {

        }
        public override string ToString()
        {
            return "{0,-150} {1,-150} {2,-150} {3,-150} {4,-150} {5,-150}";
        }
    }
}
