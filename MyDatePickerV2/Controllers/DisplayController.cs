using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyDatePickerV2.Models;

namespace MyDatePickerV2.Controllers
{
    public class DisplayController : Controller
    {
        private MyAppointmentDBEntities db = new MyAppointmentDBEntities();

        // GET: Display
        public ActionResult Index()
        {
            return View(db.Appointments.ToList());
        }

        // GET: Display/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Display/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ContentResult GetImage()
        {


            // get the screen size.
            var Height = Int32.Parse(Request.Form["Height"]);
            var Width = Int32.Parse(Request.Form["Width"]);

            // select an image to display.
            var FilePath = "";
            if (Height >= 1000 && Width >= 1000)
            {
                FilePath = Server.MapPath("~/Uploads/") + "MacBook-Big.png";
            }
            else
            {
                FilePath = Server.MapPath("~/Uploads/") + "MacBook-Small.png";
            }

            // read the file
            byte[] FileBytes = System.IO.File.ReadAllBytes(FilePath);

            // convert bytes to a Base64 string.
            string FileBase64 = Convert.ToBase64String(FileBytes, 0, FileBytes.Length);

            // send to client
            return Content(FileBase64);

        }





        // GET: Display/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Display/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentID,EngineerID,Date")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appointment);
        }

        // GET: Display/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Display/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
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
    }
}
