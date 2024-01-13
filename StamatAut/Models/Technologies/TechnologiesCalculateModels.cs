using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary1;

namespace StamatAut.Models.Technologies
{
    public class TechnologiesCalculateModels
    {
        public List<TechResult> tkrs { get; set; }
        public int countCult { get; set; }
        public int idFarm { get; set; }
        public List<string> Variants { get; set; }

        public TechnologiesCalculateModels()
        {
            
        }

        public TechnologiesCalculateModels(int IdFarm, List<TechResult> Tkrs)
        {
            idFarm = IdFarm;
            tkrs = Tkrs;
            var v = DBWork.GetTechAgro(IdFarm);
            Variants = new List<string>();
            if(v.Count()!=0)
            {
                foreach (var vv in v)
                    Variants.Add(vv.NameVar);
                Variants = Variants.Distinct().ToList();
            }
        }
        
    }
}