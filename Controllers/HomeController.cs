using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Feedback.Models;
//Original

namespace Feedback.Controllers
{
    public class HomeController : Controller
    {
        feedbackSystemDxcEntities1 entities = new feedbackSystemDxcEntities1();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CustomerDetails()
        {



            return View();
        }
        [HttpPost]
        public ActionResult CustomerDetails(Customers c)
        {
            Customers cust = new Customers();
             
            if (ModelState.IsValid)
            {
                cust.Dov = c.Dov;
                cust.Organization = c.Organization;

                entities.Customers.Add(cust);
                entities.SaveChanges();

            }
            return RedirectToAction("FeedBack", new RouteValueDictionary(cust));


        }
        public ActionResult FeedBack(Customers c)
        {
           
            DateTimeFormatInfo format = new DateTimeFormatInfo();
            format.ShortDatePattern = "MM/dd/yyyy";
            format.DateSeparator = "/";

            DateTime shortDate = Convert.ToDateTime(c.Dov, format);


            ViewBag.Dovv = shortDate.ToShortDateString();
            ViewBag.Organizationn = c.Organization;
            ViewBag.Chapterss = new SelectList(entities.Chapters, "Cid", "Cname");
            ViewBag.Productss = new SelectList(entities.Products, "Pid", "Pname");


            return View();
        }
        [HttpPost]
        public ActionResult FeedBack(FormCollection fc, Customers c, Feedbacks fd)
        {
          
            //while posting the dropdown instances from get method should be passed to post so that if they are empty
            //        we can suppress exceptions related to dropdown
            //ViewBag.Chapterss = new SelectList(fb.Chapters, "Cid", "Cname");
            //ViewBag.Productss = new SelectList(fb.Products, "Pid", "Pname");


            Feedbacks feedback = new Feedbacks();
            feedback.Custid = c.Custid;
            feedback.Pid = Convert.ToInt32(fc[1]);
            feedback.First = fd.First;
            feedback.Second = fd.Second;
            feedback.Third = fd.Third;
            feedback.Fourth = fd.Fourth;
            feedback.Fifth = fd.Fifth;
            feedback.Sixth = fd.Sixth;
            feedback.Seventh = fd.Seventh;
            feedback.Addition = fc["Addition"];
            feedback.Deletion = fc["Deletion"];
            feedback.Comments = fc["Comments"];

            feedback.Avg = (fd.First + fd.Second + fd.Third + fd.Fourth + fd.Fifth + fd.Sixth + fd.Seventh) / 7;

            entities.Feedbacks.Add(feedback);
            entities.SaveChanges();
            return RedirectToAction("SuccessPage", new RouteValueDictionary(c));






        }
        public ActionResult SuccessPage(Customers c)
        {
            ViewBag.Organization = c.Organization;
            return View();
        }


        [HttpPost]
        public ActionResult Index(Chapters chapters)
        {
            Session["username"] = chapters.Cname;
            Session["cid"] = chapters.Cid;
            List<Chapters> chap = entities.Chapters.ToList();
            Chapters temp = chap.FirstOrDefault(x => x.Cname == chapters.Cname);
            //int cid = temp.Cid;
            if (temp != null)
            {
                if (temp.Cpassword.Equals(chapters.Cpassword))
                {
                    FormsAuthentication.SetAuthCookie(chapters.Cname, false);
                    @ViewBag.result = "Login Successful";
                    return RedirectToAction("Userpage",new { cid=temp.Cid});

                }
                else
                {
                    @ViewBag.result = "Wrong Password";
                    return View();

                }
            }
            else
            {
                @ViewBag.result = "Wrong Username and Password";
                return View();

            }
        }
        public ActionResult Logout()
        {
            
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult UserPage(int cid,string searchVal,string Sortby)
        {
            List<Feedbacks> fb = entities.Feedbacks.ToList();
            IEnumerable<Feedbacks> fb1 = fb.Where(x => x.Products.Cid == cid);
            if (!String.IsNullOrEmpty(searchVal))
            {
                fb1 = fb1.Where(x => x.Customers.Organization.ToLower().Contains(searchVal.ToLower()) | x.Products.Pname.ToLower().Contains(searchVal.ToLower()));
            }

            if (!String.IsNullOrEmpty(Sortby))
            {
                switch (int.Parse(Sortby))
                {
                    case 0: fb1 = fb1.OrderByDescending(x => x.Customers.Dov);break;
                    case 1: fb1 = fb1.OrderBy(x => x.Customers.Dov); break;
                    case 2: fb1 = fb1.OrderByDescending(x => x.Avg); break;
                    case 3: fb1 = fb1.OrderBy(x => x.Avg); break;
                    default: break;
                }
            }
            return View(fb1);
        }

        public ActionResult Details(int id) {
            Feedbacks fb = entities.Feedbacks.FirstOrDefault(x => x.Fid == id);
            Chapters temp = entities.Chapters.FirstOrDefault(x => x.Cid == fb.Products.Cid);
            return View(fb);
        }

    }
}
