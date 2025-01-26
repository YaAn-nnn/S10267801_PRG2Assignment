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
    internal class BoardingGate
    {
        private string gateName;
        private bool supportsCFFT;
        private bool supportsDDJB;
        private bool supportsLWTT;
        private Flight flight;
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }


        public BoardingGate(string Name, bool CFFT, bool DDJB, bool LWTT, Flight flight)
        {
            this.gateName = Name;
            this.supportsCFFT = CFFT;
            this.supportsDDJB = DDJB;
            this.supportsLWTT = LWTT;
            this.flight = flight;
        }

        public double CalculateFees()
        {
            if (flight == null) return 0;

            // Check if the gate supports the flight type
            if ((flight is CFFTFlight && !CFFT) ||
                (flight is DDJBFlight && !DDJB) ||
                (flight is LWTTFlight && !LWTT))
            {
                throw new InvalidOperationException("This gate does not support the flight type.");
            }

            // Return the fees for the flight
            return flight.CalculateFees();
        }

        public override string ToString()
        {
            return $"Gate Name: {Name}, Supports CFFT: {CFFT}, Supports DDJB: {DDJB}, Supports LWTT: {LWTT}, " + $"Assigned Flight: {(flight != null ? flight.FlightNumber : "None")}";
        }
    }
}

