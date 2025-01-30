//==========================================================
// Student Number  : S10267801
// Student Name  : Peh Ya An
// Partner Name  : De Roza Ariel Therese
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace S10267801_PRG2Assignment
{
    internal class Flight
    {
        private string flightNumber;
        private string origin;
        private string destination;
        private DateTime expectedTime;
        private string status;
        private string specialRequestCode;

        public string FlightNumber { get { return flightNumber; } set { flightNumber = value; } }
        public string Origin { get { return origin; } set { origin = value; } }
        public string Destination { get { return destination; } set { destination = value; } }
        public DateTime ExpectedTime { get { return expectedTime; } set { expectedTime = value; } }
        public string Status { get { return status; } set { status = value; } }
        public string SpecialRequestCode { get { return specialRequestCode; } set { specialRequestCode = value; } }
        public string AirlineName;

        public Flight() { }
        public Flight(string f, string a, string o, string d, DateTime e, string s, string sqc)
        {
            FlightNumber = f;
            AirlineName = a;
            Origin = o;
            Destination = d;
            ExpectedTime = e;
            Status = s;
            SpecialRequestCode = sqc;
        }

        public virtual double CalculateFees()
        {
            double fee = 0;

            // Base boarding gate fee
            fee += 300;

            // Terminal fees
            if (Destination == "SIN") fee += 500; // Arriving flight
            if (Origin == "SIN") fee += 800; // Departing flight

            return fee;
        }
        public override string ToString()
        {
            return $"{FlightNumber,-16}{AirlineName,-25}{Origin,-26}{Destination,-25}{ExpectedTime:dd/MM/yyyy hh:mm:ss tt}";
        }
    }

    class NORMFlight : Flight
    {
        public NORMFlight() { }
        public NORMFlight(string f, string a, string o, string d, DateTime e, string s, string sqc) : base(f, a, o, d, e, s, sqc) { }

        public override double CalculateFees()
        {
            return base.CalculateFees();
        }
    }

    class LWTTFlight : Flight
    {
        private double requestFee;

        public double RequestFee { get { return requestFee; } set { requestFee = value; } }
        public LWTTFlight() { }
        public LWTTFlight(string f, string a, string o, string d, DateTime e, string s, double r, string sqc) : base(f, a, o, d, e, s, sqc) { RequestFee = r; }


        public override double CalculateFees()
        {
            return base.CalculateFees() + 500;
        }
    }

    class DDJBFlight : Flight
    {
        private double requestFee;

        public double RequestFee { get { return requestFee; } set { requestFee = value; } }
        public DDJBFlight() { }
        public DDJBFlight(string f, string a, string o, string d, DateTime e, string s, double r, string sqc) : base(f, a, o, d, e, s, sqc) { RequestFee = r; }

        public override double CalculateFees()
        {
            return base.CalculateFees() + 300;
        }
    }
    class CFFTFlight : Flight
    {
        private double requestFee;

        public double RequestFee { get { return requestFee; } set { requestFee = value; } }
        public CFFTFlight() { }
        public CFFTFlight(string f, string a, string o, string d, DateTime e, string s, double r, string sqc) : base(f, a, o, d, e, s, sqc) { RequestFee = r; }


        public override double CalculateFees()
        {
            return base.CalculateFees() + 150;
        }
    }
}
