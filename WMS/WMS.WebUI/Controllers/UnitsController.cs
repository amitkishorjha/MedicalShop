using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WMS.Models;
using WMS.Service.Interface;
using WMS.WebUI.Common;
using WMS.WebUI.Filter;

namespace WMS.WebUI.Controllers
{
    [SimpleAuthorizeAttribute]
    [SessionExpire]
    public class UnitsController : Controller
    {
        private readonly IUnitService unitService;

        public UnitsController(IUnitService unitService)
        {
            this.unitService = unitService;
        }

        // GET: Units
        public ActionResult Index()
        {
            return View(unitService.GetAll().ToList());
        }

        // GET: Units/Details/5
        public ActionResult Details(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = unitService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // GET: Units/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Units/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Unit unit)
        {
            try
            {
                unit.CreatedBy = User.Identity.Name;
                unit.CreatedDate = DateTime.Now;
                unitService.Add(unit);
                unitService.Save();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(unit);
            }
        }

        // GET: Units/Edit/5
        public ActionResult Edit(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = unitService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // POST: Units/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Unit unit)
        {
            if (ModelState.IsValid)
            {
                unit.UpdatedBy = User.Identity.Name;
                unit.UpdatedDate = DateTime.Now;

                unitService.Edit(unit);
                unitService.Save();
                return RedirectToAction("Index");
            }
            return View(unit);
        }

        // GET: Units/Delete/5
        public ActionResult Delete(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = unitService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // POST: Units/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid UniqueId)
        {
            Unit unit = unitService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();

            unit.DeletedBy = User.Identity.Name;
            unit.DeletedDate = DateTime.Now;
            unitService.Delete(unit);
            unitService.Save();
            return RedirectToAction("Index");
        }
    }
}
