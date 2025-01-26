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
using System.Xml.Linq;

namespace S10267801_PRG2Assignment
{
    internal class BoardingGate : Flight
    {
        public string gateName;
        public bool supportsCFFT;
        public bool supportsDDJB;
        public bool supportsLWTT;
        public Flight flight;
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }


        public BoardingGate(string Name, bool CFFT, bool DDJB, bool LWTT) : base()
        {
            Name = GateName;
            CFFT = SupportsCFFT;
            DDJB = SupportsDDJB;
            LWTT = SupportsLWTT;
        }

        public override double CalculateFees()
        {
            if (flight == null) return 0;

            // Check if the gate supports the flight type
            if ((flight is CFFTFlight && !SupportsCFFT) ||
                (flight is DDJBFlight && !SupportsDDJB) ||
                (flight is LWTTFlight && !SupportsLWTT))
            {
                throw new InvalidOperationException("This gate does not support the flight type.");
            }

            // Return the fees for the flight
            return flight.CalculateFees();
        }

        public override string ToString()
        {
            return $"Gate Name: {GateName}, Supports CFFT: {SupportsCFFT}, Supports DDJB: {SupportsDDJB}, Supports LWTT: {SupportsLWTT}, " + $"Assigned Flight: {(flight != null ? Flight.FlightNumber : "None")}";
        }
    }
}

