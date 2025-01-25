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
        public BoardingGate(string gate, bool CFFT, bool DDJB, bool LWTT,Flight fly):base()
        {
            gate = GateName;
            CFFT = SupportsCFFT;
            DDJB = SupportsDDJB;
            LWTT = SupportsLWTT;
            fly = Flight;
        public double CalculateFees()
        {

        }
        public override string ToString()
        {
        }
    }
}
