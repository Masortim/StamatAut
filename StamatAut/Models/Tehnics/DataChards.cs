using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StamatAut.Models.Tehnics
{
    public class DataChards
    {
        public string DataName { get; set; }
        public double Count { get; set; }
        public int Count2 { get; set; }
        public Intervals[] Interval { get; set; }
        public DateTime[,] Intervals { get; set; }
        public DateTime[,] Intervals2 { get; set; }
        public DateTime to { get; set; }
        public DateTime from { get; set; }

    }

    public class Intervals
    {

        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}