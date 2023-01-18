using SMWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMWebApp.Controllers
{
    public class HomeController : Controller
    {
        db_SMEntities db = new db_SMEntities();
        // GET: Home
        public ActionResult Index()
        {
            var data = db.student_Tbl.ToList();
            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(student_Tbl s)
        {
            if (ModelState.IsValid == true)
            {
                string fileName = Path.GetFileNameWithoutExtension(s.ImageFile.FileName);
                string extension = Path.GetExtension(s.ImageFile.FileName);
                HttpPostedFileBase postedFile = s.ImageFile;
                int length = postedFile.ContentLength;

                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                {
                    if (length <= 1000000)
                    {
                        fileName = fileName + extension;
                        s.image_path = "~/Images/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        s.ImageFile.SaveAs(fileName);
                        db.student_Tbl.Add(s);
                        int a = db.SaveChanges();
                        if (a > 0)
                        {
                            TempData["CreateMessage"] = "<script>alert('Data Inserted Sucessfully')</script>";
                            ModelState.Clear();
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["CreateMessage"] = "<script>alert('Data Not Inserted')</script>";
                        }
                    }
                    else
                    {
                        TempData["SizeMessage"] = "<script>alert('Image size should be less then 1mb')</script>";
                    }
                }
                else
                {
                    TempData["ExtensionMessage"] = "<script>alert('Format Not Supported')</script>";
                }
            }
           

            return View();
        }
        public ActionResult Edit(int id)
        {
            var studentRow = db.student_Tbl.Where(x => x.id == id).FirstOrDefault();
            Session["Image"] = studentRow.image_path;
            return View(studentRow);
        }
        [HttpPost]
        public ActionResult Edit(student_Tbl s)
        {
            if (ModelState.IsValid == true)
            {

                if(s.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(s.ImageFile.FileName);
                    string extension = Path.GetExtension(s.ImageFile.FileName);
                    HttpPostedFileBase postedFile = s.ImageFile;
                    int length = postedFile.ContentLength;

                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (length <= 1000000)
                        {
                            fileName = fileName + extension;
                            s.image_path = "~/Images/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                            s.ImageFile.SaveAs(fileName);
                            db.Entry(s).State = EntityState.Modified;
                            int a = db.SaveChanges();
                            if (a > 0)
                            {
                                TempData["UpdateMessage"] = "<script>alert('Data Updated Sucessfully')</script>";
                                ModelState.Clear();
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["UpdateMessage"] = "<script>alert('Data Not Updated')</script>";
                            }
                        }
                        else
                        {
                            TempData["SizeMessage"] = "<script>alert('Image size should be less then 1mb')</script>";
                        }
                    }
                    else
                    {
                        TempData["ExtensionMessage"] = "<script>alert('Format Not Supported')</script>";
                    }
                }
                else
                {
                    s.image_path = Session["Image"].ToString();
                    db.Entry(s).State = EntityState.Modified;
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["UpdateMessage"] = "<script>alert('Data Updated Sucessfully')</script>";
                        ModelState.Clear();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["UpdateMessage"] = "<script>alert('Data Not Updated')</script>";
                    }
                }

            }
            return View();
        }
    }
}