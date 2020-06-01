using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class SuppliersController : Controller
    {
        private readonly ISupplierService supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            this.supplierService = supplierService;
        }


        // GET: Suppliers
        public ActionResult Index()
        {
            return View(supplierService.GetAll().ToList());
        }

        // GET: Suppliers/Details/5
        public ActionResult Details(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supplier supplier)
        {
            try
            {
                supplier.CreatedBy = User.Identity.Name;
                supplier.CreatedDate = DateTime.Now;
                supplierService.Add(supplier);
                supplierService.Save();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(supplier);
            }
        }

        // GET: Suppliers/Edit/5
        public ActionResult Edit(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplier.UpdatedBy = User.Identity.Name;
                supplier.UpdatedDate = DateTime.Now;

                supplierService.Edit(supplier);
                supplierService.Save();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public ActionResult Delete(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid UniqueId)
        {
            Supplier supplier = supplierService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();

            supplier.DeletedBy = User.Identity.Name;
            supplier.DeletedDate = DateTime.Now;
            supplierService.Delete(supplier);
            supplierService.Save();
            return RedirectToAction("Index");
        }
    }
}
