using BaseballAPIproject.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BaseballAPIproject.Controllers
{
    public class HomeController : Controller
    {
        

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
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult PlayerData(string Search)
        {
            Search = Search.Replace(' ', '+');
            List<PlayerData> FoundPlayers = new List<PlayerData>();
            HttpWebRequest request = WebRequest.CreateHttp("http://lookup-service-prod.mlb.com/json/named.search_player_all.bam?sport_code=\'mlb\'&active_sw=\'Y\'&name_part=\'"+Search+"%25\'");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();
            JObject BaseballJson = JObject.Parse(data);
            List<PlayerData> output = new List<PlayerData>();
            PlayerData pr = new PlayerData();
            int count = BaseballJson["search_player_all"]["queryResults"]["row"].Count();
            if (count != 29)
            {
                ViewBag.Error = "Please be more specific";
                return View("Search");
            }
            string ID = BaseballJson["search_player_all"]["queryResults"]["row"]["player_id"].ToString();
            HttpWebRequest request2 = WebRequest.CreateHttp("http://lookup-service-prod.mlb.com/json/named.sport_hitting_tm.bam?league_list_id=\'mlb\'&game_type=\'R\'&season=\'2018\'&player_id=\'"+ID+"\'");
            HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
            StreamReader rd2 = new StreamReader(response2.GetResponseStream());
            string data2 = rd2.ReadToEnd();
            JObject BaseballJson2 = JObject.Parse(data2);
            //List<PlayerData> output2 = new List<PlayerData>();           
            pr.ID = ID;
            pr.FirstName = BaseballJson["search_player_all"]["queryResults"]["row"]["name_first"].ToString();
            pr.LastName = BaseballJson["search_player_all"]["queryResults"]["row"]["name_last"].ToString();
            pr.Postion = BaseballJson["search_player_all"]["queryResults"]["row"]["position"].ToString();
            if(pr.Postion != "P" && pr.Postion != "SP" && pr.Postion != "RP")
            {
                pr.PA = (int)BaseballJson2["sport_hitting_tm"]["queryResults"]["row"]["tpa"];
                pr.SO = (int)BaseballJson2["sport_hitting_tm"]["queryResults"]["row"]["so"];
                pr.SB = (int)BaseballJson2["sport_hitting_tm"]["queryResults"]["row"]["sb"];
                pr.BB = (int)BaseballJson2["sport_hitting_tm"]["queryResults"]["row"]["bb"];
                pr.SLG = (double)BaseballJson2["sport_hitting_tm"]["queryResults"]["row"]["slg"];
                pr.AVG = (double)BaseballJson2["sport_hitting_tm"]["queryResults"]["row"]["avg"];
                pr.OBP = (double)BaseballJson2["sport_hitting_tm"]["queryResults"]["row"]["obp"];
                pr.Babip = (double)BaseballJson2["sport_hitting_tm"]["queryResults"]["row"]["babip"];
            }
            
            else
            {
                HttpWebRequest request3 = WebRequest.CreateHttp("http://lookup-service-prod.mlb.com/json/named.sport_career_pitching.bam?league_list_id='mlb'&game_type='R'&player_id=\'"+ID+"\'");
                HttpWebResponse response3 = (HttpWebResponse)request3.GetResponse();
                StreamReader rd3 = new StreamReader(response3.GetResponseStream());
                string data3 = rd3.ReadToEnd();
                JObject BaseballJson3 = JObject.Parse(data3);
                pr.IP = (double)BaseballJson3["sport_career_pitching"]["queryResults"]["row"]["ip"];
                pr.ERA = (double)BaseballJson3["sport_career_pitching"]["queryResults"]["row"]["era"];             
            }

            ViewBag.BaseballDate = pr;
            return View();

        }
    }
}