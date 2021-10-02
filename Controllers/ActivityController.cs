using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MHRS.Models;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Data.Entity.Core.Objects;

namespace MHRS.Controllers
{
    public class ActivityController : Controller
    {
        PKPMentalHealthRecoveryEntities db = new PKPMentalHealthRecoveryEntities();

        public ActionResult Activity(string id)
        {
            Session["staffid"] = id;
            Session["activityID"] = "";
            var activity = db.Activities.ToList();
            return View(activity);
        }

        public ActionResult CreateActivity()
        {
            ViewBag.staffid = Session["staffid"].ToString();
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateActivity(Activity activity, HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
                using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                {
                    activity.Activity_Image = br.ReadBytes(postedFile.ContentLength);
                }

            DateTime newErr = new DateTime(activity.AcDate.Year, activity.AcDate.Month, activity.AcDate.Day, activity.AcTime.Hour, activity.AcTime.Minute, activity.AcTime.Second);
            activity.Activity_Date = newErr;

            db.Activities.Add(activity);
            Session["activity_id"] = activity.Activity_ID;
            try
            {
                db.SaveChanges();
            }
            catch
            {
                TempData["msg"] = "<script>alert(`Please fill all the required field before proceed`);</script>";
                return RedirectToAction("Activity", new { id = Session["staffid"].ToString() });
            }

            Session["activityID"] = activity.Activity_ID;

            return RedirectToAction("List_Activity_Staff");
        }

        public ActionResult Edit_Activity(long id)
        {
            ViewBag.staffid = Session["staffid"].ToString();
            Activity activity = db.Activities.FirstOrDefault(x => x.Activity_ID == id);
            return View(activity);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit_Activity(Activity activity, HttpPostedFileBase postedFile, string status)
        {
            Activity a = db.Activities.Single(x => x.Activity_ID == activity.Activity_ID);

            if (postedFile != null)
                using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                {
                    a.Activity_Image = br.ReadBytes(postedFile.ContentLength);
                }

            a.Activity_Name = activity.Activity_Name;
            a.Descriptions = activity.Descriptions;
            a.Activity_Date = activity.Activity_Date;

            if(status != null)
            {
                a.Activity_Status = "Completed";
            }
            else
            {
                a.Activity_Status = "";
            }


            try
            {
                db.SaveChanges();
            }
            catch
            {
                TempData["msg"] = "<script>alert('There's problem while trying to request, please try again later');</script>";
            }

            Session["activityID"] = activity.Activity_ID;

            return RedirectToAction("List_Activity_Staff");
        }

        public ActionResult Delete_Activity(long id)
        {
            string uid = Session["staffid"].ToString();
            db.Activity_Staff.RemoveRange(db.Activity_Staff.Where(x => x.Activity_ID == id));
            db.Activity_Student.RemoveRange(db.Activity_Student.Where(x => x.Activity_ID == id));
            
            db.SaveChanges();

            db.Activities.Remove(db.Activities.FirstOrDefault(x => x.Activity_ID == id));
            
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                TempData["msg"] = "<script>alert(" +e+ ");</script>";
            }
            return RedirectToAction("Activity", new { id = uid});
        }

        public ActionResult Add_Activity_Staff()
        {
            ViewData["Staffs"] = db.Staffs.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Add_Activity_Staff(Staff s)
        {
            var id = Convert.ToInt64(Session["activityID"].ToString());

            if (db.Activity_Staff.Where(x=>x.Activity_ID == id).Any(x=>x.Staff_ID == s.Staff_ID))
            {
                return RedirectToAction("List_Activity_Staff");
            }

            Activity_Staff activity_Staff = new Activity_Staff();
            activity_Staff.Activity_ID = id;
            activity_Staff.Staff_ID = s.Staff_ID;
            activity_Staff.Staff_Name = db.Staffs.Where(X => X.Staff_ID == s.Staff_ID).FirstOrDefault().Fname + " " + db.Staffs.Where(X => X.Staff_ID == s.Staff_ID).FirstOrDefault().Lname;
            db.Activity_Staff.Add(activity_Staff);
            try
            {
                db.SaveChanges();
            }
            catch
            {
                TempData["msg"] = "<script>alert(`There's problem while trying to request, please try again later`);</script>";
            }

            return RedirectToAction("List_Activity_Staff");
        }

        public ActionResult List_Activity_Staff()
        {
            var id = Convert.ToInt64(Session["activityID"].ToString());
            List<Activity_Staff> staffs = new List<Activity_Staff>();
            staffs = db.Activity_Staff.Where(x => x.Activity_ID == id).ToList();
            return View(staffs);
        }

        public ActionResult Delete_Activity_Staff(long id)
        {
            db.Activity_Staff.Remove(db.Activity_Staff.FirstOrDefault(x => x.Activity_Staff_ID == id));
            try
            {
                db.SaveChanges();
            }
            catch
            {
                TempData["msg"] = "<script>alert(`There's problem while trying to request, please try again later`);</script>";
            }

            return RedirectToAction("List_Activity_Staff");
        }

        public ActionResult Attendance(string id)
        {
            long erid = long.Parse(id);
            string uid = Session["staffid"].ToString();
            var attends = db.Activity_Student.Where(X => X.Activity_ID == erid).OrderBy(x=>x.Attend).ThenBy(x => x.Student_Name).ToList();

            if(attends.Count == 0)
            {
                TempData["msg"] = "<script>alert(`There's no student joining the activity yet`);</script>";
                return RedirectToAction("Activity", new { id = uid});
            }

            return View(attends);
        }

        public ActionResult Attend(int id, string student_id, string activityid)
        {
            Activity_Student student_activity = new Activity_Student();
            student_activity = db.Activity_Student.Single(x => x.Student_ID == student_id && x.Activity_Student_ID == id);
            student_activity.Attend = "Yes";
            try
            {
                db.SaveChanges();
            }
            catch
            {
                TempData["msg"] = "<script>alert(`There's problem while trying to request, please try again later`);</script>";
            }

            return RedirectToAction("Attendance", new { id = activityid});
        }

        public ActionResult Not_Attend(int id, string student_id, string activityid)
        {
            Activity_Student student_activity = new Activity_Student();
            student_activity = db.Activity_Student.Single(x => x.Student_ID == student_id && x.Activity_Student_ID == id);
            student_activity.Attend = "No";
            try
            {
                db.SaveChanges();
            }
            catch
            {
                TempData["msg"] = "<script>alert(`There's problem while trying to request, please try again later`);</script>";
            }

            return RedirectToAction("Attendance", new { id = activityid });
        }

        public ActionResult List_Meeting_Record(string id)
        {
            id = Session["session"].ToString();
            var list_meeting = db.Meeting_Record.Where(x => x.Session_ID == id).ToList();
            return View(list_meeting);
        }

        public ActionResult Create_Meeting_Record()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create_Meeting_Record(Meeting_Record meeting)
        {
            string id = Session["session"].ToString();
            meeting.Session_ID = id;
            db.Meeting_Record.Add(meeting);
            try
            {
                db.SaveChanges();
            }
            catch
            {
                TempData["msg"] = "<script>alert(`There's problem while trying to request, please try again later`);</script>";
            }

            return RedirectToAction("List_Meeting_Record");
        }

        public ActionResult Display_Meeting_Record(int id)
        {
            Meeting_Record d = db.Meeting_Record.FirstOrDefault(X => X.Meeting_ID == id);
            return View(d);
        }

        public ActionResult Edit_Meeting_Record(int id)
        {
            Meeting_Record d = db.Meeting_Record.FirstOrDefault(X => X.Meeting_ID == id);
            return View(d);
        }

        [HttpPost]
        public ActionResult Edit_Meeting_Record(Meeting_Record meeting)
        {
            Meeting_Record d = db.Meeting_Record.FirstOrDefault(x => x.Meeting_ID == meeting.Meeting_ID);
            d.Discussion = meeting.Discussion;
            d.Meet_Date = meeting.Meet_Date;
            d.Patient_Status = meeting.Patient_Status;
            d.Topic = meeting.Topic;
            try
            {
                db.SaveChanges();
            }
            catch
            {
                TempData["msg"] = "<script>alert(`There's problem while trying to request, please try again later`);</script>";
            }

            return RedirectToAction("List_Meeting_Record");
        }

        public ActionResult Delete_Meeting_Record(int id)
        {
            db.Meeting_Record.Remove(db.Meeting_Record.FirstOrDefault(x => x.Meeting_ID == id));
            try
            {
                db.SaveChanges();
            }
            catch
            {
                TempData["msg"] = "<script>alert(`There's problem while trying to request, please try again later`);</script>";
            }

            return RedirectToAction("List_Meeting_Record");
        }

        public ActionResult Activity_Report()
        {
            List<Graph_Activity> graph1 = new List<Graph_Activity>();
            List<Graph_Activity_Month> graphMonth = new List<Graph_Activity_Month>();

            var i = db.Activities.ToList();

            i.ForEach(x =>
            {
                var students = db.Activity_Student.Where(y => y.Activity_ID == x.Activity_ID).Count();

                graph1.Add(new Graph_Activity(x.Activity_Name, students));
            });



            for(int j = 1; j < 13; j++)
            {
                int januari = db.Activity_Student.Where(y => y.Activity.Activity_Date.Month == j).Count();
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(j);

                graphMonth.Add(new Graph_Activity_Month(monthName, januari));
            }

            ViewBag.Faculty = db.Students.GroupBy(x => x.Faculty).Select(y => y.FirstOrDefault()).ToList();
            ViewBag.graphMonth = JsonConvert.SerializeObject(graphMonth);
            ViewBag.graph = JsonConvert.SerializeObject(graph1);
            ViewBag.Data1 = graph1;
            ViewBag.Data2 = graphMonth;
            ViewBag.erMonth = "Month";

            return View();
        }

        [HttpPost]
        [Obsolete]
        public ActionResult Activity_Report(string faculty, string inSession, string condition, string attend, string byDate)
        {

            List<Graph_Activity> graph1 = new List<Graph_Activity>();
            List<Graph_Activity_Month> graphMonth = new List<Graph_Activity_Month>();

            var i = db.Activities.ToList();

            List<Activity_Student> hey = new List<Activity_Student>();

            i.ForEach(x =>
            {

                hey = db.Activity_Student.Where(y => y.Activity_ID == x.Activity_ID).ToList();

                if (faculty != "all")
                {
                    hey = hey.Where(y => y.Student.Faculty == faculty).ToList();
                }

                if(inSession != "all")
                {
                    List<Activity_Student> hm = new List<Activity_Student>();
                    hey.ForEach(y =>
                    {
                        if (db.Sessions.Any(z => z.Student_ID == y.Student_ID))
                        {
                            hm.Add(y);
                        }
                    });

                    hey = hm;
                }

                if(condition == "depress")
                {
                    List<Activity_Student> hm = new List<Activity_Student>();
                    hey.ForEach(y =>
                    {
                        if (db.Sessions.Any(z => z.Student_ID == y.Student_ID && z.Status == "Depression"))
                        {
                            hm.Add(y);
                        }
                    });

                    hey = hm;
                }

                if (condition == "anxiety")
                {
                    List<Activity_Student> hm = new List<Activity_Student>();
                    hey.ForEach(y =>
                    {
                        if (db.Sessions.Any(z => z.Student_ID == y.Student_ID && z.Status == "Anxiety"))
                        {
                            hm.Add(y);
                        }
                    });
                    hey = hm;
                }

                if (condition == "stress")
                {
                    List<Activity_Student> hm = new List<Activity_Student>();
                    hey.ForEach(y =>
                    {
                        if (db.Sessions.Any(z => z.Student_ID == y.Student_ID && z.Status == "Stress"))
                        {
                            hm.Add(y);
                        }
                    });
                    hey = hm;
                }

                if (condition == "normal")
                {
                    List<Activity_Student> hm = new List<Activity_Student>();
                    hey.ForEach(y =>
                    {
                        if (db.Sessions.Any(z => z.Student_ID == y.Student_ID && z.End_status == "Recovered"))
                        {
                            hm.Add(y);
                        }
                    });
                    hey = hm;
                }

                if(attend == "attend")
                {
                    hey = hey.Where(c => c.Attend == "Yes").ToList();
                }

                if (attend == "notAttend")
                {
                    hey = hey.Where(c => c.Attend == "No").ToList();
                }


                graph1.Add(new Graph_Activity(x.Activity_Name, hey.Count));
            });

            if (byDate == "month")
            {


                for (int j = 1; j < 13; j++)
                {
                    hey = db.Activity_Student.Where(y => y.Activity.Activity_Date.Month == j).ToList();

                    if (faculty != "all")
                    {
                        hey = hey.Where(y => y.Student.Faculty == faculty).ToList();
                    }

                    if (inSession != "all")
                    {
                        List<Activity_Student> hm = new List<Activity_Student>();
                        hey.ForEach(y =>
                        {
                            if (db.Sessions.Any(z => z.Student_ID == y.Student_ID))
                            {
                                hm.Add(y);
                            }
                        });

                        hey = hm;
                    }

                    if (condition == "depress")
                    {
                        List<Activity_Student> hm = new List<Activity_Student>();
                        hey.ForEach(y =>
                        {
                            if (db.Sessions.Any(z => z.Student_ID == y.Student_ID && z.Status == "Depression"))
                            {
                                hm.Add(y);
                            }
                        });

                        hey = hm;
                    }

                    if (condition == "anxiety")
                    {
                        List<Activity_Student> hm = new List<Activity_Student>();
                        hey.ForEach(y =>
                        {
                            if (db.Sessions.Any(z => z.Student_ID == y.Student_ID && z.Status == "Anxiety"))
                            {
                                hm.Add(y);
                            }
                        });
                        hey = hm;
                    }

                    if (condition == "stress")
                    {
                        List<Activity_Student> hm = new List<Activity_Student>();
                        hey.ForEach(y =>
                        {
                            if (db.Sessions.Any(z => z.Student_ID == y.Student_ID && z.Status == "Stress"))
                            {
                                hm.Add(y);
                            }
                        });
                        hey = hm;
                    }

                    if (condition == "normal")
                    {
                        List<Activity_Student> hm = new List<Activity_Student>();
                        hey.ForEach(y =>
                        {
                            if (db.Sessions.Any(z => z.Student_ID == y.Student_ID && z.End_status == "Recovered"))
                            {
                                hm.Add(y);
                            }
                        });
                        hey = hm;
                    }

                    if (attend == "attend")
                    {
                        hey = hey.Where(c => c.Attend == "Yes").ToList();
                    }

                    if (attend == "notAttend")
                    {
                        hey = hey.Where(c => c.Attend == "No").ToList();
                    }

                    int totalStud = hey.Count;
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(j);

                    graphMonth.Add(new Graph_Activity_Month(monthName, totalStud));
                }

                ViewBag.erMonth = "Month";
            }
            

            if(byDate == "day")
            {
                for (int j = 1; j < 8; j++)
                {

                    DateTime firstSunday = new DateTime(1753, 1, j);
                    hey = (from b in db.Activity_Student
                                   where EntityFunctions.DiffDays(firstSunday, b.Activity.Activity_Date) % 7 == 1
                                   select b).ToList();

                    if (faculty != "all")
                    {
                        hey = hey.Where(y => y.Student.Faculty == faculty).ToList();
                    }

                    if (inSession != "all")
                    {
                        List<Activity_Student> hm = new List<Activity_Student>();
                        hey.ForEach(y =>
                        {
                            if (db.Sessions.Any(z => z.Student_ID == y.Student_ID))
                            {
                                hm.Add(y);
                            }
                        });

                        hey = hm;
                    }

                    if (condition == "depress")
                    {
                        List<Activity_Student> hm = new List<Activity_Student>();
                        hey.ForEach(y =>
                        {
                            if (db.Sessions.Any(z => z.Student_ID == y.Student_ID && z.Status == "Depression"))
                            {
                                hm.Add(y);
                            }
                        });

                        hey = hm;
                    }

                    if (condition == "anxiety")
                    {
                        List<Activity_Student> hm = new List<Activity_Student>();
                        hey.ForEach(y =>
                        {
                            if (db.Sessions.Any(z => z.Student_ID == y.Student_ID && z.Status == "Anxiety"))
                            {
                                hm.Add(y);
                            }
                        });
                        hey = hm;
                    }

                    if (condition == "stress")
                    {
                        List<Activity_Student> hm = new List<Activity_Student>();
                        hey.ForEach(y =>
                        {
                            if (db.Sessions.Any(z => z.Student_ID == y.Student_ID && z.Status == "Stress"))
                            {
                                hm.Add(y);
                            }
                        });
                        hey = hm;
                    }

                    if (condition == "normal")
                    {
                        List<Activity_Student> hm = new List<Activity_Student>();
                        hey.ForEach(y =>
                        {
                            if (db.Sessions.Any(z => z.Student_ID == y.Student_ID && z.End_status == "Recovered"))
                            {
                                hm.Add(y);
                            }
                        });
                        hey = hm;
                    }

                    if (attend == "attend")
                    {
                        hey = hey.Where(c => c.Attend == "Yes").ToList();
                    }

                    if (attend == "notAttend")
                    {
                        hey = hey.Where(c => c.Attend == "No").ToList();
                    }

                    int totalStud = hey.Count;
                    
                    DateTime dayof = new DateTime(2020, 1, j);
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(dayof.DayOfWeek);

                    graphMonth.Add(new Graph_Activity_Month(monthName, totalStud));
                }

                ViewBag.erMonth = "Week";
            }


            ViewBag.Faculty = db.Students.GroupBy(x => x.Faculty).Select(y => y.FirstOrDefault()).ToList();
            ViewBag.graphMonth = JsonConvert.SerializeObject(graphMonth);
            ViewBag.graph = JsonConvert.SerializeObject(graph1);
            ViewBag.Data1 = graph1;
            ViewBag.Data2 = graphMonth;

            return View();
        }


    }
}