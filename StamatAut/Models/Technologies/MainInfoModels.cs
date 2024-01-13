using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary1;

namespace StamatAut.Models.Technologies
{
    public class MainInfoModels
    {
        public List<Culture> cultureVal { get; set; }
        public int countCult { get; set; }
        public int idFarm { get; set; }

        public MainInfoModels(int IdFarm, string str)
        {
            idFarm = IdFarm;
            cultureVal = DBWork.GetCulture();
            countCult = cultureVal.Count();
        }
        
    }
}