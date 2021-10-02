using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using MHRS.Models;
using System.Web.Mvc;

namespace MHRS.Controllers
{
    public class HomeController : Controller
    {
        PKPMentalHealthRecoveryEntities db = new PKPMentalHealthRecoveryEntities();

        class DistinctItemComparer : IEqualityComparer<DassForm>
        {
            public bool Equals(DassForm x, DassForm y)
            {
                return x.Student_ID == y.Student_ID;
            }

            public int GetHashCode(DassForm obj)
            {
                return obj.Student_ID.GetHashCode();
            }
        }

        public ActionResult List_Requested()
        {
            List<DassForm> data = db.DassForms.Where(x => x.Condition != "Normal").OrderByDescending(x => x.Date).ToList();

            List<DassForm> err = data.Distinct(new DistinctItemComparer()).ToList();

            foreach (DassForm i in err.ToList())
            {
                if(db.Sessions.Any(x=>x.Student_ID == i.Student_ID && x.Date_End == null))
                {
                    err.RemoveAll(x => x.Student_ID == i.Student_ID);
                }

                if (db.Sessions.Any(x => x.Student_ID == i.Student_ID && x.Date_End > i.Date))
                {
                    err.RemoveAll(x => x.Student_ID == i.Student_ID);
                }
            }
            return View(err);
        }

        public ActionResult Create_Session(string id)
        {
            ViewBag.Student_ID = db.Students.FirstOrDefault(x => x.Student_ID == id).Student_ID;
            ViewBag.Student_Name = db.Students.FirstOrDefault(x => x.Student_ID == id).DisplayName;
            ViewBag.Status = db.DassForms.OrderByDescending(x=>x.Date).FirstOrDefault(x => x.Student_ID == id).Condition;

            return View();
        }

        [HttpPost]
        public ActionResult Create_Session(Session s)
        {
           

            string staffid = Session["staffid"].ToString();

            if (!ModelState.IsValid)
            {
                TempData["msg"] = "<script>alert('Please insert the Counseling session start date');</script>";
                return RedirectToAction("List_Sessions", "Home", new { id = staffid });
            }

            s.Staff_ID = Session["staffid"].ToString();
            s.Session_ID = s.Staff_ID + s.Student_ID + DateTime.Now.Minute.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString();
            db.Sessions.Add(s);
            try
            {
                db.SaveChanges();
            }
            catch 
            {
                TempData["msg"] = "<script>alert('There's problem while trying to request, please try again later');</script>";
            }

            return RedirectToAction("List_Sessions", "Home", new { id = staffid });
        }

        public ActionResult List_Sessions(string id)
        {
            Session["staffid"] = id;
            List<Session> sessions = new List<Session>();
            sessions = db.Sessions.Where(x => x.Staff_ID == id).ToList();
            return View(sessions);
        }

        public ActionResult Sessions(string id, string studentid)
        {
            //session id
            //var id = Session["ID"];
            var uid = "";
            TempData["msg"] = "";
            Models.Session c = new Session();

            if (!string.IsNullOrWhiteSpace(id) && string.IsNullOrWhiteSpace(studentid))
            {
                uid = Session["staffid"].ToString();
                Session["session"] = id;
                string studid = db.Sessions.FirstOrDefault(x => x.Session_ID == id).Student.Student_ID;
                if (db.Staffs.Any(x => x.Staff_ID == uid))
                {
                    float a = 0;

                    ViewBag.levelName = db.DassForms.Where(x => x.Student_ID == studid).OrderByDescending(x => x.Date).FirstOrDefault().Condition;
                    if (ViewBag.levelName == "Stress")
                    {
                        a = Convert.ToInt32(db.DassForms.Where(x => x.Student_ID == studid).OrderByDescending(x => x.Date).FirstOrDefault().Result_Stress);
                        ViewBag.currentLevel = ((a / 21) * 100).ToString("0.00");
                    }

                    if (ViewBag.levelName == "Depression")
                    {
                        a = Convert.ToInt32(db.DassForms.Where(x => x.Student_ID == studid).OrderByDescending(x => x.Date).FirstOrDefault().Result_Depression);
                        ViewBag.currentLevel = ((a / 21) * 100).ToString("0.00");
                    }

                    if (ViewBag.levelName == "Anxiety")
                    {
                        a = Convert.ToInt32(db.DassForms.Where(x => x.Student_ID == studid).OrderByDescending(x => x.Date).FirstOrDefault().Result_Anxiety);
                        ViewBag.currentLevel = ((a / 21) * 100).ToString("0.00");
                    }

                    ViewBag.currentuser = "staff";
                    c = db.Sessions.Where(X => X.Session_ID == id).FirstOrDefault();
                }
            } 

            if(!string.IsNullOrWhiteSpace(studentid))
            {
                if(db.Sessions.Any(x=>x.Student_ID == studentid && x.Date_End == null))
                {
                    float a = 0;
                    Session["studentid"] = studentid;
                    ViewBag.currentuser = "student";
                    ViewBag.levelName = db.DassForms.Where(x => x.Student_ID == studentid).OrderByDescending(x => x.Date).FirstOrDefault().Condition;
                    if(ViewBag.levelName == "Stress")
                    {
                        a = Convert.ToInt32(db.DassForms.Where(x => x.Student_ID == studentid).OrderByDescending(x => x.Date).FirstOrDefault().Result_Stress);
                        ViewBag.currentLevel = ((a / 21) * 100).ToString("0.00");
                    }

                    if (ViewBag.levelName == "Depression")
                    {
                        a = Convert.ToInt32(db.DassForms.Where(x => x.Student_ID == studentid).OrderByDescending(x => x.Date).FirstOrDefault().Result_Depression);
                        ViewBag.currentLevel = ((a/21) * 100).ToString("0.00");
                    }

                    if (ViewBag.levelName == "Anxiety")
                    {
                        a = Convert.ToInt32(db.DassForms.Where(x => x.Student_ID == studentid).OrderByDescending(x => x.Date).FirstOrDefault().Result_Anxiety);
                        ViewBag.currentLevel  = ((a / 21) * 100).ToString("0.00");
                    }

                    c = db.Sessions.Where(X => X.Student_ID == studentid && X.Date_End == null).FirstOrDefault();
                }
                else
                {
                    return RedirectToAction("No_Session_Yet", new { id = studentid});
                }
                
            }

           
            return View(c);
        }

        public ActionResult No_Session_Yet(string id)
        {
            Session["studentid"] = id;
            ViewBag.studentName = db.Students.FirstOrDefault(x => x.Student_ID == id).Fname;
            return View();
        }

        public ActionResult End_Session(string id)
        {
            string uid = id;
            Session currentSession = db.Sessions.Single(x => x.Session_ID == id);
            currentSession.Date_End = DateTime.Now;
            currentSession.End_status = "Recovered";

            try
            {
                db.SaveChanges();
            }
            catch
            {
                TempData["msg"] = "<script>alert('There's problem while trying to request, please try again later');</script>";
            }

            return RedirectToAction("Sessions", new { id = uid});
        }

        public ActionResult Chat(string id)
        {
            Models.Session c = db.Sessions.Where(X => X.Session_ID == id).FirstOrDefault();

            if(!string.IsNullOrEmpty(Session["staffid"] as string))
            {
                ViewBag.currentLogin = "staff";
            }
            else
            {
                ViewBag.currentLogin = "student";
            }

            return View(c);
        }

        public ActionResult List_Selected_Activites(string id)
        {
            List<Recommended_Activity> recommend = db.Recommended_Activity.Where(x => x.Session_ID == id).ToList();
            
            ViewBag.SessionID = id;

            if (!string.IsNullOrEmpty(Session["staffid"] as string))
            {
                ViewBag.isStaff = true;
            }
            else
            {
                ViewBag.isStaff = false;
            }

            return View(recommend);
        }

        public ActionResult Create_Recommended_Activity()
        {
            DateTime a = DateTime.Now;
            List<Activity> activities = db.Activities.Where(x => x.Activity_Date > a && x.Activity_Status != "Completed").ToList();
            return View(activities);
        }

        public ActionResult Add_Recommended_Activity(long id)
        {
            string sessionid = Session["session"].ToString();

            Recommended_Activity abc = new Recommended_Activity
            {
                Session_ID = sessionid,
                Activity_ID = id
            };

            db.Recommended_Activity.Add(abc);

            db.SaveChanges();

            return RedirectToAction("List_Selected_Activites", new { id = sessionid });
        }

        public ActionResult Remove_Recommended_Activity(long id)
        {
            db.Recommended_Activity.Remove(db.Recommended_Activity.FirstOrDefault(x => x.RA_ID == id));
            db.SaveChanges();

            string uid = Session["session"].ToString();

            return RedirectToAction("List_Selected_Activites", new { id = uid});
        }

        public ActionResult Join_Recommended_Activity(long id, string sessionerr)
        {
            string student_id = Session["studentid"].ToString();

            if(!db.Activity_Student.Any(x=>x.Student_ID == student_id && x.Activity_ID == id))
            {
                Activity_Student student = new Activity_Student
                {
                    Student_ID = student_id,
                    Activity_ID = id,
                    Student_Name = db.Students.FirstOrDefault(x => x.Student_ID == student_id).Fname + " " + db.Students.FirstOrDefault(x => x.Student_ID == student_id).Lname
                };

                db.Activity_Student.Add(student);
                db.SaveChanges();

                TempData["msg"] = "<script>alert('You have join the activity');</script>";
            }
            else
            {
                TempData["msg"] = "<script>alert('You have already join the activity');</script>";
            }

            return RedirectToAction("List_Selected_Activites", new { id = sessionerr });
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
    }
}