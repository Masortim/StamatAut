using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary1;

namespace StamatAut.Models.Technologies
{
    public class TechnologiesFarmModels
    {
        public int idFarm { get; set; }
        public List<int> years { get; set; } // список "год урожая" из существующих ТКР + текущий год
        public int year { get; set; } // выбранный год
        public List<TehKR> tkrs { get; set; } // список ТКР из БД с рабочими участками
        public bool nullTkrs { get; set; } // флаг, говорящий что по данному году ТКР не существует (false) или есть (true)
        public List<WArea> wAreas { get; set; }//список всех рабочих участков с отметкой о распределении по ТКР (YorN)
        public List<Culture> cultureVal { get; set; } // Список культур для заполнения поля Культуры в ТКР
        public List<string> intensificalionVal { get; set; } // Список уровней интенсификации для заполнения поля Культуры в ТКР
        public List<string> predshVal { get; set; } // Список уровней интенсификации для заполнения поля Культуры в ТКР
        public int countWorkArea { get; set; }        
        public List<string> climat { get; set; }
        public List<string> cultureName { get; set; }
        public List<double> squareOfCulture { get; set; }
        //idEditTkr - id ТКР которую необходимо редактировать: 
        //-1 - нет ни 1 ТКР на выбраный год; 
        //0 - ни одна из ТКР не редактиреутся в данный момент; 
        //>0 - одна из ТКР в режиме редактирования
        public int idEditTkr { get; set; }
        public string ListTkrs { get; set; }
        public TechnologiesFarmModels(int IdFarm ,int Year, int IdEditTkr)
        {
            idFarm = IdFarm;
            idEditTkr = IdEditTkr;
            years = DBWork.GetYearsAll(idFarm);
            year = Year == 0 ? years.First() : Year;            
            tkrs = DBWork.GetTKRonYear(idFarm, year);
            ListTkrs = "";
            if (tkrs.Count()>0)
            {
                foreach (var t in tkrs)
                    ListTkrs += t.Id.ToString()+",";
            }
            
            nullTkrs = tkrs.Count > 0 ? true : false;
            idEditTkr = !nullTkrs ? -1 : 0;
            /*if (!nullTkrs)
                idEditTkr = -1;
            else idEditTkr = 0;*/
            wAreas = DBWork.GetWorkAreas(idFarm, year);
            idEditTkr = IdEditTkr>0 ? IdEditTkr : idEditTkr;//если одна из ТКР в режиме редактирования
            cultureVal = DBWork.GetCulture();
            intensificalionVal = DBWork.GetIntensification();
            predshVal = DBWork.GetPredshest();
           
            countWorkArea = wAreas.Count();

            climat = DBWork.GetAllNameClimate();
            var cult = DBWork.GetCulture();
            cultureName = new List<string>();
            squareOfCulture = new List<double>();
            foreach(var c in cult)
            {
                var tk = tkrs.Where(x => x.Culture == c.Name);
                if (tk.Count()>0)
                {
                    cultureName.Add(c.Name);
                    double i = 0;
                    foreach(var t in tk)
                    {
                        foreach(var sq in t.WA)
                        {
                            i += sq.Square;
                        }                        
                    }
                    squareOfCulture.Add(i);
                }

            }
            

        }
        
        
    }
}