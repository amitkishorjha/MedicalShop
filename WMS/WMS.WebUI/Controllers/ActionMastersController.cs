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
    public class ActionMastersController : Controller
    {
        private readonly IActionMasterService actionMasterService;
        public ActionMastersController(IActionMasterService actionMasterService)
        {
            this.actionMasterService = actionMasterService;

        }

        // GET: ActionMasters
        public ActionResult Index()
        {
            return View(actionMasterService.GetAll().ToList());
        }

        // GET: ActionMasters/Details/5
        public ActionResult Details(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var actionMaster = actionMasterService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (actionMaster == null)
            {
                return HttpNotFound();
            }
            return View(actionMaster);
        }

        // GET: ActionMasters/Create
        public ActionResult Create()
        {
            ActionMaster actionMaster = new ActionMaster();
            actionMaster.CreatedBy = User.Identity.Name;
            return View(actionMaster);
        }

        // POST: ActionMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ActionMaster actionMaster)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add insert logic here
                actionMaster.CreatedBy = User.Identity.Name;
                actionMaster.CreatedDate = DateTime.Now;
                actionMaster.IsActive = true;
                actionMasterService.Add(actionMaster);
                actionMasterService.Save();

                return RedirectToAction("Index");
            }

            return View(actionMaster);
        }

        // GET: ActionMasters/Edit/5
        public ActionResult Edit(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var actionMaster = actionMasterService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (actionMaster == null)
            {
                return HttpNotFound();
            }
            return View(actionMaster);
        }

        // POST: ActionMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( ActionMaster actionMaster)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add update logic here
                actionMaster.UpdatedBy = User.Identity.Name;
                actionMaster.UpdatedDate = DateTime.Now;
                actionMasterService.Edit(actionMaster);
                actionMasterService.Save();

                return RedirectToAction("Index");
            }
            return View(actionMaster);
        }

        // GET: ActionMasters/Delete/5
        public ActionResult Delete(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var actionMaster = actionMasterService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (actionMaster == null)
            {
                return HttpNotFound();
            }
            return View(actionMaster);
        }

        // POST: ActionMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid UniqueId)
        {
            var actionMaster = actionMasterService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            actionMaster.DeletedBy = User.Identity.Name;
            actionMaster.DeletedDate = DateTime.Now;
            actionMasterService.Delete(actionMaster);
            actionMasterService.Save();

            return RedirectToAction("Index");
        }
        
    }
}
