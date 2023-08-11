using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using MyDatePickerV2.Models;

namespace MyDatePickerV2.Controllers
{
    public class AppointmentsController : Controller
    {
        private MyAppointmentDBEntities db = new MyAppointmentDBEntities();

        // GET: Appointments
        public ActionResult Index()
        {
            return View(db.Appointments.ToList());
        }

        // GET: Appointments/Details/5
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

        // GET: Appointments/Create
        public ActionResult Create()
        {
            return View();
        }

        
        /*
        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentID,EngineerID,Date")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appointment);
        }
        */

        // GET: Appointments/Edit/5
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

        // POST: Appointments/Edit/5
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


        [HttpPost]
        public String CreateAppointment()
        {


            var appointment = new Appointment
            {
                StudentID = int.Parse(Request.Form["StudentID"]),
                EngineerID = int.Parse(Request.Form["EngineerID"]),
                Date = Request.Form["AppointmentDate"]
            };

            var vx = Request.Files["Attachment"].ContentLength;

            // Store the attachment in local storage.
            var Str1 = Request.Files[0].FileName.Split('.');
            var FileType = Str1[Str1.Length - 1];
            var FilePath =
                Server.MapPath("~/Uploads/") +
                string.Format(@"{0}", Guid.NewGuid()) +
                "." + FileType;
            Request.Files[0].SaveAs(FilePath);
            
            
            
            
            
            
            
            if (ModelState.IsValid)
            {
                // Add the appointment into the database.
                db.Appointments.Add(appointment);
                db.SaveChanges();

                // Send confirmation email.
                var mail = new MailMessage();
                mail.To.Add(new MailAddress(Request.Form["EmailAddress"]));
                mail.From = new MailAddress("Monash-fit5032-2023-T3@outlook.com");

                mail.Subject = "Appointment Conformation";
                mail.Body =
                    "You made an appointment:\n" +
                    "Student ID: " + Request.Form["StudentID"] + "\n" +
                    "Engineer ID: " + Request.Form["EngineerID"] + "\n" +
                    "Date: " + Request.Form["AppointmentDate"];
                mail.IsBodyHtml = false;

                var attachment = new System.Net.Mail.Attachment(FilePath);
                mail.Attachments.Add(attachment);

                var smtp = new SmtpClient();
                smtp.Host = "smtp.office365.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential
                    ("Monash-fit5032-2023-T3@outlook.com", "Monash2023#");

                smtp.Send(mail);



                return "Success";
            }

            return "Database Unavailable.";
        }



        // GET: Appointments/Delete/5
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

        // POST: Appointments/Delete/5
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
