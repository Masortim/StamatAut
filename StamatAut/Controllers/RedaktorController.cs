using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StamatAut.Models.Redaktor;
using ClassLibrary1;

namespace StamatAut.Controllers
{
    [Authorize()]
    public class RedaktorController : Controller
    {
        public int idFarm { get; set; }
        public bool Level { get; set; }

        [HttpGet]
        public ActionResult IndexRedaktor()
        {
            idFarm = DBWork.GetEntrance(HttpContext.User.Identity.Name);//получение idFarm по имени юзера            
            if (DBWork.GetAccessLevel(HttpContext.User.Identity.Name) == 1)
            {
                var Model = new AdminModel();
                Level = true;//администратор
                return View("IndexRedaktorAdmin", Model);
            }
            else
            {
                var Model = new UserModel();
                Level = false;
                return View("IndexRedaktorUser", Model);
            }
        }

        [HttpGet]
        public ActionResult Selector(string CurrentLink, string Action)
        {
            var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), CurrentLink, Action);
            return View("IndexRedaktorUser", Model);
        }

        [HttpGet]
        public ActionResult SelectorAdmin(string CurrentLink, string Action)
        {
            var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), CurrentLink, Action);
            return View("IndexRedaktorAdmin", Model);
        }

        [HttpPost]
        public ActionResult ButtonClickSort(string Id, string Name, string SR, string M1000, string CSonGa,
            string Product, string ProductMax, string GS0, string GS1, string Action, string TypeSort)
        {
            switch (Action)
            {
                case "Ok":
                    {
                        //код на сохранение
                        int.TryParse(Id, out int id);
                        DBWork.SetSort(DBWork.GetEntrance(HttpContext.User.Identity.Name), id, Name, SR, M1000, CSonGa, Product, ProductMax, GS0, GS1, TypeSort);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Culture", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "OkAdd":
                    {
                        DBWork.SetSort(DBWork.GetEntrance(HttpContext.User.Identity.Name), -1, Name, SR, M1000, CSonGa, Product, ProductMax, GS0, GS1, TypeSort);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Culture", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Edit":
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Culture", Action, Id);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Delete":
                    {
                        //код на удаление
                        int.TryParse(Id, out int id);
                        var d = DBWork.DeleteSort(DBWork.GetEntrance(HttpContext.User.Identity.Name), id);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Culture", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                default:
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Culture", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
            }
        }

        [HttpPost]
        public ActionResult ButtonClickFertilizer(string Id, string Name, string Reactant, string ContentOfPrep, string Unit, string Action)
        {
            switch (Action)
            {
                case "Ok":
                    {
                        //код на сохранение
                        int.TryParse(Id, out int id);
                        DBWork.SetFertilizer(DBWork.GetEntrance(HttpContext.User.Identity.Name), id, Name, Reactant, ContentOfPrep, Unit);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Fertilizer", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "OkAdd":
                    {
                        DBWork.SetFertilizer(DBWork.GetEntrance(HttpContext.User.Identity.Name), -1, Name, Reactant, ContentOfPrep, Unit);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Fertilizer", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Edit":
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Fertilizer", Action, Id);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Delete":
                    {
                        //код на удаление
                        int.TryParse(Id, out int id);
                        DBWork.DeleteFertilizer(DBWork.GetEntrance(HttpContext.User.Identity.Name), id);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Fertilizer", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                default:
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Fertilizer", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
            }
        }

        [HttpPost]
        public ActionResult ButtonClickChemical(string Id, string Name, string Action)
        {
            switch (Action)
            {
                case "Ok":
                    {
                        //код на сохранение
                        int.TryParse(Id, out int id);
                        //DBWork.SetHim(DBWork.GetEntrance(HttpContext.User.Identity.Name), id, Name, Reactant, ContentOfPrep, Unit);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Chemical", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "OkAdd":
                    {
                        //DBWork.SetHim(DBWork.GetEntrance(HttpContext.User.Identity.Name), -1, Name, Reactant, ContentOfPrep, Unit);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Chemical", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Edit":
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Chemical", Action, Id);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Delete":
                    {
                        //код на удаление
                        int.TryParse(Id, out int id);
                        //DBWork.DeleteHim(DBWork.GetEntrance(HttpContext.User.Identity.Name), id);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Chemical", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                default:
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Chemical", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
            }
        }

        [HttpPost]
        public ActionResult ButtonClickMachinery(string Id, string Name, string Type, string Price, string Kind, string Serial, string Cargo, string AmortTO,
            string Amort, string Class, string Action, string IdM)

        {
            switch (Action)
            {
                case "Ok":
                    {
                        //код на сохранение
                        int.TryParse(Id, out int id);
                        int.TryParse(IdM, out int idm);
                        DBWork.SetMachinery(id, Name, Type, float.Parse(Price), Kind, Serial, Convert.ToInt32(Cargo), float.Parse(Amort), float.Parse(AmortTO), Class, DBWork.GetEntrance(HttpContext.User.Identity.Name), idm);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Machinery", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "OkAdd":
                    {
                        int.TryParse(IdM, out int idm);
                        DBWork.SetMachinery(-1, Name, Type, float.Parse(Price), Kind, Serial, Convert.ToInt32(Cargo), float.Parse(Amort), float.Parse(AmortTO), Class, DBWork.GetEntrance(HttpContext.User.Identity.Name), idm);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Machinery", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Edit":
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Machinery", Action, Id);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Delete":
                    {
                        //код на удаление
                        int.TryParse(Id, out int id);
                        DBWork.DeleteMachinery(DBWork.GetEntrance(HttpContext.User.Identity.Name), id);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Machinery", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                default:
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Machinery", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
            }
        }

        public ActionResult ButtonClickBreaking(string Id, string Serial, string BeginDate, string EndDate, string Reason, string Action)
        // Скопирована с ButtonClickTrailers и переделано под Breaking

        {
            switch (Action)
            {
                case "Ok":
                    {
                        //код на сохранение - нужна функция БД SetBreaking
                        int.TryParse(Id, out int id);
                        DBWork.SetBreaking(id, Serial, BeginDate, EndDate, Reason);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Breaking", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "OkAdd":
                    {
                        DBWork.SetBreaking(-1, Serial, BeginDate, EndDate, Reason);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Breaking", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Edit":
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Breaking", Action, Id);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Delete":
                    {
                        //код на удаление
                        int.TryParse(Id, out int id);
                        DBWork.DeleteBreaking(id);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Breaking", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                default:
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Breaking", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
            }
        }

        public ActionResult ButtonClickStakeMech(string Id, string Stake, string Rank, string Action)
        // Скопирована с ButtonClickBreaking и переделано под StakeMech

        {
            switch (Action)
            {
                case "Ok":
                    {
                        //код на сохранение - нужна функция БД SetStakeMech
                        int.TryParse(Id, out int id);
                        DBWork.SetStakeMech(DBWork.GetEntrance(HttpContext.User.Identity.Name), id, Stake, Rank);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "StakeMech", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "OkAdd":
                    {
                        DBWork.SetStakeMech(DBWork.GetEntrance(HttpContext.User.Identity.Name), -1, Stake, Rank);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "StakeMech", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Edit":
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "StakeMech", Action, Id);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Delete":
                    {
                        //код на удаление
                        int.TryParse(Id, out int id);
                        DBWork.DeleteStakeMech(DBWork.GetEntrance(HttpContext.User.Identity.Name), id);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "StakeMech", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                default:
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "StakeMech", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
            }
        }

        public ActionResult ButtonClickMech(string Id, string Name, string Class, string Rank, string Action)
        // Скопирована с ButtonClickStakeMech и переделано под Mech

        {
            switch (Action)
            {
                case "Ok":
                    {
                        //код на сохранение - нужна функция БД SetMechanizator
                        int.TryParse(Id, out int id);
                        DBWork.SetMechanizator(DBWork.GetEntrance(HttpContext.User.Identity.Name), id, Name, Rank, Class);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Mechanics", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "OkAdd":
                    {
                        DBWork.SetMechanizator(DBWork.GetEntrance(HttpContext.User.Identity.Name), -1, Name, Rank, Class);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Mechanics", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Edit":
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Mechanics", Action, Id);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Delete":
                    {
                        //код на удаление
                        int.TryParse(Id, out int id);
                        DBWork.DeleteMechanizator(DBWork.GetEntrance(HttpContext.User.Identity.Name), id);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Mechanics", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                default:
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Mechanics", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
            }
        }
        public ActionResult ButtonClickMedic(string Id, string Name, string BeginDate, string EndDate, string Reason, string Action)
        // Скопирована с ButtonClickTrailers и переделано под Breaking

        {
            switch (Action)
            {
                case "Ok":
                    {
                        //код на сохранение - нужна функция БД SetMedic
                        int.TryParse(Id, out int id);
                        DBWork.SetMedic(id, Name, BeginDate, EndDate, Reason);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Medicine", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "OkAdd":
                    {
                        DBWork.SetMedic(-1, Name, BeginDate, EndDate, Reason);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Medicine", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Edit":
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Medicine", Action, Id);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Delete":
                    {
                        //код на удаление
                        int.TryParse(Id, out int id);
                        DBWork.DeleteMedic(id);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Medicine", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                default:
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Medicine", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
            }
        }


        [HttpPost]
        public JsonResult GetIdM(string name)
        {
            var c = DBWork.GetIdM(name);//получение id записи таблицы ClassMachine по имени Name машины (таблица Machinery)
            return Json(c, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTrailerQuantity(string name)
        {
            var c = DBWork.GetTrailerQuantity(name);//получение Quantity по имени Name Сельхозмашины
            return Json(c, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ButtonClickTrailers(string Id, string Name, string Price, string Cargo, string AmortTO,
           string Amort, string Quantity, string Action)
        // Скопирована с ButtonClickMachinery и переделано под Trailers

        {
            switch (Action)
            {
                case "Ok":
                    {
                        //код на сохранение - нужна функция БД SetTrailers
                        int.TryParse(Id, out int id);
                        DBWork.SetTrailers(id, Name, float.Parse(Price), Convert.ToInt32(Cargo), float.Parse(Amort), float.Parse(AmortTO), Convert.ToInt32(Quantity), DBWork.GetEntrance(HttpContext.User.Identity.Name));
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Trailers", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "OkAdd":
                    {
                        DBWork.SetTrailers(-1, Name, float.Parse(Price), Convert.ToInt32(Cargo), float.Parse(Amort), float.Parse(AmortTO), Convert.ToInt32(Quantity), DBWork.GetEntrance(HttpContext.User.Identity.Name));
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Trailers", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Edit":
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Trailers", Action, Id);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Delete":
                    {
                        //код на удаление
                        int.TryParse(Id, out int id);
                        DBWork.DeleteTrailer(DBWork.GetEntrance(HttpContext.User.Identity.Name), id);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Trailers", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                default:
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Trailers", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
            }
        }

        [HttpPost]
        public ActionResult ButtonClickAgregates(string Id, string Name, string Quantity, string Shirina, string Class, string Fuel, string Trailer,
                                                 List<string> AgregateOperation, List<string> AgregateOperationGSM, List<string> AgregateOperationNorm, string Action)

        {
            switch (Action)
            {
                case "Ok":
                    {
                        //код на сохранение
                        int.TryParse(Id, out int id);
                        DBWork.SetAgregates(id, Class + "+" + Trailer, Convert.ToInt32(Quantity), float.Parse(Shirina), Class, Fuel, Trailer, DBWork.GetEntrance(HttpContext.User.Identity.Name),
                                            AgregateOperation, AgregateOperationGSM, AgregateOperationNorm);
                        // Теперь нужно передать 89х2=178 значений ГСМ-затрат и норм выработки для 89 операций
                        // Непонятно, что будет, если пользователь добавит свои операции
                        // DBWork.SetAgregateOperations(id,, DBWork.GetEntrance(HttpContext.User.Identity.Name));
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Agregates", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "OkAdd":
                    {
                        DBWork.SetAgregates(-1, Class + "+" + Trailer, Convert.ToInt32(Quantity), float.Parse(Shirina), Class, Fuel, Trailer, DBWork.GetEntrance(HttpContext.User.Identity.Name),
                                            AgregateOperation, AgregateOperationGSM, AgregateOperationNorm);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Agregates", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Edit":
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Agregates", Action, Id);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Delete":
                    {
                        //код на удаление
                        int.TryParse(Id, out int id);
                        DBWork.DeleteAgregate(DBWork.GetEntrance(HttpContext.User.Identity.Name), id);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Agregates", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                default:
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Agregates", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
            }
        }

        [HttpPost]
        public ActionResult ButtonClickFuel(string Id, string Name, string Unit, string Cost, string Action)

        {
            switch (Action)
            {
                case "Ok":
                    {
                        //код на сохранение
                        int.TryParse(Id, out int id);
                        DBWork.SetKindOfFuel(DBWork.GetEntrance(HttpContext.User.Identity.Name), id, Name, Unit, Cost);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Fuels", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "OkAdd":
                    {
                        DBWork.SetKindOfFuel(DBWork.GetEntrance(HttpContext.User.Identity.Name), -1, Name, Unit, Cost);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Fuels", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Edit":
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Fuels", Action, Id);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                case "Delete":
                    {
                        //код на удаление
                        int.TryParse(Id, out int id);
                        DBWork.DeleteKindOfFuel(DBWork.GetEntrance(HttpContext.User.Identity.Name), id);
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Fuels", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
                default:
                    {
                        var Model = new UserModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Fuels", Action);
                        return View("IndexRedaktorUser", Model);
                    }
                    break;
            }
        }


        [HttpPost]
        public ActionResult ButtonClickCulture(string Id, string Name, string Action)
        {
            switch (Action)
            {
                case "Ok":
                    {
                        //код на сохранение
                        int.TryParse(Id, out int id);
                        DBWork.SetCulture(id, Name);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Culture", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                case "OkAdd":
                    {
                        DBWork.SetCulture(-1, Name);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Culture", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                case "Edit":
                    {
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Culture", Action, Id);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                case "Delete":
                    {
                        //код на удаление
                        int.TryParse(Id, out int id);
                        DBWork.DeleteCulture(id);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Culture", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                default:
                    {
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Culture", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
            }
        }

        [HttpPost]
        public ActionResult ButtonClickDisease(string Id, string Name, int Value, string Action)
        {
            switch (Action)
            {
                case "Ok":
                    {
                        //код на сохранение
                        int.TryParse(Id, out int id);
                        DBWork.SetDisease(id, Name, Value);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Disease", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                case "OkAdd":
                    {
                        DBWork.SetDisease(-1, Name, Value);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Disease", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                case "Edit":
                    {
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Disease", Action, Id);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                case "Delete":
                    {
                        //код на удаление
                        int.TryParse(Id, out int id);
                        DBWork.DeleteDisease(id);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Disease", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                default:
                    {
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Disease", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
            }
        }

        [HttpPost]
        public ActionResult ButtonClickPest(string Id, string Name, int Value, string Action)
        {
            switch (Action)
            {
                case "Ok":
                    {
                        //код на сохранение
                        int.TryParse(Id, out int id);
                        DBWork.SetPest(id, Name, Value);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Pest", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                case "OkAdd":
                    {
                        DBWork.SetPest(-1, Name, Value);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Pest", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                case "Edit":
                    {
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Pest", Action, Id);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                case "Delete":
                    {
                        //код на удаление
                        int.TryParse(Id, out int id);
                        DBWork.DeletePest(id);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Pest", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                default:
                    {
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Pest", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
            }
        }

        [HttpPost]
        public ActionResult ButtonClickStability(string Id, string Name, int Value, string Action)
        {
            switch (Action)
            {
                case "Ok":
                    {
                        //код на сохранение
                        int.TryParse(Id, out int id);
                        DBWork.SetStability(id, Name, Value);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Stability", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                case "OkAdd":
                    {
                        DBWork.SetStability(-1, Name, Value);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Stability", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                case "Edit":
                    {
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Stability", Action, Id);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                case "Delete":
                    {
                        //код на удаление
                        int.TryParse(Id, out int id);
                        DBWork.DeleteStability(id);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Stability", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                default:
                    {
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Stability", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
            }
        }

        [HttpPost]
        public ActionResult ButtonClickSoil(string Id, string Name, string Action, string NameClimat)
        {
            switch (Action)
            {
                case "Ok":
                    {
                        //код на сохранение
                        int.TryParse(Id, out int id);
                        DBWork.SetSoil(id, Name, NameClimat);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Soil", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                case "OkAdd":
                    {
                        DBWork.SetSoil(-1, Name, NameClimat);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Soil", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                case "Edit":
                    {
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Soil", Action, Id);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                case "Delete":
                    {
                        //код на удаление
                        int.TryParse(Id, out int id);
                        DBWork.DeleteSoil(id);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Soil", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                default:
                    {
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Soil", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
            }
        }

        public ActionResult ButtonClickTehOp(string Id, string Name, string Action, string NameGroup, string NameSubGroup, string NameTehOp)
        {
           switch (Action)
            {
               /* case "Ok":
                    {
                        //код на сохранение
                        int id;
                        int.TryParse(Id, out id);
                        DBWork.SetSoil(id, Name, NameClimat);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Soil", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;*/
                case "OkAddGroup"://Добавляем новую группу технологических операций в таблицу KindOfWork 
                    {
                        var v= DBWork.SetKindOfWork(-1, Name, "");
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "TehOp", Action);
                        return View("IndexRedaktorAdmin", Model);                        
                    }
                    break;
               /* case "OkAdd":
                    {
                        DBWork.SetSoil(-1, Name, NameClimat);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Soil", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;

                case "Edit":
                    {
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Soil", Action, Id);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
                case "Delete":
                    {
                        //код на удаление
                        int id;
                        int.TryParse(Id, out id);
                        DBWork.DeleteSoil(id);
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "Soil", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;*/
                default:
                    {
                        var Model = new AdminModel(DBWork.GetEntrance(HttpContext.User.Identity.Name), "TehOp", Action);
                        return View("IndexRedaktorAdmin", Model);
                    }
                    break;
            }
        }
    }
}