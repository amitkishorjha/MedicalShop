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
    public class ClientsController : Controller
    {
        private readonly IClientService clientService;

        public ClientsController(IClientService clientService)
        {
            this.clientService = clientService;
        }


        // GET: Clients
        public ActionResult Index()
        {
            return View(clientService.GetAll().ToList());
        }

        // GET: Clients/Details/5
        public ActionResult Details(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client= clientService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client client)
        {
            try
            {
                client.CreatedBy = User.Identity.Name;
                client.CreatedDate = DateTime.Now;
                clientService.Add(client);
                clientService.Save();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(client);
            }   
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = clientService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                client.UpdatedBy = User.Identity.Name;
                client.UpdatedDate = DateTime.Now;

                clientService.Edit(client);
                clientService.Save();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = clientService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid UniqueId)
        {
            Client client = clientService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();

            client.DeletedBy = User.Identity.Name;
            client.DeletedDate = DateTime.Now;
            clientService.Delete(client);
            clientService.Save();
            return RedirectToAction("Index");
        }
    }
}
