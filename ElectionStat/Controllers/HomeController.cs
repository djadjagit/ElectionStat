using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectionStat.Models;

namespace ElectionStat.Controllers
{
    public class HomeController : Controller
    {
        //private uikadresaEntities db = new uikadresaEntities();

        [System.Data.Entity.DbFunction("uikadresaModel", "Str2Double")]
        public static decimal Str2Double(string coords) //смотри файл DataModel.edmx поле Function
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        public ActionResult Index()//стартовая страница
        {
            return View ();
        }

        public ActionResult Adreses()//ОИКи и УИКи
        {
            StartOiks start = new StartOiks();
            start.ScryptBody = "";
            start.ScryptEnd = "";
            using (uikadresaEntities db = new uikadresaEntities())
            {
                List<OiksCoords> Oiks = db.uiks.GroupBy(p => p.oik).Select(g => new OiksCoords()
                { Oik = g.Max(c => c.oik), Latitude = g.Average(c => Str2Double(c.lat)), Longitude = g.Average(c => Str2Double(c.lon)) }).ToList();
                List<UiksInfo> Uiks = db.uiks.Select(g => new UiksInfo()
                { OiksNum = g.oik, UiksNum = g.uik, UiksLatitude = g.lat, UiksLongitude = g.lon, UiksAdres = g.adres }).ToList();
                for (int i = 0; i < Uiks.Count; i++)
                {
                    start.ScryptBody += "myPlacemark" + i.ToString() + " = new ymaps.GeoObject({geometry:{ type: 'Point', coordinates: [" +
                        Uiks[i].UiksLatitude + ", " + Uiks[i].UiksLongitude + "]}, properties:{iconContent: '" + Uiks[i].UiksNum + 
                        "', hintContent:'" + Uiks[i].UiksAdres + "'}}, { preset: 'islands#violetStretchyIcon'});\nmyPlacemark" + i.ToString() + 
                        ".events.add('click', function() { $('select option[value="+ Uiks[i].UiksNum + "]').prop('selected', true); $('select option[value=" + 
                        Uiks[i].UiksNum + "]').trigger('change');});\nmyPolyline" + i.ToString() + " = new ymaps.Polyline([[" + Uiks[i].UiksLatitude + ", " + 
                        Uiks[i].UiksLongitude + "],[" + Oiks[(int)Uiks[i].OiksNum - 1].Latitude + ", " + Oiks[(int)Uiks[i].OiksNum - 1].Longitude + 
                        "]], {hintContent:'" + Uiks[i].UiksNum + "'}, {strokeColor: '#FF0000', strokeWidth: 2,strokeOpacity: 0.5});\n";
                    Array.Resize(ref start.UikNumbers, i + 1);
                    start.UikNumbers[i] = Uiks[i].UiksNum.ToString();
                }
                for (int i = 0; i < Oiks.Count; i++)
                {
                    start.ScryptBody += "myPieChart" + i.ToString() + " = new ymaps.Placemark([" + Oiks[i].Latitude + ", " +
                        Oiks[i].Longitude + "], {data:[{weight:" + (i + 1).ToString() + ", color:'#FF0000'}],hintContent:'ОИК № " +
                        (i + 1).ToString() + "'},{iconLayout: 'default#pieChart', " +
                        "iconPieChartRadius: 20, iconPieChartCoreRadius: 10, iconPieChartCoreFillStyle: '#FFFFFF', iconPieChartStrokeStyle: '#FFFFFF', " +
                        "iconPieChartStrokeWidth: 1});\n";
                }
                for (int i = 0; i < Uiks.Count; i++) start.ScryptEnd += ".add(myPlacemark" + i.ToString() + ").add(myPolyline" + i.ToString() + ")";
                for (int i = 0; i < Oiks.Count; i++) start.ScryptEnd += ".add(myPieChart" + i.ToString() + ")";
            }
            if (start.ScryptEnd != "") start.ScryptEnd = "window.myMap.geoObjects" + start.ScryptEnd + ";";
            Session["HouseAdresCount"] = 0;
            return View(start);
        }

        public ActionResult UikHousesAdres()//Адреса домов по выбранному УИКу
        {
            ViewData["UikSelect"] = Request["UikSelect"];
            short? prom = Convert.ToInt16((ViewData["UikSelect"].ToString()));
            int CountHauses = (int)Session["HouseAdresCount"];
            string StrOut = "";
            for (int i = 0; i < CountHauses; i++) StrOut+= ".remove(window.House" + i.ToString() + ")";
            if (StrOut != "") StrOut = "window.myMap.geoObjects" + StrOut + ";\n";
            using (uikadresaEntities db = new uikadresaEntities())
            {
                List<UiksInfo> Uik = db.uiks.Select(g => new UiksInfo()
                { OiksNum = g.oik, UiksNum = g.uik, UiksLatitude = g.lat, UiksLongitude = g.lon, UiksAdres = g.adres }).ToList().
                Where(p => p.UiksNum == prom).ToList();
                List<HouseAdres> Houses = db.adresa.Select(g => new HouseAdres()
                { UiksNum = g.uik, Latitude = g.lat, Longitude = g.lon, Adres = g.adres }).ToList().Where(p => p.UiksNum == prom).ToList();
                for (int i = 0; i < Houses.Count; i++)
                {
                    StrOut += "House" + i.ToString() + " = new ymaps.GeoObject({geometry:{ type: 'Point', coordinates: [" +
                        Houses[i].Latitude + ", " + Houses[i].Longitude + "]}, properties:{iconContent: '" + Houses[i].Adres + "', hintContent:'УИК № " +
                        ViewData["UikSelect"] + "'}}, { preset: 'islands#darkGreenStretchyIcon'});\n";
                }
                if (StrOut != "") StrOut += "window.myMap.geoObjects";
                for (int i = 0; i < Houses.Count; i++) StrOut += ".add(House" + i.ToString() + ")";
                if (StrOut != "") StrOut += ";\n";
                StrOut += "window.myMap.setCenter([" + Uik[0].UiksLatitude + ", " + Uik[0].UiksLongitude + "], 16);";
                Session["HouseAdresCount"] = Houses.Count;
            }
            return PartialView("UikHousesAdres", StrOut);
        }

        public ActionResult RezOblPrez2018View()//Результаты выборов президента 2018 по ТИКам Новгородской области
        {
            RezTiks rezTiks = new RezTiks();
            rezTiks.ScryptBody = "";
            using (uikadresaEntities db = new uikadresaEntities())
            {
                List<RezTable> Tiks = db.Database.SqlQuery<RezTable>("select * from RezOblPrez2018").ToList();//.Select(g => new RezTiksTable()
//                { Rayon = g.rayon, P1 = g.p1, P78 = g.p78, K2_c = g.k2_c, K3_c = g.k3_c, K4_c = g.k4_c, K_c = g.k_c, Lat = g.lat, Lon = g.lon }).ToList();
                for (int i = 0; i < Tiks.Count; i++)
                {
                    rezTiks.ScryptBody += "House" + i.ToString() + " = new ymaps.GeoObject({geometry:{ type: 'Point', coordinates: [" +
                        Houses[i].Lat + ", " + Houses[i].Lon + "]}, properties:{iconContent: '" + Houses[i].P1 + "', hintContent:'УИК № " +
                        ViewData["UikSelect"] + "'}}, { preset: 'islands#darkGreenStretchyIcon'});\n";
                }
                if (rezTiks.ScryptBody != "") rezTiks.ScryptBody += "window.myMap.geoObjects";
                for (int i = 0; i < Houses.Count; i++) rezTiks.ScryptBody += ".add(House" + i.ToString() + ")";
                if (rezTiks.ScryptBody != "") rezTiks.ScryptBody += ";\n";
//                rezTiks.ScryptBody += "window.myMap.setCenter([" + Uik[0].UiksLatitude + ", " + Uik[0].UiksLongitude + "], 16);";
                Session["HouseAdresCount"] = Houses.Count;
            }
            return View();
        }
    }
}