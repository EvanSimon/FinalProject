using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaseballAPIproject.Models
{
    public class PlayerData
    {

        public string ID { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Postion { get; set; }
        public int Age { get; set; }
        public int PA { get; set; }
        public int SO { get; set; }
        public int SB { get; set; }
        public double SLG { get; set; }
        public double AVG { get; set; }
        public double BB { get; set; }
        public double Babip { get; set; }
        public double OBP { get; set; }
        public double ERA { get; set; }
        public double IP { get; set; }


    }
}