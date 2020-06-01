using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using WMS.Models;
using WMS.Service.Interface;
using WMS.WebUI.Common;
using WMS.WebUI.Filter;
using WMS.WebUI.Models;

namespace WMS.WebUI.Controllers
{
    public class UserMastersController : Controller
    {
        private readonly IUserMasterService userMasterService;
        private readonly IRoleMasterService roleMasterService;
        private readonly IUserRoleMappingService userRoleMappingService;
        private readonly IUnitService unitService;
        private readonly IStockService stockService;
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        public UserMastersController(IUserMasterService userMasterService, IRoleMasterService roleMasterService,
            IUserRoleMappingService userRoleMappingService, IUnitService unitService, IStockService stockService, IProductService productService, ICategoryService categoryService)
        {
            this.userMasterService = userMasterService;
            this.roleMasterService = roleMasterService;
            this.userRoleMappingService = userRoleMappingService;
            this.unitService = unitService;
            this.stockService = stockService;
            this.productService = productService;
            this.categoryService = categoryService;
        }

        [SimpleAuthorizeAttribute]
        [SessionExpire]
        // GET: UserMasters
        public ActionResult Index()
        {
            return View(userMasterService.GetAll().ToList());
        }

        [SimpleAuthorizeAttribute]
        [SessionExpire]
        // GET: UserMasters/Details/5
        public ActionResult Details(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMaster userMaster = userMasterService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (userMaster == null)
            {
                return HttpNotFound();
            }
            return View(userMaster);
        }

        [SimpleAuthorizeAttribute]
        [SessionExpire]
        // GET: UserMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SimpleAuthorizeAttribute]
        [SessionExpire]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserMaster userMaster)
        {
            try
            {
                userMaster.CreatedBy = User.Identity.Name;
                userMaster.CreatedDate = DateTime.Now;
                userMaster.Username = userMaster.FirstName + userMaster.MiddleName + userMaster.LastName;
                userMasterService.Add(userMaster);
                userMasterService.Save();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(userMaster);
            }
        }

        [SimpleAuthorizeAttribute]
        [SessionExpire]
        // GET: UserMasters/Edit/5
        public ActionResult Edit(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMaster userMaster = userMasterService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (userMaster == null)
            {
                return HttpNotFound();
            }
            return View(userMaster);
        }

        // POST: UserMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SimpleAuthorizeAttribute]
        [SessionExpire]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserMaster userMaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    userMaster.UpdatedBy = User.Identity.Name;
                    userMaster.UpdatedDate = DateTime.Now;
                    userMaster.Username = userMaster.FirstName + userMaster.MiddleName + userMaster.LastName;
                    userMasterService.Edit(userMaster);
                    userMasterService.Save();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

            }
            return View(userMaster);
        }

        // GET: UserMasters/Delete/5
        [SimpleAuthorizeAttribute]
        [SessionExpire]
        public ActionResult Delete(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMaster userMaster = userMasterService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (userMaster == null)
            {
                return HttpNotFound();
            }
            return View(userMaster);
        }

        // POST: UserMasters/Delete/5
        [SimpleAuthorizeAttribute]
        [SessionExpire]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid UniqueId)
        {
            UserMaster userMaster = userMasterService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();
            userMaster.DeletedBy = User.Identity.Name;
            userMaster.DeletedDate = DateTime.Now;
            userMasterService.Delete(userMaster);
            userMasterService.Save();
            return RedirectToAction("Index");
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UserMaster _user, string returnUrl)
        {
            UserMaster user = userMasterService.FindBy(us => us.Username == _user.Username && us.IsActive == true).FirstOrDefault();

            if (user != null)
            {
                if (user.Password == _user.Password)
                {
                    FormsAuthentication.SetAuthCookie(_user.Username, true);
                    Session["user"] = user;

                    user.LastLoginDate = DateTime.Now.ToString("dd-MM-yyyy hh:mm");
                    user.UpdatedBy = _user.Username;
                    user.UpdatedDate = DateTime.Now;
                    userMasterService.Edit(user);
                    userMasterService.Save();

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("GetUserRoles", "UserMasters");
                    }

                }
                else
                    ModelState.AddModelError(string.Empty, "please enter correct details");
            }

            ModelState.AddModelError(string.Empty, "User not exist");
            return View();
        }

        [HttpGet]
        public ActionResult logOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");

        }

        [SimpleAuthorizeAttribute]
        [Authorize]
        [SessionExpire]
        public ActionResult UserRoleMapping(Guid UniqueId)
        {
            UserRoleMappings userRoleMapping = new UserRoleMappings();

            UserMaster userMaster = userMasterService.FindBy(x => x.UniqueId == UniqueId).FirstOrDefault();

            userRoleMapping.UserId = userMaster.UniqueId;
            userRoleMapping.UserName = userMaster.Username;

            userRoleMapping.Role = roleMasterService.GetAll().ToList();

            userRoleMapping.UserRoleMapping = userRoleMappingService.GetAll().Where(x => x.UserMasterId == UniqueId && x.IsActive ==true).ToList();

            return View(userRoleMapping);

        }

        [SimpleAuthorizeAttribute]
        [SessionExpire]
        [HttpPost]
        public ActionResult UserRoleMapping(Guid[] RoleId, Guid UserId, Guid[] UnMap)
        {
            UserRoleMappings userRoleMappings = new UserRoleMappings();
            try
            {
                var userRoleMapp = userRoleMappingService.GetAll().Where(x => x.UserMasterId == UserId && x.IsActive == true).ToList();

                if (RoleId != null)
                {
                    foreach (var item in RoleId)
                    {
                        if (userRoleMapp.Select(x => x.RoleMasterId).Contains(item) == false)
                        {
                            UserRoleMapping userRoleMapping = new UserRoleMapping();
                            userRoleMapping.UserMasterId = UserId;
                            userRoleMapping.RoleMasterId = item;
                            userRoleMapping.CreatedBy = User.Identity.Name;
                            userRoleMapping.CreatedDate = DateTime.Now;

                            userRoleMappingService.Add(userRoleMapping);
                            userRoleMappingService.Save();
                        }
                    }
                }

                if (UnMap != null)
                {
                    foreach (var item in UnMap)
                    {
                        if (userRoleMapp.Select(x => x.RoleMasterId).Contains(item))
                        {
                            UserRoleMapping userRoleMapping = userRoleMapp.Where(x => x.RoleMasterId == item && x.UserMasterId == UserId).FirstOrDefault();
                            userRoleMapping.IsActive = false;
                            userRoleMapping.DeletedBy = User.Identity.Name;
                            userRoleMapping.DeletedDate = DateTime.Now;

                            userRoleMappingService.Delete(userRoleMapping);
                            userRoleMappingService.Save();
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                UserMaster userMaster = userMasterService.FindBy(x => x.UniqueId == UserId).FirstOrDefault();

                userRoleMappings.UserId = userMaster.UniqueId;
                userRoleMappings.UserName = userMaster.Username;

                userRoleMappings.Role = roleMasterService.GetAll().ToList();
            }

            return View(userRoleMappings);
        }

        [SimpleAuthorizeAttribute]
        [SessionExpire]
        public ActionResult WareHouseLayout()
        {
            var Units = unitService.GetAll().OrderBy(x => x.CreatedDate).ToList();
            List<UnitViewModel> unitViewmodel = new List<UnitViewModel>();

            for (int i = 0; i <= 265; i++)
            {
                UnitViewModel unit = new UnitViewModel();
                if (Units.Count > i)
                {
                    unit.Name = Units.ElementAt(i).Name;
                    unit.UnitId = Units.ElementAt(i).UniqueId;

                    var quantity = (from st in stockService.GetAll().ToList()
                                    join un in unitService.GetAll().ToList() on st.UnitId equals un.UniqueId
                                    join po in productService.GetAll().ToList() on st.ProductId equals po.UniqueId
                                    where un.Name == unit.Name && st.Quatity > 0 && st.IsActive == true
                                    group st by po.Name into pro
                                    select new
                                    {
                                        ProductName = pro.Key,
                                        data = pro.ToList()
                                    }).ToList();

                    if (quantity != null)
                    {
                        unit.IsHaveProduct = "False," + unit.Name;
                        if (quantity.Count() > 0)
                        {
                            unit.IsHaveProduct = "True,"+unit.Name;
                            foreach (var item in quantity)
                            {
                                unit.ProductName +=" Product: "+ item.ProductName + " | Quanity: " + item.data.Sum(x => x.Quatity).ToString();
                            }
                        }
                        unitViewmodel.Add(unit);
                    }
                }
                else
                {
                    unit.Name = "Unit " + i;
                    unit.IsHaveProduct = "False," + unit.Name;
                    unitViewmodel.Add(unit);
                }

            }

            return View(unitViewmodel);
        }

        [SimpleAuthorizeAttribute]
        [SessionExpire]
        [HttpGet]
        public ActionResult GetUnitProducts(Guid UnitId,string UnitName)
        {
            try
            {
                List<UnitViewModel> unitViewmodel = new List<UnitViewModel>();

                var quantity = (from st in stockService.GetAll().ToList()
                                join un in unitService.GetAll().ToList() on st.UnitId equals un.UniqueId
                                join po in productService.GetAll().ToList() on st.ProductId equals po.UniqueId
                                where un.UniqueId == UnitId && st.Quatity > 0 && st.IsActive == true
                                group st by po.UniqueId into pro
                                select new
                                {
                                    ProductName = pro.Key,
                                    data = pro.ToList()
                                }).ToList();

                if (quantity != null)
                {
                    if (quantity.Count() > 0)
                    {
                        foreach (var item in quantity)
                        {
                            UnitViewModel unit = new UnitViewModel();
                            var product = productService.GetAll().Where(x => x.UniqueId == item.ProductName).FirstOrDefault();

                            unit.UnitName = UnitName;
                            unit.ProductName = product.Name;
                            unit.ImageUrl = "/Uploads/Product/" + product.ImagePath.Substring(product.ImagePath.LastIndexOf('\\'));
                            unit.SerialNo = product.Number;
                            unit.Category = categoryService.GetAll().Where(x => x.UniqueId == product.CategoryId).Select(x => x.Name).FirstOrDefault();
                            unit.Quanitiy = item.data.Sum(x => x.Quatity).ToString();
                            unit.Price = product.Price;
                            unit.modelNo = product.modelNo;
                            unit.Description = product.Description;

                            unitViewmodel.Add(unit);
                        }
                    }else
                    {
                        UnitViewModel unit = new UnitViewModel();
                        unit.UnitName = UnitName;

                        unitViewmodel.Add(unit);
                    }
                }

                return View(unitViewmodel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View();
        }

        [HttpGet]
        public ActionResult GetUserRoles()
        {
            var user = userMasterService.GetAll().Where(x => x.Username == User.Identity.Name
                        && x.IsActive == true).FirstOrDefault();

            ViewBag.Roles = (from ur in userRoleMappingService.GetAll().ToList().Where(x => x.UserMasterId == user.UniqueId)
                             join ro in roleMasterService.GetAll().ToList() on ur.RoleMasterId equals ro.UniqueId
                             select ro).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult GetUserRoles(Guid RoleId)
        {
            Session["RoleId"] = RoleId;
            return RedirectToAction("WareHouseLayout");
        }

        public ActionResult Unauthorized()
        {
            return View();
        }


    }
}
