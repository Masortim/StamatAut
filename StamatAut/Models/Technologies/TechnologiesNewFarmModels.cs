using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary1;

namespace StamatAut.Models.Technologies
{
    public class TechnologiesNewFarmModels
    {
        public List<WArea> wAreas { get; set; }
        public int countWorkArea { get; set; }
        public int idFarm { get; set; }
        public List<string> ClimateVal {get;set;}
        public TechnologiesNewFarmModels(int IdFarm)
        {
            idFarm = IdFarm;
            countWorkArea = 0;
            ClimateVal = DBWork.GetAllNameClimate();
        }
        public TechnologiesNewFarmModels(int IdFarm, int colWAreas)
        {
            idFarm = IdFarm;
            countWorkArea = colWAreas;   
            wAreas = DBWork.GetWorkAreas(idFarm);
        }
    }

    public class NewWorkAreaModels
    {
        public bool status { get; set; }
        public int IdFarm { get; set; }
        public int IdWorkArea { get; set; }
        public string NumWorkArea { get; set; }
        public double Square { get; set; }
        public double MaxZahvat { get; set; }
        public double LongWorkArea { get; set; }
        public double WidthWorkArea { get; set; }
        public double Podvoz { get; set; }
        public double PersKolk { get; set; }
        public double UgolNakl { get; set; }
        public string Pochvi { get; set; }
        public List<Soile> PochviVal { get; set; }


        public NewWorkAreaModels(int idFarm, int num, string numWorkArea, double longWorkArea, double widthWorkArea, double persKolk, double ugolNakl, double podvoz , string pochvi, double square)
        {
            status = true;
            IdFarm = idFarm;
            IdWorkArea = num;
            NumWorkArea = numWorkArea;
            LongWorkArea = longWorkArea;
            PersKolk = persKolk;
            WidthWorkArea = widthWorkArea;
            Square = square;
            UgolNakl = ugolNakl;
            MaxZahvat = 0;
            Podvoz = podvoz;
            Pochvi = pochvi;
            PochviVal = DBWork.GetSoil(idFarm);
        }

        public NewWorkAreaModels()
        {
            status = false;           
        }
    }

    
   
}