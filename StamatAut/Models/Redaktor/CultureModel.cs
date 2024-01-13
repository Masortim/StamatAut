using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StamatAut.Models.Redaktor
{
    public class CultureModel
    {
        /*public IReadOnlyCollection<CultLib> Cltrs { get; private set; }*/
        public string FarmName { get; set; }

        public CultureModel(string FarmName)
        {
            this.FarmName = FarmName;
            //Cltrs = ForEditor.GetCulturesList(FarmName);

        }

        public CultureModel()
        {
            this.FarmName = "FarmName";
            //Cltrs = ForEditor.GetCulturesList(FarmName);

        }


    }
}