using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ClassLibrary1;
using StamatAut.Models.Technologies;

namespace StamatAut.Controllers
{
    [Authorize()]
    public class TechnologiesController : Controller
    {
        // GET: Technologies

        public int idFarm;
        public ActionResult IndexTechnologies()
        {
            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера

            List<WArea> wAreas = DBWork.GetWorkAreas(idFarm, DateTime.Now.Year);//запрос количества рабочих участков по idFarm юзера
            if (wAreas.Count() == 0)
            {
                TechnologiesNewFarmModels newFarmModel = new TechnologiesNewFarmModels(idFarm);
                return View("IndexTechnologiesNewFarm", newFarmModel);
            }
            else
            {
                //добавить проверку есть ли среди рабочих участков незаполненные например по площади участка если есть то вызвать добавление нового хозяйства, иначе открытие новой формы
                TechnologiesNewFarmModels newFarmModel = new TechnologiesNewFarmModels(idFarm, wAreas.Count());
                if (wAreas.Exists(x => x.Square == 0.0))
                    return View("IndexTechnologiesNewFarm", newFarmModel);
                else
                {
                    TechnologiesFarmModels model = new TechnologiesFarmModels(idFarm, 0, -1);
                    return View("IndexTechnologiesFarm", model);
                }

            }

        }

        [HttpGet]
        public ViewResult _newWArea(int IdFarm, string IdWorkArea)
        {
            idFarm = IdFarm;
            var wArea = DBWork.GetWorkAreas(idFarm).Find(x => x.Id == Convert.ToInt32(IdWorkArea));
            NewWorkAreaModels model;
            model = new NewWorkAreaModels(idFarm, wArea.Id, wArea.Name, wArea.Lenght, wArea.Wight, wArea.KolokPercent, wArea.Angle_gr, wArea.Distanse, wArea.SoilType, wArea.Square);
            return View("_newWArea", model);//
        }

        [HttpGet]
        public ViewResult select(string id, string year)
        {
            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера
            SelectModels model;
            var WA = DBWork.GetWorkAreas(idFarm, Convert.ToInt32(year)).Where(y => y.Id == Convert.ToInt32(id)).First();
            var tkrs = DBWork.GetTKRonYear(idFarm, Convert.ToInt32(year));
            TehKR tkr = null;
            foreach (var t in tkrs)
            {
                foreach (var wa in t.WA)
                {
                    if (wa.Id == WA.Id)
                        tkr = t;
                }
            }

            model = new SelectModels(WA, tkr, idFarm);
            return View("_select", model);//

        }

        

        [HttpGet]
        public ViewResult changeZona(string zona)
        {
            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера
            ZonaModels model;
            model = new ZonaModels(zona);
            return View("_zona", model);
        }

        [HttpGet]
        public ViewResult addWAreas()
        {
            TechnologiesFarmModels model;
            model = new TechnologiesFarmModels(DBWork.GetEntrance(HttpContext.User.Identity.Name), 0, 0);
            return View("_addWAreas", model);//

        }

        [HttpGet]
        public string allSguare(string str, string idFarm)
        {
            string[] s = str.Split(',');
            double squareAll = 0;
            var wAreas = DBWork.GetWorkAreas(Convert.ToInt32(idFarm));
            for (int i = 1; i < s.Length; i++)
            {
                squareAll += wAreas.Find(x => x.Id == Convert.ToInt32(s[i])).Square;
            }
            return squareAll.ToString();
        }
        [HttpGet]
        public ActionResult ChangeYear(string year)
        {
            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера
            TechnologiesFarmModels FarmModel = new TechnologiesFarmModels(idFarm, Convert.ToInt32(year), 0);            
            return View("IndexTechnologiesFarm", FarmModel);
        }

        [HttpPost]
        public ActionResult ChangeYear2(string year)
        {
            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера
            TechnologiesFarmModels FarmModel = new TechnologiesFarmModels(idFarm, Convert.ToInt32(year), 0);
            return View("IndexTechnologiesFarm", FarmModel);
        }
        
        [HttpPost]
        public ActionResult Index(string ColWorkArea, string Action, string numWorkArea, string longWorkArea, string widthWorkArea, string ugolNakl, string podvoz,
            string pochvi, string kolokPercent, string square, string maxZahvat, string predsh, string zona,
            string idListWorkArea, string AllwAreas, string intens, string culture, string years, string[] selectWAraes, string idTkr,
            List<string> selectPests, List<string> selectDiseases, string idWA, string tkrList, List<string> selectTechPaket, List<string> nameTKR ,string NameVariant, 
            List<string> dateB, List<string> dateE,List<string> persentTO/*, TechnologiesCalculateModels model*/)
        {
            var act = Action.Split(' ');
            if (act[0] == "editTKR" || act[0] == "delTKR")
            {
                Action = act[0];
                idTkr = act[1];
            }
            TechnologiesNewFarmModels newFarmModel = new TechnologiesNewFarmModels(idFarm);
            if (ModelState.IsValid)
            {
                switch (Action)
                {
                    #region TKR
                    case "addWAreasToTkr": //Создание новой или редактирование ТКР с довавлением в нее рабочих участков
                        {
                            
                                idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера
                                List<int> wa = new List<int>();
                                if (selectWAraes != null)
                                    foreach (var v in selectWAraes)
                                    {
                                        wa.Add(Convert.ToInt32(v));
                                    }
                                else
                                    wa = null;
                                Action = "Next";
                                var x = DBWork.SetTKR(idFarm, Convert.ToInt32(idTkr), culture, intens, predsh, Convert.ToInt32(years), wa);
                                TechnologiesFarmModels FarmModel = new TechnologiesFarmModels(idFarm, Convert.ToInt32(years), 0);
                                return View("IndexTechnologiesFarm", FarmModel);
                            
                        }
                        break;
                    case "delTKR":
                        {
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера
                            var x = DBWork.DelTKR(idFarm, Convert.ToInt32(idTkr));
                            TechnologiesFarmModels FarmModel = new TechnologiesFarmModels(idFarm, Convert.ToInt32(years), 0);
                            return View("IndexTechnologiesFarm", FarmModel);

                        }
                        break;
                    case "editTKR":
                        {
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера                            
                            TechnologiesFarmModels FarmModel = new TechnologiesFarmModels(idFarm, Convert.ToInt32(years), Convert.ToInt32(idTkr));
                            return View("IndexTechnologiesFarm", FarmModel);

                        }
                        break;
                    case "addTKR":// Добавление новой ТКР
                        {
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера                            
                            TechnologiesFarmModels FarmModel = new TechnologiesFarmModels(idFarm, Convert.ToInt32(years), 10000);
                            return View("IndexTechnologiesFarm", FarmModel);
                        }
                        break;
                    case "closeEditTkr":
                        {
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера                            
                            TechnologiesFarmModels FarmModel = new TechnologiesFarmModels(idFarm, Convert.ToInt32(years), 0);
                            return View("IndexTechnologiesFarm", FarmModel);

                        }
                        break;
                    #endregion

                    case "Ok"://Добавление новому хозяйчтву пустых рабочих участков и переход в форму редактирования информации по отдельному рабочему участку 
                        {
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера
                            ViewBag.idFarm = idFarm;
                            DBWork.SaveClimateToFarm(idFarm, zona);
                            for (int i = 0; i < Convert.ToInt32(ColWorkArea); i++)
                            {
                                DBWork.SetEmptyWorkAreas(idFarm, i);
                            }
                            newFarmModel = new TechnologiesNewFarmModels(idFarm, Convert.ToInt32(ColWorkArea));
                            return View("IndexTechnologiesNewFarm", newFarmModel);
                        }
                        break;
                    case "OkWorkAreaAdd"://Редактирование информации по отдельному рабочему участку 
                        {
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера
                            maxZahvat = "0";
                            var add = DBWork.SetWorkAreas(idFarm, Convert.ToInt32(idListWorkArea), numWorkArea, float.Parse(square), float.Parse(maxZahvat), Convert.ToDouble(longWorkArea), Convert.ToDouble(widthWorkArea), Convert.ToDouble(ugolNakl), Convert.ToDouble(podvoz), Convert.ToDouble(kolokPercent), pochvi);
                            int col = DBWork.GetWorkAreas(idFarm, Convert.ToInt32(years)).Count();
                            newFarmModel = new TechnologiesNewFarmModels(idFarm, Convert.ToInt32(col));
                            return View("IndexTechnologiesNewFarm", newFarmModel);
                        }
                        break;
                    case "Delite":
                        {
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера
                            var del = DBWork.DelWorkAreas(idFarm, Convert.ToInt32(idListWorkArea));
                            int col = DBWork.GetWorkAreas(idFarm).Count();
                            newFarmModel = new TechnologiesNewFarmModels(idFarm, Convert.ToInt32(col));
                            return View("IndexTechnologiesNewFarm", newFarmModel);
                        }
                        break;
                    case "Next":
                        {
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера
                            TechnologiesFarmModels FarmModel = new TechnologiesFarmModels(idFarm, 0, 0);
                            return View("IndexTechnologiesFarm", FarmModel);
                        }
                    case "Calculate":
                        {
                            var t = tkrList.Split(',');
                            List<int> listId = new List<int>();
                            for (int i=0;i<t.Count()-1;i++)
                            {
                                listId.Add(Convert.ToInt32(t[i]));
                            }
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);
                            var techPacket = DBWork.GetTechPacket(listId);//получение idFarm по имени юзера
                            TechnologiesCalculateModels FarmModel = new TechnologiesCalculateModels(idFarm, techPacket);
                            return View("IndexTechnologiesCalculations", FarmModel);
                        }
                        break;
                    case "Save":
                        {
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);
                            var ListVariant = new List<string[]>();
                            //срока selectTechPaket имеет вид: IdTKR-nameWaGroup-IdTechPacket по всем выбранным вариантам
                            foreach (var v in selectTechPaket)
                            {
                                string nameVar = NameVariant;//v + '-' + DateTime.Now.Date.ToShortDateString();
                                var str = v.Split('-');
                                ListVariant.Add(str);
                                //запись вариантов во вспомогательную таблицу
                                var setTA = DBWork.SetTechAgro(-1, idFarm, nameVar, Convert.ToInt32(str[0]), str[1], Convert.ToInt32(str[2]));

                            }
                            var VariantTeh = DBWork.GetTechAgro(idFarm,ListVariant);//получение вариантов из временной таблицы в БД                            
                            TechnologiesVariationResModel FarmModel = new TechnologiesVariationResModel(idFarm, VariantTeh);
                            return View("IndexTechnologiesVariationRes", FarmModel);
                        }
                        break;
                    case "SaveTehKart":
                        {
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);
                            //сохраняем данные по датам в таблицу TehKart                            
                            var v = DBWork.SetTechKart(-1, selectTechPaket,dateB,dateE);
                            string metod = "Calculate";
                            List<int> id_tkr = new List<int>();
                            string tkr = "";
                            foreach (var s in selectTechPaket)
                            {
                                var id =Convert.ToInt32( s.Split('-')[0]);
                                if (!id_tkr.Contains(id))
                                {
                                    id_tkr.Add(id);
                                    tkr += id.ToString()+",";
                                }
                            }   
                            
                            //получение вариантов из временной таблицы в БД                            
                            //TechnologiesVariationResModel FarmModel = new TechnologiesVariationResModel(idFarm, VariantTeh);
                            return RedirectToAction("IndexTechnics", "Technics",new { tkr , metod });
                        }
                        break;
                    case "okCultAdd":
                        {
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера
                            string[] s = AllwAreas.Split(',');
                            for (int i = 1; i < s.Length; i++)
                            {
                                //var add= DBWork.SetCultWAreas(idFarm, s[i]);//передать в функцию idFarm и idWArea
                            }
                            //затем обновить модель с указанием выбранных участков .....пока отрисовываем по прежнему
                            int col = DBWork.GetWorkAreas(idFarm).Count();
                            TechnologiesFarmModels FarmModel = new TechnologiesFarmModels(idFarm, Convert.ToInt32(years), 0);
                            return View("IndexTechnologiesFarm", FarmModel);
                        }
                        break;
                    case "OkIntensAdd":
                        {
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера
                            string[] s = AllwAreas.Split(',');
                            for (int i = 1; i < s.Length; i++)
                            {
                                //var add= DBWork.SetIntensAreas(idFarm, s[i]);//передать в функцию idFarm и idWArea
                            }
                            //затем обновить модель с указанием выбранных участков .....пока отрисовываем по прежнему
                            int col = DBWork.GetWorkAreas(idFarm).Count();
                            TechnologiesFarmModels FarmModel = new TechnologiesFarmModels(idFarm, Convert.ToInt32(years), 0);
                            return View("IndexTechnologiesFarm", FarmModel);
                        }
                        break;
                    case "OkPestsAdd":
                        {
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера

                            var save = DBWork.SetDopTKR(idFarm, Convert.ToInt32(years), Convert.ToInt32(idWA), selectDiseases, selectPests);
                            int col = DBWork.GetWorkAreas(idFarm).Count();
                            TechnologiesFarmModels FarmModel = new TechnologiesFarmModels(idFarm, Convert.ToInt32(years), 0);
                            return View("IndexTechnologiesFarm", FarmModel);
                        }
                        break;
                    case "OkWeedAdd":
                        {
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера
                            string[] s = AllwAreas.Split(',');
                            for (int i = 1; i < s.Length; i++)
                            {
                                //var add= DBWork.SetWeedAreas(idFarm, s[i]);//передать в функцию idFarm и idWArea
                            }
                            //затем обновить модель с указанием выбранных участков .....пока отрисовываем по прежнему
                            int col = DBWork.GetWorkAreas(idFarm).Count();
                            TechnologiesFarmModels FarmModel = new TechnologiesFarmModels(idFarm, Convert.ToInt32(years), 0);
                            return View("IndexTechnologiesFarm", FarmModel);
                        }
                        break;
                    case "OkDiseaseAdd":
                        {
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера
                            string[] s = AllwAreas.Split(',');
                            for (int i = 1; i < s.Length; i++)
                            {
                                //var add= DBWork.SetDiseaseAreas(idFarm, s[i]);//передать в функцию idFarm и idWArea
                            }
                            //затем обновить модель с указанием выбранных участков .....пока отрисовываем по прежнему
                            int col = DBWork.GetWorkAreas(idFarm).Count();
                            TechnologiesFarmModels FarmModel = new TechnologiesFarmModels(idFarm, Convert.ToInt32(years), 0);
                            return View("IndexTechnologiesFarm", FarmModel);
                        }
                        break;
                    case "OkFertilizeAdd":
                        {
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера
                            string[] s = AllwAreas.Split(',');
                            for (int i = 1; i < s.Length; i++)
                            {
                                //var add= DBWork.SetDiseaseAreas(idFarm, s[i]);//передать в функцию idFarm и idWArea
                            }
                            //затем обновить модель с указанием выбранных участков .....пока отрисовываем по прежнему
                            int col = DBWork.GetWorkAreas(idFarm).Count();
                            TechnologiesFarmModels FarmModel = new TechnologiesFarmModels(idFarm, Convert.ToInt32(years), 0);
                            return View("IndexTechnologiesFarm", FarmModel);
                        }
                        break;

                    default:
                        {
                            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера
                            TechnologiesFarmModels FarmModel = new TechnologiesFarmModels(idFarm, 0, 0);
                            return View("IndexTechnologiesFarm", FarmModel);
                        }
                        break;
                }

            }
            else
            {
                TechnologiesFarmModels FarmModel = new TechnologiesFarmModels(idFarm, Convert.ToInt32(years), 0);
                return View("IndexTechnologiesFarm", FarmModel);
            }

        }
    }
}