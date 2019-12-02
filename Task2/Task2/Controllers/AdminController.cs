using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task2.Models;

namespace Task2.Controllers
{
    public class AdminController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        [HttpGet]
        public ActionResult Products(string search = "", double min = 0, double max = 0)
        {
            if (Session["Role"] == null || (string)Session["Role"] != "Admin")
                return new HttpStatusCodeResult(403);

            List<Product> products = db.Products.Where(x => x.Name.Substring(0, search.Length).ToLower() == search.ToLower()).ToList();
            if (min > 0)
                products = products.Where(x => x.Price >= min).ToList();
            if (max > 0)
                products = products.Where(x => x.Price <= max).ToList();

            ViewBag.Products = products.OrderBy(x => x.Name).ToList();

            ViewBag.Search = search;
            ViewBag.Min = min;
            ViewBag.Max = max;

            return View();
        }
        [HttpGet]
        public ActionResult Orders()
        {
            if (Session["Role"] == null || (string)Session["Role"] != "Admin")
                return new HttpStatusCodeResult(403);

            ViewBag.Products = db.Products.ToList();
            ViewBag.Orders = db.Orders.OrderBy(x => x.OrderTime).ToList();
            ViewBag.Users = db.Users.ToList();

            return View();
        }
        [HttpGet]
        public ActionResult Users()
        {
            if (Session["Role"] == null || (string)Session["Role"] != "Admin")
                return new HttpStatusCodeResult(403);

            ViewBag.Users = db.Users.ToList();

            return View();
        }
        [HttpGet]
        public ActionResult MakeAdmin(int userId = -1)
        {
            if (Session["Role"] == null || (string)Session["Role"] != "Admin")
                return new HttpStatusCodeResult(403);

            if ((int)Session["Id"] == userId)
                return new HttpStatusCodeResult(400);

            User user = db.Users.Where(x => x.Id == userId).FirstOrDefault();
            if (user == null)
                return new HttpStatusCodeResult(404);

            user.Role = "Admin";
            db.SaveChanges();

            return RedirectToAction("Users");
        }
        [HttpGet]
        public ActionResult MakeUser(int userId = -1)
        {
            if (Session["Role"] == null || (string)Session["Role"] != "Admin")
                return new HttpStatusCodeResult(403);

            if ((int)Session["Id"] == userId)
                return new HttpStatusCodeResult(400);

            User user = db.Users.Where(x => x.Id == userId).FirstOrDefault();
            if (user == null)
                return new HttpStatusCodeResult(404);

            user.Role = "User";
            db.SaveChanges();

            return RedirectToAction("Users");
        }
        #region Add_Product
        [HttpGet]
        public ActionResult AddProduct()
        {
            if (Session["Role"] == null || (string)Session["Role"] != "Admin")
                return new HttpStatusCodeResult(403);

            ViewBag.Errors = new string[] { };
            return View(new Product());
        }

        [HttpPost]
        public ActionResult AddProduct([Bind(Include = "Name, Price, StorageCount")] Product product)
        {
            if (Session["Role"] == null || (string)Session["Role"] != "Admin")
                return new HttpStatusCodeResult(403);

            List<string> errors = new List<string>();

            if (!string.IsNullOrWhiteSpace(product.Name))
            {
                if (product.Price > 0) {
                    if (product.StorageCount >= 0)
                    {
                        db.Products.Add(product);
                        db.SaveChanges();

                        return RedirectToAction("Products");
                    }
                    else
                    {
                        errors.Add("Storage count need to be 0 or more");
                    }
                }
                else
                {
                    errors.Add("Price is need to be more than 0");
                }
            }
            else
            {
                errors.Add("Name is required");
            }

            ViewBag.Errors = errors.ToArray();
            return View(product);
        }
        #endregion //ADD PRODUCT
        #region Edit_Product
        [HttpGet]
        public ActionResult EditProduct(int id = -1)
        {
            if (Session["Role"] == null || (string)Session["Role"] != "Admin")
                return new HttpStatusCodeResult(403);
            Product dbProduct = db.Products.Where(x => x.Id == id).FirstOrDefault();
            if (dbProduct == null)
                return new HttpStatusCodeResult(404);

            ViewBag.Errors = new string[] { };
            return View(dbProduct);
        }
        [HttpPost]
        public ActionResult EditProduct([Bind(Include = "Id, Name, Price, StorageCount")] Product product) {
            if (Session["Role"] == null || (string)Session["Role"] != "Admin")
                return new HttpStatusCodeResult(403);

            List<string> errors = new List<string>();
            Product dbProduct = db.Products.Where(x => x.Id == product.Id).FirstOrDefault();
            if (dbProduct != null)
            {
                if (!string.IsNullOrWhiteSpace(product.Name))
                {
                    if (product.Price > 0)
                    {
                        if (product.StorageCount >= 0)
                        {
                            dbProduct.Name = product.Name;
                            dbProduct.Price = product.Price;
                            dbProduct.StorageCount = product.StorageCount;

                            db.SaveChanges();

                            return RedirectToAction("Products");
                        }
                        else
                        {
                            errors.Add("Storage count is need to be 0 or more");
                        }
                    }
                    else
                    {
                        errors.Add("Price is need to be more than 0");
                    }
                }
                else
                {
                    errors.Add("Name is required");
                }
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }

            ViewBag.Errors = errors.ToArray();
            return View(product);
        }
        #endregion //EDIT PRODUCT
        #region Delete_Product
        [HttpGet]
        public ActionResult DeleteProduct(int id = -1)
        {
            if (Session["Role"] == null || (string)Session["Role"] != "Admin")
                return new HttpStatusCodeResult(403);

            if (id == -1)
                return new HttpStatusCodeResult(400);

            Product dbProduct = db.Products.Where(x => x.Id == id).FirstOrDefault();
            if (dbProduct == null)
                return new HttpStatusCodeResult(404);

            return View(dbProduct);
        }

        [HttpPost]
        public ActionResult DeleteProduct([Bind(Include = "Id")] Product product)
        {
            if (Session["Role"] == null || (string)Session["Role"] != "Admin")
                return new HttpStatusCodeResult(403);

            Product dbProduct = db.Products.Where(x => x.Id == product.Id).FirstOrDefault();
            if (dbProduct == null)
                return new HttpStatusCodeResult(404);

            db.Products.Remove(dbProduct);
            db.SaveChanges();

            return RedirectToAction("Products");
        }
        #endregion //DELETE PRODUCT
    }
}