using MHRS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MHRS.Controllers
{
    public class HaoKeatHomeController : Controller
    {
        // GET: HaoKeatHome
        PKPMentalHealthRecoveryEntities abc = new PKPMentalHealthRecoveryEntities();
        public ActionResult index()
        {
            Session.Clear();
            return View();
        }

        public ActionResult staffhomepage(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staffData = abc.Staffs.FirstOrDefault(x => x.Staff_ID == id);
            if (staffData == null)
            {
                return HttpNotFound();
            }
            return View(staffData);
        }

        public ActionResult studenthomepage(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Student studentData = abc.Students.FirstOrDefault(x => x.Student_ID == id);
            ViewBag.Feature = abc.Activities.ToList();
            if (studentData == null)
            {
                return HttpNotFound();
            }
            return View(studentData);
        }


        public ActionResult studentlogin(Student a)
        {
            Session.Clear();
            if (!String.IsNullOrEmpty(a.Email) && !String.IsNullOrEmpty(a.Password))
            {
                String username = a.Email;
                String password = a.Password;


                var user = abc.Students.FirstOrDefault(c => c.Email == username && c.Password == password);
                if (user == null)
                {
                    TempData["msg"] = "<script>alert('No User found')</script>";

                }
                else
                {
                    Session.Clear();
                    Session["student"] = user.Student_ID;
                    Session["studentid"] = user.Student_ID;
                    return RedirectToAction("studenthomepage", new { id = user.Student_ID });
                }
            }
            return View();
        }
        public ActionResult stafflogin(Staff a)
        {
            if (!String.IsNullOrEmpty(a.Email) && !String.IsNullOrEmpty(a.Password))
            {
                String username = a.Email;
                String password = a.Password;


                var user = abc.Staffs.FirstOrDefault(c => c.Email == username && c.Password == password);
                if (user == null)
                {
                    TempData["msg"] = "<script>alert('No User found')</script>";

                }
                else
                {
                    if (user.Role_ID == 1)
                    {
                        Session.Clear();
                        Session["admin"] = user.Staff_ID;
                        return RedirectToAction("AdminHome","HusnaAdmin");
                    }
                    else if (user.Role_ID == 2)
                    {
                        Session.Clear();
                        Session["staff"] = user.Staff_ID;
                        Session["staffid"] = user.Staff_ID;
                        return RedirectToAction("staffhomepage", new { id = user.Staff_ID });
                    }
                }
            }
            return View();
        }
        public ActionResult studentedit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = abc.Students.Find(id);

            if (student == null)
            {
                return HttpNotFound();
            }

            ViewBag.Role_ID = new SelectList(abc.Roles, "Role_ID", "Permission", student.Role_ID);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult studentedit(Student student, HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                var whichdata = abc.Students.FirstOrDefault(x => x.Student_ID == student.Student_ID);

                student.Student_Image = new byte[postedFile.ContentLength];
                postedFile.InputStream.Read(student.Student_Image, 0, postedFile.ContentLength);

                whichdata.Student_Image = student.Student_Image;
                whichdata.Fname = student.Fname;
                whichdata.Lname = student.Lname;
                whichdata.Faculty = student.Faculty;
                whichdata.Course = student.Course;
                whichdata.Year = student.Year;
                whichdata.Semester = student.Semester;
                whichdata.Phone_No = student.Phone_No;
                whichdata.Email = student.Email;
                whichdata.Password = student.Password;



                abc.SaveChanges();
                return RedirectToAction("studenthomepage", new { id = student.Student_ID });
            }

            ViewBag.Role_ID = new SelectList(abc.Roles, "Role_ID", "Permission", student.Role_ID);
            return View(student);
        }


        public ActionResult studentregister()
        {
            ViewBag.Role_ID = new SelectList(abc.Roles, "Role_ID", "Permission");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult studentregister([Bind(Include = "Student_ID,Fname,Lname,Faculty,Course,Year,Semester,Phone_No,Email,Password")] Student student, HttpPostedFileBase postedFile)
        {
            if(postedFile != null)
            {
                if (!String.IsNullOrEmpty(student.Fname) && !String.IsNullOrEmpty(student.Lname) && !String.IsNullOrEmpty(student.Faculty) && !String.IsNullOrEmpty(student.Course) && !String.IsNullOrEmpty(student.Year) && !String.IsNullOrEmpty(student.Semester) && !String.IsNullOrEmpty(student.Phone_No) && !String.IsNullOrEmpty(student.Email) && !String.IsNullOrEmpty(student.Password))
                {
                    if (postedFile != null)
                    {

                        student.Student_Image = new byte[postedFile.ContentLength];
                        postedFile.InputStream.Read(student.Student_Image, 0, postedFile.ContentLength);
                        student.Role_ID = 3;
                        abc.Students.Add(student);
                        abc.SaveChanges();
                        return RedirectToAction("index");
                    }
                }
                else
                {
                    TempData["msg"] = "<script>alert('Please fill in the columns.')</script>";
                }
            }
            

            ViewBag.Role_ID = new SelectList(abc.Roles, "Role_ID", "Permission", student.Role_ID);
            return View(student);

        }

        public ActionResult staffregister()
        {
            ViewBag.Role_ID = new SelectList(abc.Roles, "Role_ID", "Permission");
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult staffregister([Bind(Include = "Staff_ID,Fname,Lname,Designation,Division,Unit,Phone_No,Email,Password,Role_ID")] Staff staff, HttpPostedFileBase postedFile)
        {
            if (!String.IsNullOrEmpty(staff.Fname) && !String.IsNullOrEmpty(staff.Lname) && !String.IsNullOrEmpty(staff.Designation) && !String.IsNullOrEmpty(staff.Division) && !String.IsNullOrEmpty(staff.Unit) && !String.IsNullOrEmpty(staff.Phone_No) && !String.IsNullOrEmpty(staff.Email) && !String.IsNullOrEmpty(staff.Password))
            {
                if (postedFile != null)
                {

                    staff.Staff_Image = new byte[postedFile.ContentLength];
                    postedFile.InputStream.Read(staff.Staff_Image, 0, postedFile.ContentLength);
                    abc.Staffs.Add(staff);
                    abc.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["msg"] = "<script>alert('Please fill in the columns.')</script>";
            }

            ViewBag.Role_ID = new SelectList(abc.Roles, "Role_ID", "Permission", staff.Role_ID);
            return View(staff);
        }


        // GET: Staffs/Edit/5
        public ActionResult staffedit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = abc.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            ViewBag.Role_ID = new SelectList(abc.Roles, "Role_ID", "Permission", staff.Role_ID);
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult staffedit(Staff staff, HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                var whichdata = abc.Staffs.FirstOrDefault(x => x.Staff_ID == staff.Staff_ID);


                staff.Staff_Image = new byte[postedFile.ContentLength];
                postedFile.InputStream.Read(staff.Staff_Image, 0, postedFile.ContentLength);
                whichdata.Staff_Image = staff.Staff_Image;

                whichdata.Fname = staff.Fname;
                whichdata.Lname = staff.Lname;
                whichdata.Designation = staff.Designation;
                whichdata.Division = staff.Division;
                whichdata.Unit = staff.Unit;
                whichdata.Phone_No = staff.Phone_No;
                whichdata.Email = staff.Email;
                whichdata.Password = staff.Password;
                whichdata.Role_ID = staff.Role_ID;

                abc.SaveChanges();
                return RedirectToAction("staffhomepage", new { id = staff.Staff_ID });
            }

            ViewBag.Role_ID = new SelectList(abc.Roles, "Role_ID", "Permission", staff.Role_ID);
            return View(staff);
        }

        public ActionResult viewstat(Session a)
        {
            if (a.Date_Start == null)
            {

                a.Date_Start = DateTime.Now;

            }
            return View(a);
        }

        public ActionResult viewlist(string Status)
        {
            var studentlist = abc.Sessions.ToList();

            if (!String.IsNullOrEmpty(Status))
            {
                studentlist = studentlist.Where(s => s.Status == Status).ToList();
            }

            return View(studentlist.ToList());
        }


        public ActionResult seechart1(DateTime dt)
        {//array for storing x-value and y-value
            DateTime dummy;
            DateTime.TryParseExact("1/1/0001", "MM-dd-yyyy", null, DateTimeStyles.None, out dummy);

            if (dt.Date == dummy.Date)
            {
                dt = DateTime.Now;
            }
            ArrayList xval = new ArrayList();
            ArrayList yval = new ArrayList();

            //select result set from database
            var result = abc.compareDateForchart(dt).ToList();

            //put result set into two array
            result.ToList().ForEach(x => xval.Add(x.Status));
            result.ToList().ForEach(x => yval.Add(x.Number));
            //setting up the chart

            var howmanyrecover = abc.Sessions.Where(x => x.End_status != null && (x.Date_End.Value.Month == dt.Month));
            if (howmanyrecover.ToList().Count().ToString() != "0")
            {
                xval.Add("recovered");
                yval.Add(howmanyrecover.ToList().Count().ToString());
            }

            new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla).AddTitle("Total Result").AddSeries("Default", chartType: "Column", xValue: xval, yValues: yval).SetYAxis(title: "Number of Students")
            .SetXAxis(title: "Status").Write("bmp");
            //this is setting for graph

            return null;
        }
    }
}