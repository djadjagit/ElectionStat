using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectionStat.Models
{
    public class OiksCoords
    {
        public short? Oik { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }

    public class UiksInfo
    {
        public short? OiksNum { get; set; }
        public short? UiksNum { get; set; }
        public string UiksAdres { get; set; }
        public string UiksLatitude { get; set; }
        public string UiksLongitude { get; set; }
    }

    public class StartOiks
    {
        public string ScryptBody { get; set; }
        public string ScryptEnd { get; set; }
        public string[] UikNumbers = new string[1];
    }

    public class HouseAdres
    {
        public short? UiksNum { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Adres { get; set; }
    }

    public class RezTiks
    {
        public string ScryptBody { get; set; }
        public string[] TikNumbers = new string[1];
    }

    public class RezTable
    {
        public string Rayon { get; set; }//ТИК
        public int P1 { get; set; }//всего избирателей
        public int P78 { get; set; }//пришло на выборы
        public int K2_c { get; set; }//голосов за Грудинина
        public int K3_c { get; set; }//голосов за Жириновского
        public int K4_c { get; set; }//голосов за Путина
        public int K_c { get; set; }//голосов за всех остальных
        public decimal Lat { get; set; }//широта метки ТИКа
        public decimal Lon { get; set; }//долгота метки ТИКа
    }

    public class RezCalc
    {
        public decimal RezPercent1 { get; set; }//результат в % Грудинин
        public int RezWeight1 { get; set; }//вес результата Грудинина
        public decimal RezPercent2 { get; set; }//результат в % Жириновский
        public int RezWeight2 { get; set; }//вес результата Жириновского
        public decimal RezPercent3 { get; set; }//результат в % Путин
        public int RezWeight3 { get; set; }//вес результата Путина
        public decimal RezPercent4 { get; set; }//результат в % все остальные
        public int RezWeight4 { get; set; }//вес результата все остальные
        public decimal Vote { get; set; }//Явка
    }
}