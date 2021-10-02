using MHRS.Models;
using Newtonsoft.Json;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MHRS.Controllers
{
    public class HusnaAdminController : Controller
    {
        PKPMentalHealthRecoveryEntities db = new PKPMentalHealthRecoveryEntities();

        public ActionResult AdminHome()
        {
            TotalStaffStudent total = new TotalStaffStudent();

            total.staff_count = db.Staffs.Count();
            total.student_count = db.Students.Count();
            return View(total);
        }

        public ActionResult IndexStaff()
        {
            return View();
        }

        //staff management
        public ActionResult IndexStaffAdmin(string search, int? i)
        {

            return View(db.Staffs.Where(x => x.Fname.StartsWith(search) || search == null).ToList().ToPagedList(i ?? 1, 3));
        }


        //chart get data
        public ActionResult GetDataStaff()
        {
            int admin = db.Staffs.Where(x => x.Role_ID == 1).Count();
            int staff = db.Staffs.Where(x => x.Role_ID == 2).Count();

            RatioStaffRole obj = new RatioStaffRole();
            obj.Admin = admin;
            obj.Staff = staff;

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public class RatioStaffRole
        {
            public int Admin { get; set; }
            public int Staff { get; set; }
        }


        // GET: Admin/Details/5
        public ActionResult DetailsStaff(string id)
        {
            //PKPdb dbmodel = new PKPdb();
            if (id != null)
            {
                Staff staff = db.Staffs.Single(X => X.Staff_ID == id);

                return View(staff);
            }
            return View();


        }

        // GET: Admin/Create
        public ActionResult CreateStaff()
        {

            var division = new List<String>() { "BAHAGIAN PERKHIDMATAN PELAJAR", "PUSAT PENGURUSAN ALUMNI & KEBOLEHPASARAN GRADUAN", "BAHAGIAN SUMBER MANUSIA", "BAHAGIAN PERKHIDMATAN PELAJAR" };
            ViewBag.listDiv = division;

            var designation = new List<String>() { "PEGAWAI PSIKOLOGI" };
            ViewBag.listDes = designation;

            var role = new List<Role>()
            {
                new Role() {Id = 1, Roles = "Admin"},
                new Role() {Id = 2, Roles = "Staff"}
            };

            ViewBag.list = role;


            return View();
        }


        // POST: Admin/Create
        [HttpPost]
        public ActionResult CreateStaff(Staff staff)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Staffs.Add(staff);
                    db.SaveChanges();

                    TempData["msg"] = "<script>alert('Staff created');</script>";
                }
                return RedirectToAction("IndexStaffAdmin");

            }
            catch
            {
                var division = new List<String>() { "BAHAGIAN PERKHIDMATAN PELAJAR", "PUSAT PENGURUSAN ALUMNI & KEBOLEHPASARAN GRADUAN", "BAHAGIAN SUMBER MANUSIA", "BAHAGIAN PERKHIDMATAN PELAJAR" };
                ViewBag.listDiv = division;

                var designation = new List<String>() { "PEGAWAI PSIKOLOGI" };
                ViewBag.listDes = designation;

                var role = new List<Role>()
                {
                    new Role() {Id = 1, Roles = "Admin"},
                    new Role() {Id = 2, Roles = "Staff"}
                };

                TempData["msg"] = "<script>alert('Staff not created');</script>";


                ViewBag.list = role;
                return View();
            }
        }

        class Role
        {
            public int Id { get; set; }
            public String Roles { get; set; }
        }


        // GET: Admin/Edit/5
        public ActionResult EditStaff(string id)
        {
            if (id != null)
            {
                Staff staff = db.Staffs.Single(X => X.Staff_ID == id);
                var division = new List<String>() { "BAHAGIAN PERKHIDMATAN PELAJAR", "PUSAT PENGURUSAN ALUMNI & KEBOLEHPASARAN GRADUAN", "BAHAGIAN SUMBER MANUSIA", "BAHAGIAN PERKHIDMATAN PELAJAR" };
                ViewBag.listDiv = division;

                var designation = new List<String>() { "PEGAWAI PSIKOLOGI" };
                ViewBag.listDes = designation;

                var role = new List<Role>()
                {
                    new Role() {Id = 1, Roles = "Admin"},
                    new Role() {Id = 2, Roles = "Staff"}
                };

                ViewBag.list = role;



                return View(staff);
            }
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult EditStaff(string id, Staff model)
        {

            using (var context = new PKPMentalHealthRecoveryEntities())
            {

                var data = context.Staffs.FirstOrDefault(x => x.Staff_ID == id);

                // Checking if any such record exist  
                if (data != null)
                {
                    data.Fname = model.Fname;
                    data.Lname = model.Lname;
                    data.Designation = model.Designation;
                    data.Division = model.Division;
                    data.Unit = model.Unit;
                    data.Phone_No = model.Phone_No;
                    data.Email = model.Email;
                    data.Password = model.Password;
                    data.Role_ID = model.Role_ID;

                    var role = new List<Role>()
                    {
                        new Role() {Id = 1, Roles = "Admin"},
                        new Role() {Id = 2, Roles = "Staff"}
                    };

                    ViewBag.list = role;

                    var division = new List<String>() { "BAHAGIAN PERKHIDMATAN PELAJAR", "PUSAT PENGURUSAN ALUMNI & KEBOLEHPASARAN GRADUAN", "BAHAGIAN SUMBER MANUSIA", "BAHAGIAN PERKHIDMATAN PELAJAR" };
                    ViewBag.listDiv = division;

                    var designation = new List<String>() { "PEGAWAI PSIKOLOGI" };
                    ViewBag.listDes = designation;

                    context.SaveChanges();

                    TempData["update"] = "<script>alert('Staff Updated');</script>";

                    // It will redirect to  
                    return RedirectToAction("IndexStaffAdmin");
                }
                else
                    return View();
            }
        }


        // GET: Admin/Delete/5
        public ActionResult DeleteStaff(string id)
        {
            return View(db.Staffs.Where(x => x.Staff_ID == id).FirstOrDefault());
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult DeleteStaff(string id, Staff staff)
        {
            try
            {
                staff = db.Staffs.Where(x => x.Staff_ID == id).FirstOrDefault();
                db.Staffs.Remove(staff);
                db.SaveChanges();

                TempData["delete"] = "<script>alert('Staff Deleted');</script>";

                return RedirectToAction("IndexStaffAdmin");
            }
            catch
            {
                return View();
            }
        }

        //-----------------------------------------------STUDENT-----------------------------------------------------------

        public ActionResult IndexStudent(string search)
        {
            if (search == "All")
            {
                return View(db.Students);

            }
            else
            {
                return View(db.Students.Where(x => x.Faculty.StartsWith(search) || search == null).ToList());
            }
            //return View(db.Students.Where(x => x.Fname.StartsWith(search) || search == null).ToList());
        }

        //chart get data student
        public ActionResult GetDataStudent()
        {
            int ftmk = db.Students.Where(x => x.Faculty == "FTMK").Count();
            int fkekk = db.Students.Where(x => x.Faculty == "FKEKK").Count();
            int fke = db.Students.Where(x => x.Faculty == "FKE").Count();
            int fkp = db.Students.Where(x => x.Faculty == "FKP").Count();
            int fkm = db.Students.Where(x => x.Faculty == "FKM").Count();
            int fptt = db.Students.Where(x => x.Faculty == "FPTT").Count();
            int ftk = db.Students.Where(x => x.Faculty == "FTK").Count();
            int ftkmp = db.Students.Where(x => x.Faculty == "FTKMP").Count();

            ratioFaculty fakulti = new ratioFaculty();
            fakulti.Ftmk = ftmk;
            fakulti.Fkekk = fkekk;
            fakulti.Fke = fke;
            fakulti.Fkp = fkp;
            fakulti.Fkm = fkm;
            fakulti.Fptt = fptt;
            fakulti.Ftk = ftk;
            fakulti.Ftkmp = ftkmp;

            return Json(fakulti, JsonRequestBehavior.AllowGet);
        }

        public class ratioFaculty
        {
            public int Ftmk { get; set; }
            public int Fkekk { get; set; }
            public int Fke { get; set; }
            public int Fkp { get; set; }
            public int Fkm { get; set; }
            public int Fptt { get; set; }
            public int Ftk { get; set; }
            public int Ftkmp { get; set; }
        }

        public ActionResult IndexStudentAdmin(string search, int? i)
        {
            return View(db.Students.Where(x => x.Fname.StartsWith(search) || search == null).ToList().ToPagedList(i ?? 1, 3));
        }


        // GET: Admin/Details/5
        public ActionResult DetailsStudent(string id)
        {
            if (id != null)
            {
                Student stu = db.Students.Single(X => X.Student_ID == id);

                return View(stu);
            }
            return View();
        }

        // GET: Admin/Create
        public ActionResult CreateStudent()
        {

            //var listGender = new List<String>() {"M", "F"};
            //ViewBag.list = listGender;            

            var listFaculty = new List<String>() { "FTMK", "FKEKK", "FKE", "FKP", "FKM", "FPTT", "FTKEE", "FTKMP" };
            ViewBag.listFaculty = listFaculty;

            var listYear = new List<int>() { 1, 2, 3 };
            ViewBag.listYear = listYear;

            var listSem = new List<int>() { 1, 2 };
            ViewBag.listSem = listSem;

            var roleStudent = new List<RoleStud>()
            {
                new RoleStud() {Stud_Id = 3, Role = "Student"},
            };
            ViewBag.list = roleStudent;


            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult CreateStudent(Student student)
        {
            try
            {

                db.Students.Add(student);
                db.SaveChanges();

                TempData["createStudent"] = "<script>alert('New student created');</script>";


                return RedirectToAction("IndexStudentAdmin");
            }
            catch
            {
                //var listGender = new List<String>() { "M", "F" };
                //ViewBag.list = listGender;

                var listFaculty = new List<String>() { "FTMK", "FKEKK", "FKE", "FKP", "FKM", "FPTT", "FTK" };
                ViewBag.listFaculty = listFaculty;

                var listYear = new List<int>() { 1, 2, 3 };
                ViewBag.listYear = listYear;

                var listSem = new List<int>() { 1, 2 };
                ViewBag.listSem = listSem;

                var roleStudent = new List<RoleStud>()
                 {
                    new RoleStud() {Stud_Id = 3, Role = "Student"},
                 };
                ViewBag.list = roleStudent;

                return View();
            }
        }

        class RoleStud
        {
            public int Stud_Id { get; set; }
            public string Role { get; set; }
        }


        // GET: Admin/Edit/5
        public ActionResult EditStudent(string id)
        {
            if (id != null)
            {
                Student stud = db.Students.Single(X => X.Student_ID == id);

                var list = new List<String>() { "FTMK", "FKEKK", "FKE", "FKP", "FKM", "FPTT", "FTK" };
                ViewBag.listFaculty = list;

                var listYear = new List<int>() { 1, 2, 3 };
                ViewBag.listYear = listYear;

                var listSem = new List<int>() { 1, 2 };
                ViewBag.listSem = listSem;

                return View(stud);
            }
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult EditStudent(string id, Student model)
        {
            using (var context = new PKPMentalHealthRecoveryEntities())
            {
                var data = context.Students.FirstOrDefault(x => x.Student_ID == id);

                // Checking if any such record exist  
                if (data != null)
                {
                    data.Fname = model.Fname;
                    data.Lname = model.Lname;
                    data.Faculty = model.Faculty;
                    data.Course = model.Course;
                    data.Year = model.Year;
                    data.Semester = model.Semester;
                    data.Phone_No = model.Phone_No;
                    data.Email = model.Email;
                    data.Password = model.Password;


                    var list = new List<String>() { "FTMK", "FKEKK", "FKE", "FKP", "FKM", "FPTT", "FTK" };
                    ViewBag.listFaculty = list;

                    var listYear = new List<int>() { 1, 2, 3 };
                    ViewBag.listYear = listYear;

                    var listSem = new List<int>() { 1, 2 };
                    ViewBag.listSem = listSem;

                    context.SaveChanges();

                    TempData["updateStudent"] = "<script>alert('Student updated');</script>";

                    return RedirectToAction("IndexStudentAdmin");
                }
                else
                {
                    return View();

                }
            }
        }

        // GET: Admin/Delete/5
        public ActionResult DeleteStudent(string id)
        {
            return View(db.Students.Where(x => x.Student_ID == id).FirstOrDefault());
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult DeleteStudent(string id, Student stu)
        {
            try
            {
                // TODO: Add delete logic here
                stu = db.Students.Where(x => x.Student_ID == id).FirstOrDefault();
                db.Students.Remove(stu);
                db.SaveChanges();

                TempData["deleteStudent"] = "<script>alert('Student Deleted');</script>";
                return RedirectToAction("IndexStudentAdmin");
            }
            catch
            {
                return View();
            }
        }

        //----------------------------------------DASS RESULT----------------------------------------
        public ActionResult dassResult(string search)
        {
            if (search == "All")
            {
                return View(db.DassForms);

            }
            else
            {
                return View(db.DassForms.Where(x => x.Condition.StartsWith(search) || search == null).ToList());
            }
        }

        //details dass
        public ActionResult DetailsDass(int id)
        {
            if (id != null)
            {
                DassForm dass = db.DassForms.Single(X => X.Form_ID == id);

                return View(dass);
            }
            return View();


        }

        //delete dass
        public ActionResult DeleteDass(int id)
        {
            return View(db.DassForms.Where(x => x.Form_ID == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult DeleteDass(int id, DassForm das)
        {
            try
            {
                das = db.DassForms.Where(x => x.Form_ID == id).FirstOrDefault();
                db.DassForms.Remove(das);
                db.SaveChanges();
                return RedirectToAction("DASSresult");
            }
            catch
            {
                return View();
            }
        }

        //chart dass report
        public ActionResult GetDataDASS()
        {
            int normal = db.DassForms.Where(x => x.Condition == "Normal").Count();
            int stress = db.DassForms.Where(x => x.Condition == "Stress").Count();
            int anxiety = db.DassForms.Where(x => x.Condition == "Anxiety").Count();
            int depression = db.DassForms.Where(x => x.Condition == "Depression").Count();


            RatioDASS obj = new RatioDASS();
            obj.Normal = normal;
            obj.Stress = stress;
            obj.Anxiety = anxiety;
            obj.Depression = depression;

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public class RatioDASS
        {
            public int Normal { get; set; }
            public int Stress { get; set; }
            public int Anxiety { get; set; }
            public int Depression { get; set; }
        }

        //Bar Graph for Dass Form
        public ActionResult DataPoint()
        {
            List<Data_Point> dataPoints = new List<Data_Point>();

            int Stress = db.DassForms.Where(x => x.Condition == "Stress").Count();
            int Anxiety = db.DassForms.Where(x => x.Condition == "Anxiety").Count();
            int Depression = db.DassForms.Where(x => x.Condition == "Depression").Count();

            dataPoints.Add(new Data_Point("Stress", Stress));
            dataPoints.Add(new Data_Point("Anxiety", Anxiety));
            dataPoints.Add(new Data_Point("Depression", Depression));


            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            ViewBag.DataPoint = dataPoints;
            return View();
        }

        [HttpPost]
        public ActionResult DataPoint(string month)
        {
            if (!string.IsNullOrWhiteSpace(month))
            {
                List<Data_Point> dataPoints = new List<Data_Point>();
                for (int j = 1; j < 13; j++)
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(j);

                    if (month == monthName)
                    {
                        int Stress = db.DassForms.Where(x => x.Condition == "Stress" && x.Date.Month == j).Count();
                        int Anxiety = db.DassForms.Where(x => x.Condition == "Anxiety" && x.Date.Month == j).Count();
                        int Depression = db.DassForms.Where(x => x.Condition == "Depression" && x.Date.Month == j).Count();

                        dataPoints.Add(new Data_Point("Stress", Stress));
                        dataPoints.Add(new Data_Point("Anxiety", Anxiety));
                        dataPoints.Add(new Data_Point("Depression", Depression));
                    }
                }


                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
                ViewBag.DataPoint = dataPoints;
            }

            else
            {
                List<Data_Point> dataPoints = new List<Data_Point>();

                int Stress = db.DassForms.Where(x => x.Condition == "Stress").Count();
                int Anxiety = db.DassForms.Where(x => x.Condition == "Anxiety").Count();
                int Depression = db.DassForms.Where(x => x.Condition == "Depression").Count();

                dataPoints.Add(new Data_Point("Stress", Stress));
                dataPoints.Add(new Data_Point("Anxiety", Anxiety));
                dataPoints.Add(new Data_Point("Depression", Depression));


                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
                ViewBag.DataPoint = dataPoints;
            }


            return View();
        }


        //logout
        public ActionResult Logout()
        {
            //redirect to staff login 
            return RedirectToAction("index", "HaoKeatHome");
        }
    }
}