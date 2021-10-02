using MHRS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MHRS.Controllers
{
    public class FarzanaDassController : Controller
    {
        PKPMentalHealthRecoveryEntities db = new PKPMentalHealthRecoveryEntities();


        public ActionResult DassForm(string studentid, string fromSess)
        {
            if(fromSess == "true")
                Session["fromsession"] = fromSess;

            Session["studentid"] = studentid;
            ViewBag.Message = "First. You Need To Fill The DASS Form.";

            return View();
        }

        [HttpPost]
        public JsonResult DassForm_Post(List<Dass_Form> data) //create new list model base on table name
        {
            int totalStress = 0;
            int totalAnxiety = 0;
            int totalDepress = 0;
            string condition = " ";

            Question_DassForm value = new Question_DassForm();


            foreach (Dass_Form var in data)
            {
                if (var.Question == "1")
                {
                    value.Q1_Value = var.Answer;
                }

                if (var.Question == "2")
                {
                    value.Q2_Value = var.Answer;
                }

                if (var.Question == "3")
                {
                    value.Q3_Value = var.Answer;
                }

                if (var.Question == "4")
                {
                    value.Q4_Value = var.Answer;
                }

                if (var.Question == "5")
                {
                    value.Q5_Value = var.Answer;
                }

                if (var.Question == "6")
                {
                    value.Q6_Value = var.Answer;
                }

                if (var.Question == "7")
                {
                    value.Q7_Value = var.Answer;
                }

                if (var.Question == "8")
                {
                    value.Q8_Value = var.Answer;
                }

                if (var.Question == "9")
                {
                    value.Q9_Value = var.Answer;
                }

                if (var.Question == "10")
                {
                    value.Q10_Value = var.Answer;
                }

                if (var.Question == "11")
                {
                    value.Q11_Value = var.Answer;
                }

                if (var.Question == "12")
                {
                    value.Q12_Value = var.Answer;
                }

                if (var.Question == "13")
                {
                    value.Q13_Value = var.Answer;
                }

                if (var.Question == "14")
                {
                    value.Q14_Value = var.Answer;
                }

                if (var.Question == "15")
                {
                    value.Q15_Value = var.Answer;
                }

                if (var.Question == "16")
                {
                    value.Q16_Value = var.Answer;
                }

                if (var.Question == "17")
                {
                    value.Q17_Value = var.Answer;
                }

                if (var.Question == "18")
                {
                    value.Q18_Value = var.Answer;
                }

                if (var.Question == "19")
                {
                    value.Q19_Value = var.Answer;
                }

                if (var.Question == "20")
                {
                    value.Q20_Value = var.Answer;
                }

                if (var.Question == "21")
                {
                    value.Q21_Value = var.Answer;
                }




                //stress
                if (var.Question == "1" || var.Question == "6" || var.Question == "8" || var.Question == "11" || var.Question == "12" || var.Question == "14" || var.Question == "18")
                {
                    totalStress = totalStress + var.Answer;
                }

                //anxiety
                if (var.Question == "2" || var.Question == "4" || var.Question == "7" || var.Question == "9" || var.Question == "15" || var.Question == "19" || var.Question == "20")
                {
                    totalAnxiety = totalAnxiety + var.Answer;
                }

                //depress
                if (var.Question == "3" || var.Question == "5" || var.Question == "10" || var.Question == "13" || var.Question == "16" || var.Question == "17" || var.Question == "21")
                {
                    totalDepress = totalDepress + var.Answer;
                }
            }

            //insert database
            if (totalStress > totalAnxiety && totalStress > totalDepress)
            {
                condition = "Stress";
            }

            if (totalAnxiety > totalStress && totalAnxiety > totalDepress)
            {
                condition = "Anxiety";
            }

            if (totalDepress > totalStress && totalDepress > totalAnxiety)
            {
                condition = "Depression";
            }

            if (totalStress < 5 && totalAnxiety < 4 && totalDepress < 7)
            {
                condition = "Normal";
            }


            string studid = Session["studentid"].ToString();
            DassForm dassform = new DassForm
            {
                Student_ID = studid,
                Result_Stress = totalStress,
                Result_Anxiety = totalAnxiety,
                Result_Depression = totalDepress,
                Condition = condition,
                Date = DateTime.Now
            };

            db.DassForms.Add(dassform);
            int i = db.SaveChanges();
            if (i > 0)
            {
                DassForm id = db.DassForms.OrderByDescending(x => x.Date).FirstOrDefault(x => x.Student_ID == studid);
                value.Form_ID = id.Form_ID;
                value.Student_ID = id.Student_ID;
                value.Date = id.Date;

                db.Question_DassForm.Add(value);
                db.SaveChanges();
            }

            string uid = Session["studentid"].ToString();

            string fromSession = "";
            if (!string.IsNullOrEmpty(Session["fromsession"] as string))
            {
                fromSession = "Yes";
            }

            return Json(new { redirectToUrl = Url.Action("studenthomepage", "HaoKeatHome", new { id = uid }) , inSession = fromSession });

        }

        public ActionResult GenerateReport(string id)
        {
            Session["staffid"] = id;

            var err = db.Sessions.Where(x => x.Staff_ID == id).ToList();
            List<DassForm> dasss = new List<DassForm>();

            foreach(var i in err)
            {
                List<DassForm> s = db.DassForms.Where(x => x.Student_ID == i.Student_ID).OrderByDescending(x=>x.Date).ToList();
                
                if(s != null)
                    dasss.AddRange(s);
            }

            return View(dasss);
        }

        //Pie Chart for Dass Form
        public ActionResult GetData()
        {
            string staffid = Session["staffid"].ToString();

            var err = db.Sessions.Where(x => x.Staff_ID == staffid).ToList();
            List<DassForm> stressdass = new List<DassForm>();
            List<DassForm> anxietydass = new List<DassForm>();
            List<DassForm> depressdass = new List<DassForm>();
            List<DassForm> normaldass = new List<DassForm>();

            foreach (var i in err)
            {
                List<DassForm> st = db.DassForms.Where(x => x.Student_ID == i.Student_ID && x.Condition == "Stress").ToList();
                List<DassForm> an = db.DassForms.Where(x => x.Student_ID == i.Student_ID && x.Condition == "Anxiety").ToList();
                List<DassForm> dp = db.DassForms.Where(x => x.Student_ID == i.Student_ID && x.Condition == "Depression").ToList();
                List<DassForm> nn = db.DassForms.Where(x => x.Student_ID == i.Student_ID && x.Condition == "Normal").ToList();

                if (st != null)
                    stressdass.AddRange(st);

                if (an != null)
                    anxietydass.AddRange(an);

                if (dp != null)
                    depressdass.AddRange(dp);

                if (nn != null)
                    normaldass.AddRange(nn);
            }

            int Stress = stressdass.Count;
            int Anxiety = anxietydass.Count;
            int Depression = depressdass.Count;
            int Normal = normaldass.Count;

            ratio obj = new ratio();
            
            obj.Stress = Stress;
            obj.Anxiety = Anxiety;
            obj.Depression = Depression;
            obj.Normal = Normal;

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public class ratio
        {
            public int Stress { get; set; }
            public int Anxiety { get; set; }
            public int Depression { get; set; }

            public int Normal { get; set; }
        }


        public ActionResult DetailsGenerateReport(string id)
        {
            long err = long.Parse(id);
            Question_DassForm d = db.Question_DassForm.FirstOrDefault(x => x.Form_ID == err);
            return View(d);
        }

        //Bar Graph for Dass Form
        public ActionResult DataPoint()
        {
            List<Data_Point> dataPoints = new List<Data_Point>();

            int Stress = db.DassForms.Where(x => x.Condition == "Stress").Count();
            int Anxiety = db.DassForms.Where(x => x.Condition == "Anxiety").Count();
            int Depression = db.DassForms.Where(x => x.Condition == "Depression").Count();
            int Normal = db.DassForms.Where(x => x.Condition == "Normal").Count();

            dataPoints.Add(new Data_Point("Stress", Stress));
            dataPoints.Add(new Data_Point("Anxiety", Anxiety));
            dataPoints.Add(new Data_Point("Depression", Depression));
            dataPoints.Add(new Data_Point("Normal", Normal));


            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            ViewBag.DataPoint = dataPoints;
            ViewBag.DP = db.DassForms.GroupBy(x => x.Date.Year).Select(y => y.FirstOrDefault()).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult DataPoint(string month, string year)
        {
            if (!string.IsNullOrWhiteSpace(month) && !string.IsNullOrWhiteSpace(year))

            {
                int k = Convert.ToInt32(year);
                List<Data_Point> dataPoints = new List<Data_Point>();
                for (int j = 1; j < 13; j++)
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(j);

                    if (month == monthName)
                    {

                        int Stress = db.DassForms.Where(x => x.Condition == "Stress" && x.Date.Month == j && x.Date.Year == k).Count();
                        int Anxiety = db.DassForms.Where(x => x.Condition == "Anxiety" && x.Date.Month == j && x.Date.Year == k).Count();
                        int Depression = db.DassForms.Where(x => x.Condition == "Depression" && x.Date.Month == j && x.Date.Year == k).Count();
                        int Normal = db.DassForms.Where(x => x.Condition == "Normal" && x.Date.Month == j && x.Date.Year == k).Count();


                        dataPoints.Add(new Data_Point("Stress", Stress));
                        dataPoints.Add(new Data_Point("Anxiety", Anxiety));
                        dataPoints.Add(new Data_Point("Depression", Depression));
                        dataPoints.Add(new Data_Point("Normal", Depression));
                    }
                }


                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
                ViewBag.DataPoint = dataPoints;
                ViewBag.DP = db.DassForms.GroupBy(x => x.Date.Year).Select(y => y.FirstOrDefault()).ToList();
            }

            if (string.IsNullOrWhiteSpace(month) && !string.IsNullOrWhiteSpace(year))
            {
                int k = Convert.ToInt32(year);
                List<Data_Point> dataPoints = new List<Data_Point>();
                int Stress = db.DassForms.Where(x => x.Condition == "Stress" && x.Date.Year == k).Count();
                int Anxiety = db.DassForms.Where(x => x.Condition == "Anxiety" && x.Date.Year == k).Count();
                int Depression = db.DassForms.Where(x => x.Condition == "Depression" && x.Date.Year == k).Count();
                int Normal = db.DassForms.Where(x => x.Condition == "Normal" && x.Date.Year == k).Count();


                dataPoints.Add(new Data_Point("Stress", Stress));
                dataPoints.Add(new Data_Point("Anxiety", Anxiety));
                dataPoints.Add(new Data_Point("Depression", Depression));
                dataPoints.Add(new Data_Point("Normal", Depression));

                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
                ViewBag.DataPoint = dataPoints;
                ViewBag.DP = db.DassForms.GroupBy(x => x.Date.Year).Select(y => y.FirstOrDefault()).ToList();
            }



            if (!string.IsNullOrWhiteSpace(month) && string.IsNullOrWhiteSpace(year))
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
                        int Normal = db.DassForms.Where(x => x.Condition == "Normal" && x.Date.Month == j).Count();


                        dataPoints.Add(new Data_Point("Stress", Stress));
                        dataPoints.Add(new Data_Point("Anxiety", Anxiety));
                        dataPoints.Add(new Data_Point("Depression", Depression));
                        dataPoints.Add(new Data_Point("Normal", Depression));
                    }
                }


                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
                ViewBag.DataPoint = dataPoints;
                ViewBag.DP = db.DassForms.GroupBy(x => x.Date.Year).Select(y => y.FirstOrDefault()).ToList();
            }

            if (string.IsNullOrWhiteSpace(month) && string.IsNullOrWhiteSpace(year))
            {
                List<Data_Point> dataPoints = new List<Data_Point>();

                int Stress = db.DassForms.Where(x => x.Condition == "Stress").Count();
                int Anxiety = db.DassForms.Where(x => x.Condition == "Anxiety").Count();
                int Depression = db.DassForms.Where(x => x.Condition == "Depression").Count();
                int Normal = db.DassForms.Where(x => x.Condition == "Normal").Count();

                dataPoints.Add(new Data_Point("Stress", Stress));
                dataPoints.Add(new Data_Point("Anxiety", Anxiety));
                dataPoints.Add(new Data_Point("Depression", Depression));
                dataPoints.Add(new Data_Point("Normal", Normal));


                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
                ViewBag.DataPoint = dataPoints;
                ViewBag.DP = db.DassForms.GroupBy(x => x.Date.Year).Select(y => y.FirstOrDefault()).ToList();
            }

            ViewBag.DP = db.DassForms.GroupBy(x => x.Date.Year).Select(y => y.FirstOrDefault()).ToList();
            return View();
        }

        //bar chart by session with date and month
        public ActionResult Point(string id)
        {
            var err = db.Sessions.Where(x => x.Staff_ID == id).ToList();
            List<DassForm> stressdass = new List<DassForm>();
            List<DassForm> anxietydass = new List<DassForm>();
            List<DassForm> depressdass = new List<DassForm>();
            List<DassForm> normaldass = new List<DassForm>();

            foreach (var i in err)
            {
                List<DassForm> st = db.DassForms.Where(x => x.Student_ID == i.Student_ID && x.Condition == "Stress").ToList();
                List<DassForm> an = db.DassForms.Where(x => x.Student_ID == i.Student_ID && x.Condition == "Anxiety").ToList();
                List<DassForm> dp = db.DassForms.Where(x => x.Student_ID == i.Student_ID && x.Condition == "Depression").ToList();
                List<DassForm> nn = db.DassForms.Where(x => x.Student_ID == i.Student_ID && x.Condition == "Normal").ToList();

                if (st != null)
                    stressdass.AddRange(st);

                if (an != null)
                    anxietydass.AddRange(an);

                if (dp != null)
                    depressdass.AddRange(dp);

                if (nn != null)
                    normaldass.AddRange(nn);
            }

            List<P_oint> points = new List<P_oint>();

            int Stress = stressdass.Count;
            int Anxiety = anxietydass.Count;
            int Depression = depressdass.Count;
            int Normal = normaldass.Count;

            points.Add(new P_oint("Stress", Stress));
            points.Add(new P_oint("Anxiety", Anxiety));
            points.Add(new P_oint("Depression", Depression));
            points.Add(new P_oint("Normal", Normal));


            ViewBag.Points = JsonConvert.SerializeObject(points);
            ViewBag.P = db.DassForms.GroupBy(x => x.Date.Year).Select(y => y.FirstOrDefault()).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Point(string month, string year)
        {
            if (!string.IsNullOrWhiteSpace(month))
            {
                List<P_oint> points = new List<P_oint>();
                for (int j = 1; j < 13; j++)
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(j);

                    if (month == monthName)
                    {
                        string id = Session["staffid"].ToString();
                        var err = db.Sessions.Where(x => x.Staff_ID == id).ToList();
                        List<DassForm> stressdass = new List<DassForm>();
                        List<DassForm> anxietydass = new List<DassForm>();
                        List<DassForm> depressdass = new List<DassForm>();
                        List<DassForm> normaldass = new List<DassForm>();

                        foreach (var i in err)
                        {
                            List<DassForm> st = db.DassForms.Where(x => x.Student_ID == i.Student_ID && x.Condition == "Stress" && x.Date.Month == j).ToList();
                            List<DassForm> an = db.DassForms.Where(x => x.Student_ID == i.Student_ID && x.Condition == "Anxiety" && x.Date.Month == j).ToList();
                            List<DassForm> dp = db.DassForms.Where(x => x.Student_ID == i.Student_ID && x.Condition == "Depression" && x.Date.Month == j).ToList();
                            List<DassForm> nn = db.DassForms.Where(x => x.Student_ID == i.Student_ID && x.Condition == "Normal" && x.Date.Month == j).ToList();

                            if (st != null)
                                stressdass.AddRange(st);

                            if (an != null)
                                anxietydass.AddRange(an);

                            if (dp != null)
                                depressdass.AddRange(dp);

                            if (nn != null)
                                normaldass.AddRange(nn);
                        }

                        int Stress = stressdass.Count;
                        int Anxiety = anxietydass.Count;
                        int Depression = depressdass.Count;
                        int Normal = normaldass.Count;



                        points.Add(new P_oint("Stress", Stress));
                        points.Add(new P_oint("Anxiety", Anxiety));
                        points.Add(new P_oint("Depression", Depression));
                        points.Add(new P_oint("Normal", Normal));
                    }
                }

                ViewBag.Points = JsonConvert.SerializeObject(points);
                ViewBag.P = db.DassForms.GroupBy(x => x.Date.Year).Select(y => y.FirstOrDefault()).ToList();
            }

            else
            {
                string id = Session["staffid"].ToString();
                var err = db.Sessions.Where(x => x.Staff_ID == id).ToList();
                List<DassForm> stressdass = new List<DassForm>();
                List<DassForm> anxietydass = new List<DassForm>();
                List<DassForm> depressdass = new List<DassForm>();
                List<DassForm> normaldass = new List<DassForm>();

                foreach (var i in err)
                {
                    List<DassForm> st = db.DassForms.Where(x => x.Student_ID == i.Student_ID && x.Condition == "Stress").ToList();
                    List<DassForm> an = db.DassForms.Where(x => x.Student_ID == i.Student_ID && x.Condition == "Anxiety").ToList();
                    List<DassForm> dp = db.DassForms.Where(x => x.Student_ID == i.Student_ID && x.Condition == "Depression").ToList();
                    List<DassForm> nn = db.DassForms.Where(x => x.Student_ID == i.Student_ID && x.Condition == "Normal").ToList();

                    if (st != null)
                        stressdass.AddRange(st);

                    if (an != null)
                        anxietydass.AddRange(an);

                    if (dp != null)
                        depressdass.AddRange(dp);

                    if (nn != null)
                        normaldass.AddRange(nn);
                }

                List<P_oint> points = new List<P_oint>();

                int Stress = stressdass.Count;
                int Anxiety = anxietydass.Count;
                int Depression = depressdass.Count;
                int Normal = normaldass.Count;

                points.Add(new P_oint("Stress", Stress));
                points.Add(new P_oint("Anxiety", Anxiety));
                points.Add(new P_oint("Depression", Depression));
                points.Add(new P_oint("Normal", Normal));


                ViewBag.Points = JsonConvert.SerializeObject(points);
                ViewBag.P = db.DassForms.GroupBy(x => x.Date.Year).Select(y => y.FirstOrDefault()).ToList();
            }


            return View();
        }

        public ActionResult ResultDass(string id)
        {
            List<DassForm> h = db.DassForms.Where(x => x.Student_ID == id).OrderByDescending(x=>x.Date).ToList();
            return View(h);
        }
    }
}