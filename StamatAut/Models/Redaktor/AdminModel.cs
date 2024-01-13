using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary1;

namespace StamatAut.Models.Redaktor
{
    public class AdminModel
    {
        public string CurrentLink { get; set; }
        public string Action { get; set; }
        public List<Culture> Culture { get; set; }
        public List<Diseas> Disease { get; set; }
        public List<Pests> Pest { get; set; }
        public List<Stabil> Stabil { get; set; }
        public List<Soile> Soil { get; set; }
        public List<Climat> Climat { get; set; }
        public List<string> Clim { get; set; }        
        public List<TechnologicalOperations> TehOp { get; set; }
        public List<Operations> TechOperations { get; set; }
        public List<Operations> OperationLists { get; set; }
        public List<TechPacket> TehPaket { get; set; }

        public AdminModel()
        {
            this.CurrentLink = "";
        }

        public AdminModel(int idF, string CurrentLink, string Action)
        {
            this.CurrentLink = CurrentLink;
            this.Action = Action;
            switch (CurrentLink)
            {
                case "Culture":
                    {
                        Culture = DBWork.GetCulture();
                    }
                    break;
                case "Disease":
                    {
                        Disease = DBWork.GetDisease();
                    }
                    break;
                case "Pest":
                    {
                        Pest = DBWork.GetPest();
                    }
                    break;
                case "Stabilily":
                    {
                        Stabil = DBWork.GetStability();
                    }
                    break;
                case "Soil":
                    {
                        Soil = DBWork.GetSoil();
                        Climat = DBWork.GetAllClimate();
                        Clim = new List<string>();
                        foreach (var s in Soil)
                            Clim.Add(DBWork.GetClimateOnSoil(s.Id).Name);
                    }
                    break;
                case "Climat":
                    {
                        Climat = DBWork.GetAllClimate();
                    }
                    break;
                case "TehOp":
                    {
                        TehOp = DBWork.GetTO();
                        TechOperations = DBWork.GetOperationGroups();
                        OperationLists = DBWork.GetOperationLists(TechOperations);
                    }
                    break;
                default:
                    break;
            }
        }

        public AdminModel(int idF, string CurrentLink, string Action, string Id)
        {
            this.CurrentLink = CurrentLink;
            this.Action = Action;
            int.TryParse(Id, out int id);
            switch (CurrentLink)
            {
                case "Culture":
                    {
                        Culture = DBWork.GetCulture();
                        Culture.Where(p => p.Id == id).First().Editable = true;
                    }
                    break;
                case "Disease":
                    {
                        Disease = DBWork.GetDisease();
                        Disease.Where(p => p.Id == id).First().Editable = true;
                    }
                    break;
                case "Pest":
                    {
                        Pest = DBWork.GetPest();
                        Pest.Where(p => p.Id == id).First().Editable = true;
                    }
                    break;
                case "Stability":
                    {
                        Stabil = DBWork.GetStability();
                        Stabil.Where(p => p.Id == id).First().Editable = true;
                    }
                    break;
                case "Soil":
                    {
                        Soil = DBWork.GetSoil();
                        Soil.Where(p => p.Id == id).First().Editable = true;                        
                        Climat = DBWork.GetAllClimate();
                        Clim = new List<string>();
                        foreach (var s in Soil)
                            Clim.Add(DBWork.GetClimateOnSoil(s.Id).Name);
                    }
                    break;
                case "Climat":
                    {
                        Climat = DBWork.GetAllClimate();
                        Climat.Where(p => p.Id == id).First().Editable = true;
                    }
                    break;
                case "TehOp":
                    {
                        TehOp = DBWork.GetTO();
                        TechOperations = DBWork.GetOperationGroups();
                        OperationLists = DBWork.GetOperationLists(TechOperations);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}



        