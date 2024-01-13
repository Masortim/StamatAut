using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary1;

namespace StamatAut.Models.Technologies
{
    public class ZonaModels
    {
        public Climat Zone { get; set; }
        public string DateBeg { get; set; }
        public string DateEnd { get; set; }
        public string KolOsS { get; set; }
        public string SumT { get; set; }
        public string KoefUvl { get; set; }
        public string SummT_10_12 { get; set; }
        public string SummT_5_10 { get; set; }

        public ZonaModels(string nameZone)
        {            
            Zone = DBWork.GetClimateOnName(nameZone);            
        }


    }
}