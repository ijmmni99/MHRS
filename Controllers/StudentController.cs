using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MHRS.Models;
using System.Web.Mvc;

namespace MHRS.Controllers
{
    public class StudentController : Controller
    {
        PKPMentalHealthRecoveryEntities db = new PKPMentalHealthRecoveryEntities();
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List_Activity(string studentid)
        {
            Session["studentid"] = studentid;
            var activity = db.Activities.Where(x=>x.Activity_Status != "Completed").ToList();
            return View(activity);
        }

        public ActionResult Join_Activity(long id)
        {
            string student_id = Session["studentid"].ToString();

            if (!db.Activity_Student.Any(x => x.Student_ID == student_id && x.Activity_ID == id))
            {
                Activity_Student student = new Activity_Student
                {
                    Student_ID = student_id,
                    Activity_ID = id,
                    Student_Name = db.Students.FirstOrDefault(x => x.Student_ID == student_id).Fname + " " + db.Students.FirstOrDefault(x => x.Student_ID == student_id).Lname,
                };

                db.Activity_Student.Add(student);

                try
                {
                    db.SaveChanges();
                    TempData["msg"] = "<script>alert('You Have Joined The Activity');</script>";
                }
                catch
                {
                    TempData["msg"] = "<script>alert('There's problem while trying to request, please try again later');</script>";
                }
            }
            else
            {
                TempData["msg"] = "<script>alert('You have already join the activity');</script>";
            }

            return RedirectToAction("List_Activity", new {studentid = student_id});
        }

        public ActionResult List_Joined_Activity()
        {
            string student_id = Session["studentid"].ToString();

            List<Activity_Student> err = new List<Activity_Student>();
            DateTime a = DateTime.Now;

            err = db.Activity_Student.Where(x => x.Student_ID == student_id && x.Activity.Activity_Date > a && x.Activity.Activity_Status != "Completed").ToList();

            return View(err);
        }

        public JsonResult GetEvents()
        {
            string student_id = Session["studentid"].ToString();

            DateTime a = DateTime.Now;

            List<Joined_Calendar> joined_s = new List<Joined_Calendar>();

            using (PKPMentalHealthRecoveryEntities dc = new PKPMentalHealthRecoveryEntities())
            {
               

                var events = dc.Activity_Student.Where(x => x.Student_ID == student_id && x.Activity.Activity_Date > a && x.Activity.Activity_Status != "Completed").ToList();
                int i = 0;

                events.ForEach(x =>
                {
                    Joined_Calendar err = new Joined_Calendar();

                    i++;
                    err.EventID = i;
                    err.Subject = x.Activity.Activity_Name;
                    err.Description = x.Activity.Descriptions;
                    err.Start = x.Activity.Activity_Date;
                    err.ThemeColor = "green";
                    err.IsFullDay = false;

                    joined_s.Add(err);
                });
                
                return new JsonResult { Data = joined_s, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public ActionResult LogBook(string id, string currentUser)
        {
            var logbooks = db.LogBooks.Where(x => x.Session_ID == id).OrderByDescending(x=>x.Date_Post).ToList();

            ViewBag.SessionID = id;

            ViewBag.StudentName = db.Sessions.FirstOrDefault(x => x.Session_ID == id).Student.Fname + " " + db.Sessions.FirstOrDefault(x => x.Session_ID == id).Student.Lname;

            ViewBag.currentuser = currentUser;

            if (currentUser == "staff")
                ViewBag.isStudent = false;
            else
                ViewBag.isStudent = true;

            return View(logbooks);
        }

        public ActionResult Display_LogBook(int id, string currentuser)
        {
            ViewBag.currentuser = currentuser;
            LogBook d = db.LogBooks.FirstOrDefault(x => x.LogBook_ID == id);
            return View(d);
        }

        public ActionResult Create_LogBook(string id)
        {
            ViewBag.SessionID = id;
            return View();
        }

        [HttpPost]
        public ActionResult Create_LogBook(LogBook log, string sessionid)
        {
            string session = sessionid;
            log.Session_ID = sessionid;
            log.Date_Post = DateTime.Now;
            db.LogBooks.Add(log);
            try
            {
                db.SaveChanges();
            }
            catch
            {
                TempData["msg"] = "<script>alert('There's problem while trying to request, please try again later');</script>";
            }
            return RedirectToAction("LogBook", new { id = session, currentUser = "student" });
        }

        public ActionResult Edit_LogBook(int id, string currentuser)
        {
            Session["hmm"] = currentuser;
            LogBook log = db.LogBooks.FirstOrDefault(x => x.LogBook_ID == id);
            return View(log);
        }

        [HttpPost]
        public ActionResult Edit_LogBook(LogBook log)
        {
           
            LogBook d = db.LogBooks.Single(x => x.LogBook_ID == log.LogBook_ID);
            string session = d.Session_ID;
            d.Title = log.Title;
            d.Description = log.Description;
            d.Date_Post = DateTime.Now;
            try
            {
                db.SaveChanges();
            }
            catch
            {
                TempData["msg"] = "<script>alert('There's problem while trying to request, please try again later');</script>";
            }
            return RedirectToAction("LogBook", new { id = session, currentUser = Session["hmm"].ToString() });
        }

        public ActionResult Delete_LogBook(int id, string currentusers)
        {
            string sessionid = db.LogBooks.FirstOrDefault(x => x.LogBook_ID == id).Session_ID;
            db.LogBooks.Remove(db.LogBooks.FirstOrDefault(X => X.LogBook_ID == id));
            try
            {
                db.SaveChanges();
            }
            catch
            {
                TempData["msg"] = "<script>alert('There's problem while trying to request, please try again later');</script>";
            }
            return RedirectToAction("LogBook", new { id = sessionid, currentUser = currentusers });
        }
    }
}