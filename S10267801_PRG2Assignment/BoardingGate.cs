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
        private string gateName;
        private bool supportsCFFT;
        private bool supportsDDJB;
        private bool supportsLWTT;
        private Flight flight;
        public string GateName { get { return gateName; } set { gateName = value; } }
        public bool SupportsCFFT { get { return supportsCFFT; } set { supportsCFFT = value; } }
        public bool SupportsDDJB { get { return supportsDDJB; } set { supportsDDJB = value; } }
        public bool SupportsLWTT { get { return supportsLWTT; } set { supportsLWTT = value; } }
        public Flight Flight { get { return flight; } set { flight = value; } }


        public BoardingGate(string Name, bool CFFT, bool DDJB, bool LWTT) : base()
        {
            GateName = Name;
            SupportsCFFT = CFFT;
            SupportsDDJB = DDJB;
            SupportsLWTT = LWTT;
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
            return $"{GateName,-15} {SupportsCFFT,-22} {SupportsDDJB,-22} {SupportsLWTT}";
        }
    }
}

