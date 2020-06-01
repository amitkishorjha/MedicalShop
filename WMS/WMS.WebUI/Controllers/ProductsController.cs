using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMS.Models;
using WMS.Repository.Common;
using WMS.Service.Interface;
using WMS.WebUI.Common;
using WMS.WebUI.Filter;

namespace WMS.WebUI.Controllers
{
    [SimpleAuthorizeAttribute]
    [SessionExpire]
    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }


        // GET: Products
        public ActionResult Index()
        {
            var products = from po in productService.GetAll().ToList()
                           join ca in categoryService.GetAll().ToList() on po.CategoryId equals ca.UniqueId
                           select new Product()
                           {
                                UniqueId = po.UniqueId,
                                Name = po.Name,
                                Number = po.Number,
                                modelNo = po.modelNo,
                                Price = po.Price,
                                CategoryName = ca.Name,
                                Description = po.Description,
                                ImagePath = po.ImagePath,
                                ImagePath1 = po.ImagePath1
                           };

            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Categories = categoryService.GetAll().Where(x => x.IsActive == true).ToList();
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            try
            {
                if (!Directory.Exists(Server.MapPath("~/Uploads/Product/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/Product/"));
                }

                string path = null, secondpath = null;
                if (product.ImageUrl != null)
                {
                    var filename = Path.GetFileName(product.ImageUrl.FileName);
                    path = Path.Combine(Server.MapPath("~/Uploads/Product/"), product.UniqueId.ToString().Substring(0, product.UniqueId.ToString().IndexOf('-')) + "__" + filename);
                    product.ImageUrl.SaveAs(path);
                }

                if (product.ImageUrl1 != null)
                {
                    var secondname = Path.GetFileName(product.ImageUrl1.FileName);
                    secondpath = Path.Combine(Server.MapPath("~/Uploads/Product/"), product.UniqueId.ToString().Substring(0, product.UniqueId.ToString().IndexOf('-')) + "__" + secondname);
                    product.ImageUrl1.SaveAs(secondpath);
                }

                if (path != null)
                {
                    product.ImagePath = path;
                }else
                {
                    throw new Exception("Please choose one image for product");
                }

                product.ImagePath1 = secondpath;
                product.CreatedBy = User.Identity.Name;
                product.CreatedDate = DateTime.Now;
                productService.Add(product);
                productService.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Categories = categoryService.GetAll().Where(x => x.IsActive == true).ToList();
                return View(product);
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Categories = categoryService.GetAll().Where(x => x.IsActive == true).ToList();
            Product product = productService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                string lastImagePath = product.ImagePath;
                string lastsecondImagePath = product.ImagePath1;

                if (!Directory.Exists(Server.MapPath("~/Uploads/Product/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/Product/"));
                }

                string path =null, secondpath = null;
                if (product.ImageUrl != null)
                {
                    var filename = Path.GetFileName(product.ImageUrl.FileName);
                    path = Path.Combine(Server.MapPath("~/Uploads/Product/"), product.UniqueId.ToString().Substring(0, product.UniqueId.ToString().IndexOf('-')) + "__" + filename);
                    product.ImageUrl.SaveAs(path);
                }
                else
                    path = lastImagePath;

                if (product.ImageUrl1 != null)
                {

                    var secondname = Path.GetFileName(product.ImageUrl1.FileName);
                    secondpath = Path.Combine(Server.MapPath("~/Uploads/Product/"), product.UniqueId.ToString().Substring(0, product.UniqueId.ToString().IndexOf('-')) + "__" + secondname);
                    product.ImageUrl1.SaveAs(secondpath);
                }
                else
                    secondpath = lastsecondImagePath;

                product.ImagePath = path;
                product.ImagePath1 = secondpath;
                product.UpdatedBy = User.Identity.Name;
                product.UpdatedDate = DateTime.Now;

                productService.Edit(product);
                productService.Save();

                if ((System.IO.File.Exists(lastImagePath)))
                {
                    System.IO.File.Delete(lastImagePath);
                }

                if ((System.IO.File.Exists(lastsecondImagePath)))
                {
                    System.IO.File.Delete(lastsecondImagePath);
                }

                return RedirectToAction("Index");
                
            }

            ViewBag.Categories = categoryService.GetAll().Where(x => x.IsActive == true).ToList();
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid UniqueId)
        {
            Product product = productService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();

            product.DeletedBy = User.Identity.Name;
            product.DeletedDate = DateTime.Now;
            productService.Delete(product);
            productService.Save();
            return RedirectToAction("Index");
        }
    }
}
