using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary1;

namespace StamatAut.Models.Technologies
{
    public class SelectModels
    {
        public TehKR Tkr { get; set; }
        public List<Pests> PestsVat { get; set; }//вредители
        public List<Diseas> DiseaseVal { get; set; }//болезни
        public List<Pests> WeedVal { get; set; }//сорняки
        public List<Fert> FertilizersVal { get; set; }//удобрения

        public List<Pests> Pest { get; set; }//вредители
        public List<Diseas> Disease { get; set; }//болезни
        public List<Pests> Weed { get; set; }//сорняки
        public List<Fert> Fertilizers { get; set; }//удобрения
        public WArea WorkAreas { get; set; }

        public SelectModels(WArea WorkArea, TehKR tkr, int idFarm)
        {
            WorkAreas = WorkArea;
            Tkr = tkr;
            PestsVat = DBWork.GetPest().Where(y => y.Value == 0).ToList();
            DiseaseVal = DBWork.GetDisease().Where(y => y.Value == 0).ToList();
            FertilizersVal = DBWork.GetFert(idFarm);
            if (tkr.TK_dop.Where(y => y.Id_Wa == WorkArea.Id).Count() != 0)
            {
                Pest = tkr.TK_dop.Where(y => y.Id_Wa == WorkArea.Id).First().Pes;
            }
            if (tkr.TK_dop.Where(y => y.Id_Wa == WorkArea.Id).Count() != 0)
            {
                Disease = tkr.TK_dop.Where(y => y.Id_Wa == WorkArea.Id).First().Dis;
            }

        }


    }
}