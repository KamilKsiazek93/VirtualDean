using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VirtualDean.Enties
{
    public class Hours
    {
        private const string T8 = "8.00";
        private const string T9 = "9.00";
        private const string T10 = "10.30";
        private const string T12 = "12.00";
        private const string T13 = "13.30";
        private const string T15 = "15.30";
        private const string T17 = "17.00";
        private const string T19 = "19.00";
        private const string T20 = "20.20";
        private const string T21 = "21.30";

        private const string COMMUNION_HOUR_TO_EXCLUDE = "10.30";

        public IEnumerable<string> TrayHours { get { return new List<string>() { T8, T9, T10, T12, T13, T15, 
            T17, T19, T20, T21 }; 
            } }

        public IEnumerable<string> CommunionHours { get {
                return TrayHours.Where(item => item != COMMUNION_HOUR_TO_EXCLUDE);
            } }
    }
}
