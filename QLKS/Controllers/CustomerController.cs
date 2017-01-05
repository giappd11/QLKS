using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLKS.Models;
using System.Web.Script.Serialization;

namespace QLKS.Controllers
{
    public class CustomerController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Customer/

        public ActionResult Index()
        {
            var customers = db.Customers.Include(c => c.Nationality);
            return View(customers.ToList());
        }
        public JsonResult SearchAddJson(int id)
        {
            if (id != null)
            {
                var result = from u in db.Customers
                             where u.CustomerID == id
                             select new CustomerView()
                             {
                                 Name = u.Name,
                                 IDCard = u.IDCard,
                                 CustomerID = u.CustomerID,
                                 NationalityID = u.NationalityID,
                                 Sex = u.Sex,
                                 Mail = u.Mail,
                                 Phone = u.Phone,
                                 Company = u.Company,
                                 Address = u.Address


                             };
                // Customer cus = db.Customers.Where(u => u.CustomerID == id).FirstOrDefault();
                //cus.checkin_Custommer = null;
                var json = new JavaScriptSerializer().Serialize(result);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            else return null;
        }

        public string SearchAdd(string ids, int index)
        {
            string html = "";
            if (ids != "")
            {
                string[] idss = ids.Split(',');
                for (int i = 0; i < idss.Length - 1; i++)
                {
                    int id = int.Parse(idss[i]);
                    Customer cus = db.Customers.Where(u => u.CustomerID == id).FirstOrDefault();

                    html += "<tr><td>";
                    html += "<input class = 'CustomerList" + index + "_IDCard' name = 'CustomerList[" + index + "].IDCard' value = '" + cus.IDCard + "' /></td>";
                    html += "<td><input class = 'CustomerList" + index + "_Name'  name = 'CustomerList[" + index + "].Name' value = '" + cus.Name + "' /></td>";
                    html += "<td><input class ='CustomerList" + index + "_Phone'  name = 'CustomerList[" + index + "].Phone' value = '" + cus.Phone + "' /></td>";
                    html += "<td><input class ='CustomerList" + index + "_Mail'  name = 'CustomerList[" + index + "].Mail' value = '" + cus.Mail + "' /></td>";
                    html += "<td><input class = 'CustomerList" + index + "_Address' name = 'CustomerList[" + index + "].Address' value = '" + cus.Address + "' /></td>";
                    html += "<td><input class = 'CustomerList" + index + "_Sex'  name = 'CustomerList[" + index + "].Sex' value = '" + cus.Sex.ToString().ToLower() + "' /></td>";
                    html += "<td><input class= 'CustomerList" + index + "_NationalityID' name = 'CustomerList[" + index + "].NationalityID' value = '" + cus.NationalityID + "' /></td>";
                    html += "<td><input class =  'CustomerList" + index + "_Company'  name = 'CustomerList[" + index + "].Company' value = '" + cus.Company + "' /></td>";
                    html += "<td><input class =  'CustomerList" + index + "_CustomerID'  name = 'CustomerList[" + index + "].CustomerID' value = '" + cus.CustomerID + "' />";
                    html += "</td></tr>";
                    index++;

                }

            }
            return html;
        }
         
        public string SearchAddShow(string ids, int index)
        {
            string html = "";
            if (ids != "")
            {
                string[] idss = ids.Split(',');
                for (int i = 0; i < idss.Length - 1; i++)
                {
                    int id = int.Parse(idss[i]);
                    Customer cus = db.Customers.Where(u => u.CustomerID == id).FirstOrDefault();
                    html += "<tr>";
                    html += "<td><a href = 'javascript:showDialogEdit(" + index + ")' >" + cus.Name + "</a></td>";
                    html += "<td>" + cus.Phone + "</td>";
                    html += "<td>" + cus.IDCard + "</td>";
                    //html += "<td>" + cus.Mail + "</td>";
                    html += "<td><input name = 'selectedCus' value = " + index + " type ='radio'/></td>";
                    html += "<td><a href = '#' onclick ='javascript:removeCus(" + index + ");' >xóa</a></td>";
                    index++;

                }

            }
            return html;
        }




        public ActionResult Search(string name, string phone, string mail, string cmnd, int? showOne)
        {
            ViewBag.showOne = showOne;
            var customers = db.Customers.Where(u => u.Name.Contains(name) && u.Phone.Contains(phone) && u.Mail.Contains(mail) && u.IDCard.Contains(cmnd));
            return View(customers.ToList());
        }
        //
        // GET: /Customer/Details/5

        public ActionResult Details(int id = 0)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //
        // GET: /Customer/Create

        public ActionResult Create()
        {
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName");
            return View();
        }

        //
        // POST: /Customer/Create

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName", customer.NationalityID);
            return View(customer);
        }

        //
        // GET: /Customer/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName", customer.NationalityID);
            return View(customer);
        }

        //
        // POST: /Customer/Edit/5

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName", customer.NationalityID);
            return View(customer);
        }

        //
        // GET: /Customer/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //
        // POST: /Customer/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}