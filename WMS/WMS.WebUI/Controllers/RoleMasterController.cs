using System;
using System.Linq;
using System.Web.Mvc;
using WMS.Models;
using WMS.Service.Interface;
using WMS.WebUI.Common;
using WMS.WebUI.Filter;
using WMS.WebUI.Models;

namespace WMS.WebUI.Controllers
{
    [SimpleAuthorizeAttribute]
    [SessionExpire]
    public class RoleMasterController : Controller
    {
        private readonly IRoleMasterService roleMasterService;
        private readonly IActionMasterService actionMasterService;
        private readonly IRoleActionMappingService roleActionMappingService;

        public RoleMasterController(IRoleMasterService roleMasterService, IRoleActionMappingService roleActionMappingService,
            IActionMasterService actionMasterService)
        {
            this.roleMasterService = roleMasterService;
            this.roleActionMappingService = roleActionMappingService;
            this.actionMasterService = actionMasterService;

        }
        
        // GET: ActionMaster
        public ActionResult Index()
        {
            var roleList = roleMasterService.GetAll();
            return View(roleList);
        }

        // GET: ActionMaster/Details/5
        public ActionResult Details(Guid UniqueId)
        {
            var rolemaster = roleMasterService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            return View(rolemaster);
        }

        // GET: ActionMaster/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActionMaster/Create
        [HttpPost]
        public ActionResult Create(RoleMaster roleMaster)
        {
            try
            {
                // TODO: Add insert logic here
                roleMaster.CreatedBy = User.Identity.Name;
                roleMaster.CreatedDate = DateTime.Now;
                roleMaster.IsActive = true;
                roleMasterService.Add(roleMaster);
                roleMasterService.Save();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(roleMaster);
            }
        }

        // GET: ActionMaster/Edit/5
        public ActionResult Edit(Guid UniqueId)
        {
            var rolemaster = roleMasterService.FindBy(x=>x.UniqueId == UniqueId).FirstOrDefault();
            return View(rolemaster);
        }

        // POST: ActionMaster/Edit/5
        [HttpPost]
        public ActionResult Edit(RoleMaster roleMaster)
        {
            try
            {
                // TODO: Add update logic here
                roleMaster.UpdatedBy = User.Identity.Name;
                roleMaster.UpdatedDate = DateTime.Now;
                roleMasterService.Edit(roleMaster);
                roleMasterService.Save();

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(roleMaster);
            }
        }

        // GET: ActionMaster/Delete/5
        public ActionResult Delete(Guid UniqueId)
        {
            var rolemaster = roleMasterService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            return View(rolemaster);
        }

        // POST: ActionMaster/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid UniqueId,RoleMaster roleMaster)
        {
            var rolemaster = roleMasterService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();

            try
            {

                rolemaster.DeletedBy = User.Identity.Name;
                rolemaster.DeletedDate = DateTime.Now;
                roleMasterService.Delete(rolemaster);
                roleMasterService.Save();
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View(rolemaster);
            }
        }

        [Authorize]
        [SessionExpire]
        public ActionResult RoleActionMapping(Guid UniqueId)
        {
            RoleActionMappings roleActionMapping = new RoleActionMappings();

            RoleMaster roleMaster = roleMasterService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();

            roleActionMapping.RoleId = roleMaster.UniqueId;
            roleActionMapping.RoleName = roleMaster.Name;

            roleActionMapping.Action = actionMasterService.GetAll().ToList();

            roleActionMapping.RoleActionMapping = roleActionMappingService.GetAll().Where(x => x.RoleMasterId == UniqueId && x.IsActive == true).ToList();

            return View(roleActionMapping);

        }

        [Authorize]
        [SessionExpire]
        [HttpPost]
        public ActionResult RoleActionMapping(Guid[] ActionId, Guid RoleId, Guid[] UnMap)
        {
            RoleActionMappings userRoleMappings = new RoleActionMappings();
            try
            {
                var roleActionMap = roleActionMappingService.GetAll().Where(x => x.RoleMasterId == RoleId && x.IsActive == true).ToList();

                if (ActionId != null)
                {
                    foreach (var item in ActionId)
                    {
                        if (roleActionMap.Select(x => x.ActionMasterId).Contains(item) == false)
                        {
                            RoleActionMapping roleActionMapping = new RoleActionMapping();
                            roleActionMapping.RoleMasterId = RoleId;
                            roleActionMapping.ActionMasterId = item;
                            roleActionMapping.CreatedBy = User.Identity.Name;
                            roleActionMapping.CreatedDate = DateTime.Now;

                            roleActionMappingService.Add(roleActionMapping);
                            roleActionMappingService.Save();
                        }
                    }
                }

                if (UnMap != null)
                {
                    foreach (var item in UnMap)
                    {
                        if (roleActionMap.Select(x => x.ActionMasterId).Contains(item))
                        {
                            RoleActionMapping roleActionMapping = roleActionMap.Where(x => x.ActionMasterId == item && x.RoleMasterId == RoleId).FirstOrDefault();
                            roleActionMapping.IsActive = false;
                            roleActionMapping.DeletedBy = User.Identity.Name;
                            roleActionMapping.DeletedDate = DateTime.Now;

                            roleActionMappingService.Delete(roleActionMapping);
                            roleActionMappingService.Save();
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                RoleMaster roleMaster = roleMasterService.FindBy(x => x.UniqueId == RoleId).FirstOrDefault();

                userRoleMappings.RoleId = roleMaster.UniqueId;
                userRoleMappings.RoleName = roleMaster.Name;

                userRoleMappings.Action = actionMasterService.GetAll().ToList();

                userRoleMappings.RoleActionMapping = roleActionMappingService.GetAll().Where(x => x.RoleMasterId == RoleId).ToList();
            }

            return View(userRoleMappings);
        }

        public ActionResult Poster()
        {
            return View();
        }
    }

    
}