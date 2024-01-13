using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StamatAut.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult SelectTechnologies()
        {
            return View();
        }

        public ActionResult SelectTechnics()
        {
            return View();
        }

        public ActionResult Account()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Save(string FarmName, string IdUser, string NameUser, string Password, string AccessLevel, string Action, string EMail)//
        {
            if (ModelState.IsValid)
            {
                switch (Action)
                {
                    case "OkCultureAdd":
                        {
                            /*string id = "0";
                            ViewBag.FarmName = FarmName;
                            ViewBag.Action = "Culture";
                            ViewBag.Controller = "Redaktor";
                            var result = ForEditor.SaveCulture(id, NameCult, Norm, Mass, Col, FarmName);
                            if (result)
                                ViewBag.Message = "Данные успешно добавлены.";
                            else
                                ViewBag.Message = "Ошибка добавления данных.";
                            return View("_Message_");*/
                            return View();
                        }
                        break;
                    default:
                        {
                            return View();
                        }
                        break;
                }

            }
            else
            {
                return View();
            }
        }
    }
}