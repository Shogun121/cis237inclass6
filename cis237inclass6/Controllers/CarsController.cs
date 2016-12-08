using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237inclass6.Models;

namespace cis237inclass6.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
        private CarsRCooleyEntities db = new CarsRCooleyEntities();

        // GET: Cars
        public ActionResult Index()
        {
            //Setup  a variable to hold the Cars Data Set
            DbSet<Car> CarsToSearch = db.Cars;

            //Setup some strings to hold the data that might be in the session.
            //If there is nothing in the session we can still use these variables
            //as a default value.
            string filterMake = "";
            string filterMin = "";
            string filterMax = "";

            //Define a min and max for the cylinders
            int min = 0;
            int max = 16;

            //Check to see if there is a value in the session, and if there is, assign it to the variable that
            //we setup to hold the value.
            if (Session["make"]!= null && !String.IsNullOrWhiteSpace((string)Session["make"]))
            {
                filterMake = (string)Session["make"];
            }
            if (Session["min"] != null && !String.IsNullOrWhiteSpace((string)Session["min"]))
            {
                filterMin = (string)Session["min"];
                min = Int32.Parse(filterMin);
            }
            if (Session["max"] != null && !String.IsNullOrWhiteSpace((string)Session["max"]))
            {
                filterMax = (string)Session["max"];
                max = Int32.Parse(filterMax);
            }
            //Do the filter on the CarsTOSearch Dataset. Use the where that we used before.
            //When doing EF work, only this time send in more lamda expressions to narrow it down further.
            //Since we setup default values fo each of the filter parameters, min, max, and FilterMake,
            //we can count on tihs always running with no errors.
            IEnumerable<Car> filtered=CarsToSearch.Where(car => car.cylinders >= min &&
                                                                car.cylinders <= max &&
                                                                car.make.Contains(filterMake));

            //Place the string representation of the values in the session into the ViewBag
            //so that they can be retrieved and displayed on the view.
            ViewBag.filterMake = filterMake;
            ViewBag.filterMin = filterMin;
            ViewBag.filterMax = filterMax;

            //Return the view with a filtered selection of the cars.
            return View(filtered);

            //This is what used to be returned before a filter was setup.
            //return View(db.Cars.ToList());
        }

        // GET: Cars/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,year,make,model,type,horsepower,cylinders")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(car);
        }

        // GET: Cars/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,year,make,model,type,horsepower,cylinders")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(car);
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //This is the filter method. It will take in the dat submitted from the form
        // and store it in the session so we can access it later.
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Contact()
        {
            ViewBag.Message ="Foo bar";
            return View();
        }
        //USe the JSON action result to send back the cars data transformed to JSON data.
        //This would be good for a web API.
        
        public ActionResult JSON()
        {
            
            return Json(db.Cars.ToList(),JsonRequestBehavior.AllowGet);
        }
        //This is the filter method. It will take in the data submitted from the form and 
        //store it in the session so we can access it alter.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter()
        {
            //return Content("Made it to Filter");
            //Get the form data that was sent out of the Request object.
            //The string that is used as a key to get the data matches the name property of the form control.
            //(for us this is the first parameter).
            String make = Request.Form.Get("make");
            String min = Request.Form.Get("min");
            String max = Request.Form.Get("max");

            //Store the form data into the session so that it can be retrieved later on to fliter the data.
            Session["make"] = make;
            Session["min"] = min;
            Session["max"] = max;

            //Redirect the user to the index page. We will do the work of actually filtering the list in the index method.
            return RedirectToAction("Index");
        }
        
    }
}
