using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BaseballAPIproject.Models;

namespace BaseballAPIproject.Controllers
{
    public class FreeAgentController : Controller
    {
        private FreeAgentsEntities db = new FreeAgentsEntities();

        // GET: FreeAgent
        public ActionResult Index()
        {
            return View(db.Free_Agents.ToList());
        }

        // GET: FreeAgent/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Free_Agent free_Agent = db.Free_Agents.Find(id);
            if (free_Agent == null)
            {
                return HttpNotFound();
            }
            return View(free_Agent);
        }

        // GET: FreeAgent/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FreeAgent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Pos,Prev_Team,Age,Prev_WAR,Proj_WAR,Avg_Years,Avg_Total,Med_Years,Med_Total,QO,Signing_Team,Date,Years,Salary,column_16,playerid")] Free_Agent free_Agent)
        {
            if (ModelState.IsValid)
            {
                db.Free_Agents.Add(free_Agent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(free_Agent);
        }

        // GET: FreeAgent/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Free_Agent free_Agent = db.Free_Agents.Find(id);
            if (free_Agent == null)
            {
                return HttpNotFound();
            }
            return View(free_Agent);
        }

        // POST: FreeAgent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,Pos,Prev_Team,Age,Prev_WAR,Proj_WAR,Avg_Years,Avg_Total,Med_Years,Med_Total,QO,Signing_Team,Date,Years,Salary,column_16,playerid")] Free_Agent free_Agent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(free_Agent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(free_Agent);
        }

        // GET: FreeAgent/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Free_Agent free_Agent = db.Free_Agents.Find(id);
            if (free_Agent == null)
            {
                return HttpNotFound();
            }
            return View(free_Agent);
        }

        // POST: FreeAgent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Free_Agent free_Agent = db.Free_Agents.Find(id);
            db.Free_Agents.Remove(free_Agent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult SalToWar()
        {
            List<Free_Agent> Player = new List<Free_Agent>();
            Player = db.Free_Agents.ToList();
            foreach (Free_Agent player in Player)
            {

                
                    if (player.Prev_WAR == null)
                {
                    player.Prev_WAR = 0;
                }

                if(player.Salary == "0" || player.Salary == null || player.Prev_WAR == 0)
                {
                    player.DolPerWar = ((double)player.Prev_WAR);
                }
                else 
                {
                    double temp = (double.Parse(player.Salary) / (double)player.Prev_WAR);
                    player.DolPerWar = (Math.Round(temp, 2));
                }              
            }
            ViewBag.PlayerList = Player.OrderByDescending(x=>x.DolPerWar).ToList();

            db.SaveChanges();
            return View();
        }

        public ActionResult Pitchers()
        {
            List<Free_Agent> Pitch = new List<Free_Agent>();
            List<Free_Agent> output = new List<Free_Agent>();
            Pitch = db.Free_Agents.ToList();

            foreach (Free_Agent P in Pitch)
            {
                if(P.Pos =="SP/RP"|| P.Pos == "SP"|| P.Pos == "RP")
                {
                    output.Add(P);
                    ViewBag.Pitcher = output;                 
                }             
            }
            return View();
        }     
        public ActionResult PostionPlayers()
        {
            List<Free_Agent> Postion = new List<Free_Agent>();
            List<Free_Agent> output2 = new List<Free_Agent>();
            Postion = db.Free_Agents.ToList();

            foreach (Free_Agent P in Postion)
            {
                if (P.Pos != "SP/RP" && P.Pos != "SP" && P.Pos != "RP")
                {
                    output2.Add(P);
                    ViewBag.Postion = output2;
                }
            }
            return View();
        }

        public ActionResult Calculator()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Calculator(double UserSal, double UserWar)
        {
            double answer = UserSal / UserWar;

            ViewBag.Answer = Math.Round(answer, 1);
            return View();
        }
    }
}
