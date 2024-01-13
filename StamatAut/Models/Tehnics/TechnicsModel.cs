using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary1;
using DotNet.Highcharts;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;

namespace StamatAut.Models.Tehnics
{
    public class TechnicsModel
    {
        public string CurrentLink;
        public List<TehKart> TechKart;
        public List<Raspisanie> Raspis = new List<Raspisanie>();
        public Dictionary<int, string> TKR = new Dictionary<int, string>();
        public List<List<RGroups>> GroupTO { get; set; }
        public Highcharts GantTO { get; set; }
        public Highcharts Pic { get; set; }
        public Highcharts ChartEconom { get; set; }
        public List<NecessTechnics> CountAgregate = new List<NecessTechnics>();
        public Dictionary<string,int> CountM = new Dictionary<string, int>();
        public Dictionary<string, int> CountT = new Dictionary<string, int>();
        public string Tkr;
        

        public TechnicsModel(int idFarm, int year, string tkr, string currentLink)
        {
            Tkr = tkr;
            CurrentLink = currentLink;
            List<int> t = new List<int>();
            var res = tkr.Split(',');
            for (int i = 0; i < res.Count() - 1; i++)
            {
                var id = Convert.ToInt32(res[i]);
                t.Add(Convert.ToInt32(res[i]));
                var name = DBWork.GetTKRNameOnId(id);
                TKR.Add(id, name);
            }
            
            GantTO = GetGantTO(idFarm, year, t);
            //Алгоритм по теории расписаний
            //получаем список операций, сгруппированный по группам, со списком техники, расходом ГСМ и нормами выработки            
            GroupTO = DBWork.GetRGroups(t, idFarm);
            //цикл по группам, где i номер группы            
            for (int i = 0; i < GroupTO.Count(); i++)
            {
                List<AgregateAll> KolAgrAll = new List<AgregateAll>();
                //Цикл по операциям, где j -порядковый номер операции в i-й группе                
                for (int j = 0; j < GroupTO[i].Count(); j++)
                {
                    var TOij = GroupTO[i][j];
                    var Days = (TOij.EndDate - TOij.BeginDate).Days+1;
                    List<ZamenaAgregate> TableZamen = new List<ZamenaAgregate>();//таблица замен техники для j-й операции
                    int KolAgr;
                    //цикл по технике j-й операции в i-й греппе  
                    TOij.Agregate.Sort((x, y) => x.ShirZahv.CompareTo(y.ShirZahv));
                    foreach (var ag in TOij.Agregate)
                    {
                        //расчет количества агрегатов,необходимых для выполнения данной операции
                        if (TOij.MaxShirZahvat >= ag.ShirZahv)
                        {
                            int Chas = 10;//условно 10 часов в смену
                            var square = TOij.OperationName.Contains("Обкос") ? TOij.PoleSquare * 0.1f : TOij.PoleSquare;
                            KolAgr = (int)Math.Ceiling(Convert.ToDouble(square / (ag.SeedingRates * Chas * Days)));
                            var SR = ag.SeedingRates * KolAgr;
                            //расчет экономики                          
                            var econom = GetEconom(TOij, ag, KolAgr, Days);
                            //составляем таблицу замен
                            List<Zamena> ZamenaAg = new List<Zamena>();
                            foreach (var agZam in TOij.Agregate)
                            {
                                if (agZam != ag)
                                {
                                    var zamKolAgr = (int)Math.Ceiling(Convert.ToDouble(square / (agZam.SeedingRates * Chas * Days)));
                                    var zamSR = SR - agZam.SeedingRates * zamKolAgr;
                                    var Econom2 = GetEconom(TOij, agZam, zamKolAgr, Days);
                                    var zamEconom = econom[0] - Econom2[0];
                                    ZamenaAg.Add(new Zamena(agZam.Id, agZam.Name, zamSR, zamEconom, zamKolAgr));
                                }
                            }
                            ZamenaAg.Sort((x, y) => x.RaznEconom.CompareTo(y.RaznEconom));
                            TableZamen.Add(new ZamenaAgregate(ag.Id, ag.Name, SR, econom, KolAgr, ZamenaAg));
                        }
                    }
                    if (TableZamen.Count() == 0)
                    {
                        var ag = TOij.Agregate.First();
                        int Chas = 10;//условно 10 часов в смену
                        KolAgr = (int)Math.Ceiling(Convert.ToDouble(TOij.PoleSquare / (ag.SeedingRates * Chas * Days)));
                        var SR = ag.SeedingRates * KolAgr;
                        //расчет экономики  
                        var econom = GetEconom(TOij, ag, KolAgr, Days);
                        TableZamen.Add(new ZamenaAgregate(ag.Id, ag.Name, SR, econom, KolAgr, null));
                    }

                    //формируем первичное расписание по минимальным (по экономике) агрегатам    
                    var min = TableZamen.Min(p => p.EconomAll);
                    var MinAg = TableZamen.First(p => p.EconomAll == min);
                    Raspis.Add(new Raspisanie(TOij.IdTKR, TOij.IdTechKart, TOij.PoleSquare, TOij.OperationId, TOij.OperationName, MinAg, 
                        TOij.BeginDate.ToShortDateString(), TOij.EndDate.ToShortDateString(), (TOij.EndDate- TOij.BeginDate).Days+1));
                    //подсчет необходимого числа агрегатов и тректоров
                    if (KolAgrAll.FirstOrDefault(p => p.Name == MinAg.Name) != null)
                    {
                        KolAgrAll.FirstOrDefault(p => p.Name == MinAg.Name).CoutAll += MinAg.Count;
                    }
                    else
                        KolAgrAll.Add(new AgregateAll { IdAg = MinAg.IdAg, Name = MinAg.Name, CoutAll = MinAg.Count });
                }
                //проверка на наличие техники и корректировка расписания
                CountAgregate.AddRange(CountAg(idFarm, Raspis, GroupTO[i]));

            }

            var M = DBWork.GetMachineryList(idFarm);
            var T = DBWork.GetTrailerList(idFarm);
            foreach (var m in M)
            {
                if (!CountM.ContainsKey(m.ClassMachine))
                {
                    int i = 0;
                    foreach (var rr in CountAgregate)
                    {
                        foreach (var rrr in rr.NecessM)
                        {
                            if (rrr.ClassM == m.ClassMachine)
                                i = rrr.CountM > i ? rrr.CountM : i;
                        }
                    }
                    if (i > 0)
                        CountM.Add(m.ClassMachine, i);
                }
            }

            foreach (var tt in T)
            {
                if (!CountT.ContainsKey(tt.Name))
                {
                    int i = 0;
                    foreach (var rr in CountAgregate)
                    {
                        foreach (var rrr in rr.NecessT)
                        {
                            if (rrr.Trailer == tt.Name)
                                i = rrr.CountT > i ? rrr.CountT : i;
                        }
                    }
                    if (i > 0)
                        CountT.Add(tt.Name, i);
                }
            }
            var v = 1;
            Pic = GetPik(idFarm, year, t, Raspis, CountAgregate);
            ChartEconom = GetChartEconom(Raspis, t);


        }

        public Highcharts GetGantTO(int idFarm, int year, List<int> t)
        {
            var Options = new GlobalOptions()
            {
                Lang = new DotNet.Highcharts.Helpers.Lang()
                {
                    Loading = "Загрузка...",
                    Months = new string[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" },
                    ShortMonths = new string[] { "Янв", "Фев", "Мрт", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Нбр", "Дек" },
                    Weekdays = new string[] { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" },
                    ExportButtonTitle = "",
                    ResetZoom = "Масштаб",
                    ResetZoomTitle = "Вернуться к исходному масштабу",
                    PrintButtonTitle = "Печать",
                    DownloadJPEG = "Сохранить как *.jpg",
                    DownloadPDF = "Сохранить как *.pdf",
                    DownloadPNG = "Сохранить как *.png",
                    DownloadSVG = "Сохранить как *.svg",
                    DecimalPoint = ",",
                    ThousandsSep = "    "
                }
            };            
            List<string> CategoriesK = new List<string>();
            List<Series> SeriesAll = new List<Series>();
            // 
            var techKart = DBWork.GetTechKatr(idFarm, year);
            for (int j = 0; j < t.Count(); j++)
            {
                Data DataInterval = null;
                List<object[]> Interval = new List<object[]>();
                int r = 0;
                var SelectedTKR = DBWork.GetTKRNameOnId(t[j]);
                var ListTO = techKart.Where(c => c.IdTKR == t[j]).ToList();
                var dataChards = new List<DataChards>();
                CategoriesK.AddRange(ListTO.Select(p => p.TechOp).ToList());
                foreach (var to in ListTO)
                {
                    Interval.Add(new object[] {to.TechOp, new DateTime(to.DateBegin.Year, to.DateBegin.Month,to.DateBegin.Day),
                                                    new DateTime(to.DateEnd.Year,to.DateEnd.Month, to.DateEnd.Day)});
                    r++;
                }
                DataInterval = new Data(Interval.ToArray());
                var SeriesJ = new Series()
                {
                    Name = SelectedTKR,
                    Data = DataInterval,
                };
                SeriesAll.Add(SeriesJ);
            }
           var chart = new Highcharts("container")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Columnrange, Inverted = true, Width = 1200, Height = 600, ZoomType = ZoomTypes.Y })
                 .SetTitle(new Title { Text = "   " })
                 .SetXAxis(new XAxis()
                 {
                     Categories = CategoriesK.ToArray(),
                     LineColor = System.Drawing.Color.Black,
                     GridLineWidth = 1,                     
                 })
                 .SetYAxis(new YAxis()
                 {                     
                     Type = AxisTypes.Datetime,
                     LineWidth = 1,
                 })
                 .SetPlotOptions(new PlotOptions()
                 {
                     Columnrange = new PlotOptionsColumnrange()
                     {
                         PointWidth = 10,
                     }
                 })
                 .SetLegend(new Legend
                 {
                     Layout = Layouts.Vertical,
                     X = 0.1,
                     Y = 0.1
                 })
                 .SetTooltip(
                 new Tooltip()
                 {                             
                     Formatter = @"function () { return this.x +' : '+ Highcharts.dateFormat('%d.%m.%y', this.point.low) +' - '+ Highcharts.dateFormat('%d.%m.%y', this.point.high-864000) ;}",
                 })
                    .SetSeries(SeriesAll.ToArray());
            chart.SetOptions(Options);
            return chart;
        }

        public Highcharts GetPik(int idFarm, int year, List<int> t, List<Raspisanie> rasp, List<NecessTechnics> countAg)
        {
            List<string> CategoriesM = new List<string>();
            List<string> CategoriesT = new List<string>();            
            List<string> M = DBWork.GetClassMachineNames(idFarm);
            List<string> T = DBWork.GetTrailerNames(idFarm);
            List<Series> seriesM = new List<Series>();
            Series[] SeriesData = new Series[M.Count];
            Series[] seriesT = new Series[T.Count];
            foreach (var ca in countAg)
            {               
                CategoriesM.Add(ca.DateB.Day + "." + ca.DateB.Month + " - " + ca.DateE.Day + "." + ca.DateE.Month);
            }
            var chart = new Highcharts("countAg");
            for (int i = 0; i < M.Count; i++)
            {
                
                object[][] data = new object[CategoriesM.Count][];
                for (int j = 0; j < countAg.Count(); j++)
                {
                    var a = countAg[j].NecessM.FirstOrDefault(c => c.ClassM == M[i]);
                    data[j] = a != null ? new object[] { countAg[j].NecessM.First(c => c.ClassM == M[i]).CountM } : new object[] { 0 };

                }                
                seriesM.Add(new Series { Name = M[i], Data = new Data(data) });

                chart.InitChart(new Chart { DefaultSeriesType = ChartTypes.Column, Width = 1200, Height = 550, ZoomType = ZoomTypes.Y })
                    .SetTitle(new Title { Text = "Потребность в технике", Align = HorizontalAligns.Left })
                    .SetXAxis(new XAxis
                    {
                        Categories = CategoriesM.ToArray()
                    })
                    .SetYAxis(new YAxis
                    {
                        Min = 0,
                        MinTickInterval = 1,
                        Title = new YAxisTitle { Text = "Количество техники, шт." },
                        StackLabels = new YAxisStackLabels()
                        {
                            Enabled = true,
                        },
                    })
                    .SetLegend(new Legend
                    {
                        Align = HorizontalAligns.Center,
                        X = 70,
                        VerticalAlign = VerticalAligns.Top,
                        Y = 70,
                        Floating = true,
                        BackgroundColor = new BackColorOrGradient(System.Drawing.Color.White),
                        BorderColor = System.Drawing.Color.DarkGray,
                        BorderWidth = 1,
                        Shadow = false
                    })
                    .SetTooltip(new Tooltip
                    {
                    })
                    .SetPlotOptions(new PlotOptions
                    {
                        Column = new PlotOptionsColumn { Stacking = Stackings.Normal, DataLabels = new PlotOptionsColumnDataLabels { Enabled = true } },
                    })
                    .SetSeries(seriesM.ToArray());

            }
            return chart;   
        }

        public Highcharts GetChartEconom(List<Raspisanie> rasp, List<int> t)
        {
            var Options = new GlobalOptions()
            {
                Lang = new DotNet.Highcharts.Helpers.Lang()
                {
                    Loading = "Загрузка...",
                    Months = new string[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" },
                    ShortMonths = new string[] { "Янв", "Фев", "Мрт", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Нбр", "Дек" },
                    Weekdays = new string[] { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" },
                    ExportButtonTitle = "",
                    ResetZoom = "Масштаб",
                    ResetZoomTitle = "Вернуться к исходному масштабу",
                    PrintButtonTitle = "Печать",
                    DownloadJPEG = "Сохранить как *.jpg",
                    DownloadPDF = "Сохранить как *.pdf",
                    DownloadPNG = "Сохранить как *.png",
                    DownloadSVG = "Сохранить как *.svg",
                    DecimalPoint = ",",
                    ThousandsSep = "    "
                }
            };

            var chart = new Highcharts("container");           
            List<Series> S = new List<Series>();
            Series[] SeriesAll = new Series[t.Count];
            var xDataNameAll = new string[t.Count()][];
            var yDataCountAll = new Object[t.Count()][];
            for (int i=0; i < t.Count; i++)
            {                
                var rr = rasp.Where(p => p.IdTKR == t[i]).ToList();
                float gsm = 0;
                float toRem = 0;                
                float amort = 0;                
                float zarPl = 0;
                float all = 0;                
                foreach (var r in rr)
                {                    
                    gsm += r.Agregate.EconomGsm;
                    toRem += r.Agregate.EconomToRemM + r.Agregate.EconomToRemT;
                    amort += r.Agregate.EconomAmortM + r.Agregate.EconomAmortT;
                    zarPl += r.Agregate.EconomZarPl;
                    all += r.Agregate.EconomAll;
                }

                var dataChards = new List<DataChards>
                                    {
                                        new DataChards(){ DataName = "ГСМ", Count= gsm},             //  Затраты ГСМ                                            
                                        new DataChards(){DataName = "Оплата труда",Count=zarPl },
                                        new DataChards(){DataName = "ТО ремонт", Count=toRem},//ТО рем  
                                        new DataChards(){DataName = "Амортизация", Count= amort},
                                        new DataChards(){DataName = "Всего", Count= all }//Все затраты
                                    };
                var xDataName = dataChards.Select(j => j.DataName).ToArray();
                xDataNameAll[i] = xDataName; 
                var yDataCount = dataChards.Select(j => new object[] { j.Count }).ToArray();
                yDataCountAll[i] = yDataCount;
                SeriesAll[i] = new Series { Name = DBWork.GetTKRNameOnId(t[i]), Data = new Data(yDataCountAll[i]) };
            }            
            
            chart.InitChart(new Chart { DefaultSeriesType = ChartTypes.Column, Width = 1200, Height = 600 })
                .SetTitle(new Title { Text = "Прямые затраты" })
                .SetXAxis(new XAxis
                {
                    Categories = xDataNameAll[0],
                    Title = new XAxisTitle { Text = "   " },                    
                    Labels = new XAxisLabels
                    {
                        Enabled = true,
                        Rotation = -90,
                        Align = HorizontalAligns.Right,
                    },
                    TickColor = System.Drawing.Color.Black,//маленькие черточки на ОХ оси
                    LineColor = System.Drawing.Color.Black,//линия ОХ
                })
                .SetYAxis(new YAxis
                {
                    Title = new YAxisTitle { Text = "Затраты, тыс. руб" },
                    Labels = new YAxisLabels
                    {
                        Enabled = true,
                    },                  
                    LineWidth = 2,
                })
                .SetTooltip(new Tooltip
                {
                    Enabled = true,
                    Formatter = @"function () {return Highcharts.numberFormat(this.y, 2, ',',' ')+' тыс. руб';}",
                })
                .SetPlotOptions(new PlotOptions
                {                    
                    Column = new PlotOptionsColumn
                    {
                        PointPadding = 0.2,
                        DataLabels = new PlotOptionsColumnDataLabels
                        {
                            Enabled = true,
                            Rotation = -90,
                            Align = HorizontalAligns.Left,
                            VerticalAlign = VerticalAligns.Bottom,
                            X = 2,
                            Y = -10,
                            BorderWidth = 40,
                            Color = System.Drawing.Color.Black,
                            Formatter = @"function () {return Highcharts.numberFormat(this.y, 2, ',','<p> </p>');}",
                        },
                        EnableMouseTracking = true,
                    },
                })
                .SetSeries(SeriesAll)
                .SetLegend(new Legend
                {
                    Layout = Layouts.Vertical,
                    X = 0.1,
                    Y = 0.1
                }).SetOptions(Options);
            return chart;
        }

        public List<float> GetEconom(RGroups TOij, AgregateInfo ag, int KolAgr, int Days)
        {
            var square = TOij.OperationName.Contains("Обкос") ? TOij.PoleSquare*0.1f : TOij.PoleSquare;
            int Chas = 10;//условно 10 часов в смену           
            var SR = ag.SeedingRates * KolAgr;
            //расчет экономики на 1 га                          
            float normadni = square / (ag.SeedingRates * Chas);//количество нормаденей за 10 часовую смену
            //расчет экономики на 1 агрегат на весь объем работ без расчета механизаторов 
            float GSM = (ag.GSM * ag.CostGSM * ag.Koef * square) / 1000;//затраты ГСМ на весь объем, тыс. р
            float ToRemM = 0, ToRemT = 0, AmortM = 0, AmortT = 0, economAll,ZarPl;
            ZarPl = ag.ZarPl * square;
            List<float> econom= new List<float>();
            if (ag.PriceT!=0)
            {
                ToRemT = (ag.ToT / 100 + ag.PriceT) * normadni / ag.NormZT; //затратына ТО и ремонт сельхозмашины, тыс. р
                AmortT = (ag.PriceT / 100 * ag.PriceT) * normadni / ag.NormZT * ag.CountT; //затраты на амортизацию сельхозмашины, тыс. р    
            }
            if (ag.PriceM != 0)
            {
                ToRemM = (ag.ToM / 100 + ag.PriceM) * normadni / ag.NormZM; //затратына ТО и ремонт тратокора, тыс. р
                AmortM = (ag.PriceM / 100 * ag.PriceM) * normadni / ag.NormZM; //затраты на амортизацию трактора, тыс. р 
            }
            economAll = GSM + ToRemM + ToRemT + AmortM + AmortT+ ZarPl;
            //расчет экономических затрат на необходимое количество агрегатов для данной операции, тыс. р
            econom.Add(economAll * KolAgr);
            econom.Add(GSM * KolAgr);
            econom.Add(ToRemM * KolAgr);
            econom.Add(ToRemT * KolAgr);
            econom.Add(AmortM * KolAgr);
            econom.Add(AmortT * KolAgr);
            econom.Add(ZarPl * KolAgr);

            return econom;
        }

        public List<NecessTechnics> CountAg(int idFarm, List<Raspisanie> r, List<RGroups> groupTO)
        {
            List<NecessTechnics> result = new List<NecessTechnics>();
            //определяем группы по датам
            int idGroup = 0;
            DateTime dB = new DateTime(0001, 01, 01);
            DateTime dE = new DateTime(3001, 01, 01);
            for (int i = 0; i < groupTO.Count(); i++)
            {
                if (i == 0)
                {
                    dB = groupTO[i].BeginDate;
                    dE = groupTO[i].EndDate;
                    result.Add(new NecessTechnics { DateB = dB, DateE = dE });
                }
                else
                {
                    if (groupTO[i].BeginDate > dB)
                    {
                        result[idGroup].DateE = groupTO[i].BeginDate.AddDays(-1);
                        idGroup++;
                        dB = groupTO[i].BeginDate;
                        result.Add(new NecessTechnics { DateB = dB, DateE = dE });
                        if (groupTO[i].EndDate < dE)
                        {
                            result[idGroup].DateE = groupTO[i].EndDate;
                            idGroup++;
                            dB = groupTO[i].EndDate.AddDays(1);
                            result.Add(new NecessTechnics { DateB = dB, DateE = dE });
                        }
                        if (groupTO[i].EndDate > dE)
                        {
                            idGroup++;
                            dB = dE.AddDays(1);
                            dE = groupTO[i].EndDate;
                            result.Add(new NecessTechnics { DateB = dB, DateE = dE });
                        }
                    }
                    else
                    {
                        if (groupTO[i].EndDate < dE)
                        {
                            result[idGroup].DateE = groupTO[i].EndDate;
                            idGroup++;
                            dB = groupTO[i].EndDate.AddDays(1);
                            result.Add(new NecessTechnics { DateB = dB, DateE = dE });
                        }
                        if (groupTO[i].EndDate > dE)
                        {
                            idGroup++;
                            dB = dE.AddDays(1);
                            dE = groupTO[i].EndDate;
                            result.Add(new NecessTechnics { DateB = dB, DateE = dE });
                        }
                    }
                }
            }

            var A = DBWork.GetAgregateList(idFarm);
            var M = DBWork.GetMachineryList(idFarm);
            var T = DBWork.GetTrailerList(idFarm);

            for (int i = 0; i < result.Count(); i++)
            {          
                var grTO = groupTO.Where(c => c.BeginDate <= result[i].DateB && c.EndDate >= result[i].DateE).ToList();
                List<M> lm = new List<M>();
                List<T> lt = new List<T>();
                foreach (var g in grTO)
                {
                    var Ag = r.First(c => c.IdTehKart == g.IdTechKart && c.IdTO == g.OperationId).Agregate;
                    var ag = A.First(c=>c.Id == Ag.IdAg);                    
                    string classM = M.First(c=>c.ClassMachine==ag.ClassMachine).ClassMachine;
                    int MAll = M.Where(c => c.ClassMachine == ag.ClassMachine).Count();
                    var nameT = T.First(c=>c.Name==ag.Trailer).Name;
                    int TAll = T.First(c => c.Name == ag.Trailer).Quantity;
                    var m = lm.FirstOrDefault(c => c.ClassM == classM);
                    var t = lt.FirstOrDefault(c => c.Trailer == nameT);
                    if (m != null)
                    {
                        m.CountM += Ag.Count;
                        m.trueM = MAll >= m.CountM ? true : false;
                    }
                    else
                    {
                        lm.Add(new M { ClassM = classM, CountM = Ag.Count, trueM = MAll >= Ag.Count ? true : false });

                    }
                    if (t != null)
                    {
                        t.CountT += Ag.Count * ag.Quantity;
                        t.trueT = TAll >= t.CountT ? true : false;
                    }
                    else
                    {
                        lt.Add(new T { Trailer = nameT, CountT = Ag.Count * ag.Quantity, trueT = TAll >= Ag.Count * ag.Quantity ? true : false });
                    }
                }
                result[i].NecessM = lm;                
                result[i].NecessT = lt;            
            }
            return result;
        }
    }

    public class Zamena
    {
        int IdAg;
        public string Name;
        public float RaznSeedingRates;
        public float RaznEconom;
        public float Count;

        public Zamena(int idAg, string name, float sr, float econom, float count)
        {
            IdAg = idAg;            
            Name = name;
            RaznSeedingRates = sr;
            RaznEconom = econom;
            Count = count;
        }
    }

    public class ZamenaAgregate
    {
        public int IdAg;
        public string Name;
        public float SeedingRates;
        public float EconomAll;
        public float EconomGsm;
        public float EconomToRemM;
        public float EconomToRemT;
        public float EconomAmortM;
        public float EconomAmortT;
        public float EconomZarPl;
        public int Count;
        public List<Zamena> Zamen;

        public ZamenaAgregate(int idAg, string name, float sr, List<float> econom, int count, List<Zamena> zam)
        {
            IdAg = idAg;
            Name = name;
            SeedingRates = sr;
            EconomAll = econom[0];
            EconomGsm = econom[1];
            EconomToRemM = econom[2];
            EconomToRemT = econom[3];
            EconomAmortM = econom[4];
            EconomAmortT = econom[5];
            EconomZarPl = econom[6];
            Count = count;
            Zamen = zam;
        }
    }

    public class Raspisanie
    {
        public int IdTehKart;
        public int IdTO;
        public string NameTO;
        public int IdTKR;
        public float Square;
        public string DateB;
        public string DateE;
        public int KolDays;
        public ZamenaAgregate Agregate;

        public Raspisanie(int idTKR, int idTehKart, float square, int idTO,string nameTO, ZamenaAgregate agregate,string db, string de,int kolDays)
        {
            IdTKR = idTKR;
            IdTehKart = idTehKart;
            IdTO = idTO;
            Square = square;
            NameTO = nameTO;
            Agregate = agregate;
            DateB = db;
            DateE = de;
            KolDays = kolDays;
        }
    }

    public class AgregateAll
    {
        public int IdAg;
        public string Name;
        public int IDm;
        public int IDt;
        public int CoutAll;
        public DateTime DateB;
        public DateTime dateE;
    }

    //Требуемое число техники на период
    public class NecessTechnics
    {
        public DateTime DateB;
        public DateTime DateE;
        public List<M> NecessM;        
        public List<T> NecessT;        
    }

    public class M
    {
        public string ClassM;
        public int CountM;
        public bool trueM;
    }

    public class T
    {
        public string Trailer;
        public int CountT;
        public bool trueT;
    }
}