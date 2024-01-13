using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary1;

namespace StamatAut.Models.Technologies
{
    public class TechnologiesVariationResModel
    {
        public List<TechAgroVarToModel> VarTkrs { get; set; }       
        public int IdFarm { get; set; }
        public List<string> NameTKR { get; set; }
        public Dictionary<string, List<TechAgroVar>> GroupVariants { get; set; }
        public List<TechnologicalOperations> ListTO { get; set; }

        public TechnologiesVariationResModel(int idFarm, List<TechAgroVar> variantTeh)
        {
            IdFarm =idFarm;
            ListTO = DBWork.GetTO();
            VarTkrs = new List<TechAgroVarToModel>();
            foreach (var v in variantTeh)
            {
                var name = DBWork.GetTKRNameOnId(v.IdTKR);
                VarTkrs.Add(new TechAgroVarToModel(name,v));
            }
            GroupVariants = new Dictionary<string, List<TechAgroVar>>();
            for (int i = 0; i < VarTkrs.Count; i++)
            {
                string key = VarTkrs.ElementAt(i).NameTKR;
                TechAgroVar value = VarTkrs.ElementAt(i).VTkrs;
                if (GroupVariants.ContainsKey(key))
                    GroupVariants[key].Add(value);
                else
                    GroupVariants.Add(key, new List<TechAgroVar> { value });
            }            
        }

       /* public string GetNameTKR(string Id)
        {
            int id = Convert.ToInt32(Id);
            Name=VarTkrs.First(c=>c.IdTKR==id).NameVar.Split('-')
        }
        */
    }

    public class TechAgroVarToModel
    {
        public TechAgroVar VTkrs { get; set; }
        public string NameTKR { get; set; }
        
        public TechAgroVarToModel(string name, TechAgroVar varTkrs)
        {
            NameTKR = name;
            VTkrs = varTkrs;
        }
    }
}