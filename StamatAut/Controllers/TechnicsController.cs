using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibrary1;
using StamatAut.Models.Tehnics;

namespace StamatAut.Controllers
{
    public class TechnicsController : Controller
    {
        public int idFarm;        

        [HttpGet]
        public ActionResult IndexTechnics(string tkr, string metod, string CurrentLink)
        {
            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера
            if (metod == "Calculate")
            {                
                TechnicsModel TechModel = new TechnicsModel(idFarm, DateTime.Now.Year,tkr, CurrentLink);
                return View("IndexTechnicsCalculate", TechModel);
            }
            List<TehKart> TKart = DBWork.GetTechKatr(idFarm, DateTime.Now.Year);//запрос технологических операций по idFarm юзера и году урожая
            if (TKart.Count() == 0)
            {                
                return View("IndexTechnics");                
            }
            else
            {
                TechnicsModel TechModel = new TechnicsModel(idFarm, DateTime.Now.Year,"", CurrentLink);
                return View("IndexTechnics", TechModel);
            }
            return View();

        }
    }
}