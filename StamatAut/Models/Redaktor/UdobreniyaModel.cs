using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StamatAut.Models.Redaktor
{
    public class UdobreniyaModel
    {
        /*public IReadOnlyCollection<CultLib> Cltrs { get; private set; }*/
        public string FarmName { get; set; }

        public UdobreniyaModel(string FarmName)
        {
            this.FarmName = FarmName;
            //Cltrs = ForEditor.GetCulturesList(FarmName);

        }


    }
}