using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task2.Models;

namespace Task2.Controllers
{
    public class ShopController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        //Список всех продуктов где кол-во продукта больше 0, а так же поиск
        [HttpGet]
        public ActionResult Index(string search = "", double min = 0, double max = 0)
        {
            List<Product> products = db.Products.Where(x => x.StorageCount > 0 && x.Name.Substring(0, search.Length).ToLower() == search.ToLower()).ToList();
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

        //Страница заказа продукта
        [HttpGet]
        public ActionResult MakeOrder(int productId = -1)
        {
            if (productId < 1)
                return new HttpStatusCodeResult(400);

            ViewBag.Errors = new string[] { };
            return View(db.Products.FirstOrDefault(x => x.Id == productId));
        }

        [HttpPost]
        public ActionResult MakeOrder([Bind(Include = "Id, StorageCount")] Product product)
        {
            List<string> errors = new List<string>();
            Product dbProduct = db.Products.FirstOrDefault(x => x.Id == product.Id);

            if (product.StorageCount >= 1 && product.StorageCount <= dbProduct.StorageCount)
            {
                dbProduct.StorageCount -= product.StorageCount;

                Order order = new Order();
                order.OrderTime = DateTime.Now;
                order.ProductCount = product.StorageCount;
                order.ProductId = product.Id;
                order.UserId = Convert.ToInt32(Session["Id"]);

                db.Orders.Add(order);

                db.SaveChanges();

                return RedirectToAction("OrderConfirm", new { orderId = order.Id });
            }
            else
            {
                errors.Add("To order count is less than 1 or more than product count on storage");
            }

            ViewBag.Errors = errors.ToArray();
            return View(dbProduct);
        }

        [HttpGet]
        public ActionResult OrderConfirm(int orderId = -1)
        {
            if (orderId < 1)
                return new HttpStatusCodeResult(400);

            Order order = db.Orders.FirstOrDefault(x => x.Id == orderId);
            if (Convert.ToInt32(Session["Id"]) != order.UserId)
                return new HttpStatusCodeResult(403);

            Product product = db.Products.FirstOrDefault(x => x.Id == order.ProductId);
            ViewBag.Product = product;

            return View(order);
        }
    }
}