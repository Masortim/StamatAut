using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary1;

namespace StamatAut.Models.Redaktor
{
    public class UserModel
    {
        public int IdM { get; set; }
        public string CurrentLink { get; set; }
        public string Action { get; set; }
        public List<Sorte> Culture { get; set; }
        public List<string> Tsorte { get; set; }
        public List<string> TypeSorte { get; set; }
        public List<Himiz> Him { get; set; }
        public List<Fert> Fertilizer { get; set; }
        public List<Machin> Machine { get; set; }
        public List<string> TypeMachine { get; set; }
        public List<string> KindMachine { get; set; }
        public List<string> ClassMachine { get; set; }
        public List<ClassMashin> ClassMachineName { get; set; }
        public List<Trailing> Trailers { get; set; } // Тип данных Trailing должен быть задан в DBWork
        public List<Agreg> Agregate { get; set; } // Тип данных Agreg должен быть задан в DBWork
        public List<string> FuelNameFarm { get; set; }  // Тип данных Fuels должен быть задан в DBWork
        public List<Fuels> Fuel { get; set; } // Тип данных Toplivo должен быть задан в DBWork
        public List<string> ClassMachineNameFarm { get; set; } // Список уникальных имён Name записей таблицы Machinery.
        public List<string> TrailerNameFarm { get; set; } // Список уникальных имён Name записей таблицы Trailers.
        public List<Break> Crash { get; set; } // Тип данных Breaking должен быть задан в DBWork
        public List<MechStake> MechStakes { get; set; } // Тип данных MechStake должен быть задан в DBWork
        public List<string> Rank { get; set; } // Выдаём доступные разряды, т.к. каждый разряд в таблице StakesOfMech должен быть внесён 1 раз       
        public List<Mechan> Mechanic { get; set; } // Тип данных Mechan должен быть задан в DBWork
        public List<Medik> MedicList { get; set; } // Тип данных Medik должен быть задан в DBWork
        public List<Operations> TechOperations { get; set; }
        public List<Operations> OperationLists { get; set; } // Из таблицы TehnologicalOperations
        public List<AgregOper> AgregateOperationList { get; set; } // Из таблицы OpeartionsOfAgregate
        public UserModel()
        {
            this.CurrentLink = "";
        }

        public UserModel(int idF, string CurrentLink, string Action) //Добавление нового - без ID
        {
            this.CurrentLink = CurrentLink;
            this.Action = Action;
            switch (CurrentLink)
            {
                case "Culture":
                    {
                        Culture = DBWork.GetSort(idF);
                        Tsorte = new List<string>();
                        TypeSorte = DBWork.GetAllTypeSort();
                        foreach (var c in Culture)
                        {
                            Tsorte.Add(DBWork.GetTypeSort(c.Id).Name);
                        }
                    }
                    break;
                case "Machinery":
                    {
                        Machine = DBWork.GetMachineryList(idF); // список машин на агрохозяйстве, внесённых в БД
                        TypeMachine = new List<string>(); //Задаём список из 2х типов машин
                        TypeMachine.Add("Колёсный");
                        TypeMachine.Add("Гусеничный");
                        KindMachine = new List<string>(); //Задаём список из 2х производителей машин
                        KindMachine.Add("Отечественный");
                        KindMachine.Add("Зарубежный");
                        ClassMachineName = DBWork.GetClassMashin(); // Это список для Select'a поля Name, берётся из БД, таблица ClassMachine                       
                        IdM = -1;
                        ClassMachine = new List<string>(); // Задаём список из 9 тяговых классов тракторов
                        ClassMachine.Add("0,2");
                        ClassMachine.Add("0,6");
                        ClassMachine.Add("0,9");
                        ClassMachine.Add("1,4");
                        ClassMachine.Add("2");
                        ClassMachine.Add("3");
                        ClassMachine.Add("4");
                        ClassMachine.Add("6");
                        ClassMachine.Add("8");
                    }
                    break;
                case "Breaking":
                    {
                        Crash = DBWork.GetBreaking(idF); // список машин на агрохозяйстве, внесённых в БД; из них будем брать серийные номера                    
                        Machine = DBWork.GetMachineryList(idF); // список машин на агрохозяйстве, внесённых в БД
                    }
                    break;
                case "StakeMech":
                    {
                        MechStakes = DBWork.GetStakesMech(idF); // список cтавок механизаторов на агрохозяйстве, внесённых в БД
                        Rank = new List<string>(); // Задаём список из 7 разрядов механизаторов
                        Rank.Add("2");
                        Rank.Add("3");
                        Rank.Add("4");
                        Rank.Add("5");
                        Rank.Add("6");
                        Rank.Add("7");
                        Rank.Add("8");
                    }
                    break;
                case "Mechanics":
                    {
                        Mechanic = DBWork.GetMechanizator(idF); // список механизаторов агрохозяйства, внесённых в БД  
                        MechStakes = DBWork.GetStakesMech(idF); // список ставок механизаторов на агрохозяйстве, внесённых в БД
                        ClassMachine = new List<string>(); // Задаём список из 9 тяговых классов тракторов
                        ClassMachine.Add("0,2");
                        ClassMachine.Add("0,6");
                        ClassMachine.Add("0,9");
                        ClassMachine.Add("1,4");
                        ClassMachine.Add("2");
                        ClassMachine.Add("3");
                        ClassMachine.Add("4");
                        ClassMachine.Add("6");
                        ClassMachine.Add("8");
                    }
                    break;
                case "Medicine":
                    {
                        Mechanic = DBWork.GetMechanizator(idF); // список механизаторов агрохозяйства, внесённых в БД  
                        MedicList = DBWork.GetMedic(idF); // список ставок механизаторов на агрохозяйстве, внесённых в БД                        
                    }
                    break;
                case "Trailers": // Скопировано с кейса Machinery и переделано под Trailers
                    {
                        Trailers = DBWork.GetTrailerList(idF); // список сельхозмашин на агрохозяйстве, внесённых в БД - важная переменная!
                    }
                    break;
                case "Agregates": // Скопировано с кейса Machinery и переделано под Agregates
                    {
                        Agregate = DBWork.GetAgregateList(idF); // список агрегатов на агрохозяйстве, внесённых в БД
                        ClassMachineName = DBWork.GetClassMashin(); // Это список для Select'a поля Техника, берётся из БД, таблица ClassMachine
                        ClassMachineNameFarm = DBWork.GetClassMachineNames(idF); // Это список для Select'a поля "Наименование техники"
                        FuelNameFarm = DBWork.GetFuelNames(idF); // Это список для список Select'a поля Тип Топлива, берётся из БД, таблица KindOfFuel
                        TrailerNameFarm = DBWork.GetTrailerNames(idF); // Это список имён сельхозмашин на агрохозяйстве
                        Trailers = DBWork.GetTrailerList(idF); // Это список сельхозмашин на агрохозяйстве                        
                        TechOperations = DBWork.GetOperationGroups();
                        OperationLists = DBWork.GetOperationLists(TechOperations);
                        // WorksGroup = DBWork.GetWorkKinds();
                    }
                    break;
                case "Fuels": // Скопировано с кейса Agregates и переделано под Fuels
                    {
                        Fuel = DBWork.GetKindOfFuel(idF); // виды топлива на агрохозяйстве, внесённых в БД                                            
                    }
                    break;

                case "Chemical":
                    {
                        Him = DBWork.GetHim(idF);
                    }
                    break;
                case "Fertilizer":
                    {
                        Fertilizer = DBWork.GetFert(idF);
                    }
                    break;
                default:
                    break;
            }
        }

        public UserModel(int idF, string CurrentLink, string Action, string Id) // Редактирование - с ID
        {
            this.CurrentLink = CurrentLink;
            this.Action = Action;
            int id;
            int.TryParse(Id, out id);
            switch (CurrentLink)
            {
                case "Culture":
                    {
                        Culture = DBWork.GetSort(idF);
                        Culture.Where(p => p.Id == id).First().Editable = true;
                        Tsorte = new List<string>();
                        TypeSorte = DBWork.GetAllTypeSort();
                        foreach (var c in Culture)
                        {
                            Tsorte.Add(DBWork.GetTypeSort(c.Id).Name);
                        }
                    }
                    break;
                case "Machinery":
                    {
                        Machine = DBWork.GetMachineryList(idF); // список машин на агрохозяйстве, внесённых в БД;
                        var M = Machine.First(p => p.Id == id);
                        M.Editable = true;
                        TypeMachine = new List<string>(); //Задаём список из 2х типов машин
                        TypeMachine.Add("Колёсный");
                        TypeMachine.Add("Гусеничный");
                        KindMachine = new List<string>(); //Задаём список из 2х производителей машин
                        KindMachine.Add("Отечественный");
                        KindMachine.Add("Зарубежный");
                        ClassMachineName = DBWork.GetClassMashin(); // Это список для Select'a поля Name, берётся из БД, таблица ClassMachine
                        IdM = Convert.ToInt32(DBWork.GetIdM(M.Name)[0]);
                        ClassMachine = new List<string>(); // Задаём список из 9 тяговых классов тракторов
                        ClassMachine.Add("0,2");
                        ClassMachine.Add("0,6");
                        ClassMachine.Add("0,9");
                        ClassMachine.Add("1,4");
                        ClassMachine.Add("2");
                        ClassMachine.Add("3");
                        ClassMachine.Add("4");
                        ClassMachine.Add("6");
                        ClassMachine.Add("8");
                    }
                    break;
                case "Breaking":
                    {
                        Machine = DBWork.GetMachineryList(idF); // список машин на агрохозяйстве, внесённых в БД
                        Crash = DBWork.GetBreaking(idF); // список машин на агрохозяйстве, внесённых в БД;
                        var C = Crash.First(p => p.Id == id);
                        C.Editable = true;
                    }
                    break;
                case "Mechanics":
                    {
                        Mechanic = DBWork.GetMechanizator(idF); // список машин на агрохозяйстве, внесённых в БД                        
                        var C = Mechanic.First(p => p.Id == id);
                        C.Editable = true;
                        MechStakes = DBWork.GetStakesMech(idF); // список ставок механизаторов на агрохозяйстве, внесённых в БД
                        ClassMachine = new List<string>(); // Задаём список из 9 тяговых классов тракторов
                        ClassMachine.Add("0,2");
                        ClassMachine.Add("0,6");
                        ClassMachine.Add("0,9");
                        ClassMachine.Add("1,4");
                        ClassMachine.Add("2");
                        ClassMachine.Add("3");
                        ClassMachine.Add("4");
                        ClassMachine.Add("6");
                        ClassMachine.Add("8");
                    }
                    break;
                case "Medicine":
                    {
                        Mechanic = DBWork.GetMechanizator(idF); // список механизаторов агрохозяйства, внесённых в БД  
                        MedicList = DBWork.GetMedic(idF); // список ставок механизаторов на агрохозяйстве, внесённых в БД         
                        var C = MedicList.First(p => p.Id == id);
                        C.Editable = true;
                    }
                    break;
                case "StakeMech":
                    {
                        MechStakes = DBWork.GetStakesMech(idF); // список ставок механизаторов на агрохозяйстве, внесённых в БД                        
                        var C = MechStakes.First(p => p.Id == id);
                        C.Editable = true;
                        Rank = new List<string>(); // Задаём список из 7 разрядов механизаторов
                        Rank.Add("2");
                        Rank.Add("3");
                        Rank.Add("4");
                        Rank.Add("5");
                        Rank.Add("6");
                        Rank.Add("7");
                        Rank.Add("8");
                    }
                    break;
                case "Trailers":
                    {
                        Trailers = DBWork.GetTrailerList(idF); // список машин на агрохозяйстве, внесённых в БД;
                        Trailers.Where(p => p.Id == id).First().Editable = true;
                    }
                    break;
                case "Agregates": // Скопировано с кейса Machinery и переделано под Agregates
                    {
                        Agregate = DBWork.GetAgregateList(idF); // список агрегатов на агрохозяйстве, внесённых в БД
                        Agregate.First(p => p.Id == id).Editable = true; //чтобы работала кнопка редактирования
                        ClassMachineName = DBWork.GetClassMashin(); // Это список для Select'a поля Техника, берётся из БД, таблица ClassMachine
                        ClassMachineNameFarm = DBWork.GetClassMachineNames(idF); // Это список для Select'a поля "Наименование техники"
                        FuelNameFarm = DBWork.GetFuelNames(idF); // Это список для список Select'a поля Тип Топлива, берётся из БД, таблица KindOfFuel                        
                        Trailers = DBWork.GetTrailerList(idF); // Это список сельхозмашин на агрохозяйстве
                        TrailerNameFarm = DBWork.GetTrailerNames(idF); // Это список имён сельхозмашин на агрохозяйстве     
                        TechOperations = DBWork.GetOperationGroups();
                        OperationLists = DBWork.GetOperationLists(TechOperations);
                        AgregateOperationList = DBWork.GetAgregateOperations(id);
                    }
                    break;
                case "Fuels": // Скопировано с кейса Agregates и переделано под Fuels
                    {
                        Fuel = DBWork.GetKindOfFuel(idF); // виды топлива на агрохозяйстве, внесённых в БД
                        Fuel.First(p => p.Id == id).Editable = true;
                    }
                    break;
                case "Chemical":
                    {
                        Him = DBWork.GetHim(idF);
                        Him.Where(p => p.Id == id).First().Editable = true;
                    }
                    break;
                case "Fertilizer":
                    {
                        Fertilizer = DBWork.GetFert(idF);
                        Fertilizer.Where(p => p.Id == id).First().Editable = true;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}