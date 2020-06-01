using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMS.Models;
using WMS.Service.Interface;
using WMS.WebUI.Common;
using WMS.WebUI.Filter;
using WMS.WebUI.Helper;
using WMS.WebUI.Models;

namespace WMS.WebUI.Controllers
{
    [SimpleAuthorizeAttribute]
    [SessionExpire]
    public class OrderController : Controller
    {
        private readonly IProductService productService;
        private readonly IClientService clientService;
        private readonly IStockService stockService;
        private readonly ISupplierService supplierService;
        private readonly IUnitService unitService;
        private readonly IOrderExitService orderExitService;

        public OrderController(IProductService productService, IClientService clientService, IStockService stockService
            , ISupplierService supplierService, IUnitService unitService, IOrderExitService orderExitService)
        {
            this.productService = productService;
            this.clientService = clientService;
            this.stockService = stockService;
            this.supplierService = supplierService;
            this.unitService = unitService;
            this.orderExitService = orderExitService;
        }


        public ActionResult GetOrderEntry()
        {
            var ordersEntry = (from sto in stockService.GetAll().ToList()
                              join sup in supplierService.GetAll().ToList() on sto.SupplierId equals sup.UniqueId
                              join pro in productService.GetAll().ToList() on sto.ProductId equals pro.UniqueId
                              join un in unitService.GetAll().ToList() on sto.UnitId equals un.UniqueId
                              select new OrderEntry
                              {
                                  StockId = sto.UniqueId,
                                  ProductName = pro.Name,
                                  ProductNumber = pro.Number,
                                  SupplierName = sup.Name,
                                  UnitName = un.Name,
                                  Quantity = sto.Quatity,
                                  CostPerProduct = sto.CostPerProduct,
                                  EntryDate = sto.EntryDate
                              }).ToList();

            return View(ordersEntry);
        }

        [HttpGet]
        // GET: Order
        public ActionResult OrderEntry()
        {
            ViewBag.Suppliers = supplierService.GetAll().Where(x => x.IsActive == true).ToList();
            ViewBag.Products = productService.GetAll().Where(x => x.IsActive == true).ToList();
            ViewBag.Units = unitService.GetAll().Where(x => x.IsActive == true).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult OrderEntry(OrderEntry orderEntry)
        {
            try
            {
                Stock stock = new Stock();
                stock.SupplierId = orderEntry.Supplier.UniqueId;
                stock.ProductId = orderEntry.Product.UniqueId;
                stock.UnitId = orderEntry.Unit.UniqueId;
                stock.EntryDate = orderEntry.Stock.EntryDate;
                stock.Quatity = orderEntry.Stock.Quatity;
                stock.CostPerProduct = orderEntry.Stock.CostPerProduct;
                stock.CreatedBy = User.Identity.Name;
                stock.CreatedDate = DateTime.Now;

                stockService.Add(stock);
                stockService.Save();

                return RedirectToAction("GetOrderEntry");
            }
            catch (Exception ex)
            {
                ViewBag.Suppliers = supplierService.GetAll().Where(x => x.IsActive == true).ToList();
                ViewBag.Products = productService.GetAll().Where(x => x.IsActive == true).ToList();
                ViewBag.Units = unitService.GetAll().Where(x => x.IsActive == true).ToList();
            }

            return View(orderEntry);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Suppliers = supplierService.GetAll().Where(x => x.IsActive == true).ToList();
            ViewBag.Products = productService.GetAll().Where(x => x.IsActive == true).ToList();
            ViewBag.Units = unitService.GetAll().Where(x => x.IsActive == true).ToList();

            var ordersEntry = (from sto in stockService.GetAll().ToList()
                               where sto.UniqueId == UniqueId
                               select new OrderEntry
                               {
                                   StockId = sto.UniqueId,
                                   ProductId = sto.ProductId,
                                   SupplierId = sto.SupplierId,
                                   UnitId = sto.UnitId,
                                   Quantity = sto.Quatity,
                                   CostPerProduct = sto.CostPerProduct,
                                   EntryDate = sto.EntryDate
                               }).FirstOrDefault();

            if (ordersEntry == null)
            {
                return HttpNotFound();
            }

            return View(ordersEntry);
        }


        [HttpPost]
        public ActionResult Edit(OrderEntry orderEntry)
        {
            try
            {
                Stock stock = stockService.FindBy(x => x.UniqueId == orderEntry.StockId && x.IsActive == true).FirstOrDefault();
                stock.SupplierId = orderEntry.SupplierId;
                stock.ProductId = orderEntry.ProductId;
                stock.UnitId = orderEntry.UnitId;
                stock.EntryDate = orderEntry.EntryDate;
                stock.Quatity = orderEntry.Quantity;
                stock.CostPerProduct = orderEntry.CostPerProduct;
                stock.UpdatedBy = User.Identity.Name;
                stock.UpdatedDate = DateTime.Now;

                stockService.Edit(stock);
                stockService.Save();

                return RedirectToAction("GetOrderEntry");
            }
            catch (Exception ex)
            {
                ViewBag.Suppliers = supplierService.GetAll().Where(x => x.IsActive == true).ToList();
                ViewBag.Products = productService.GetAll().Where(x => x.IsActive == true).ToList();
                ViewBag.Units = unitService.GetAll().Where(x => x.IsActive == true).ToList();
            }

            return View(orderEntry);
        }



        public ActionResult GetOrderExit()
        {
            var ordersExit= (from sto in orderExitService.GetAll().ToList()
                               join cli in clientService.GetAll().ToList() on sto.ClientId equals cli.UniqueId
                               join pro in productService.GetAll().ToList() on sto.ProductId equals pro.UniqueId
                               select new WMS.WebUI.Models.OrderExit
                               {
                                   OrderExitId = sto.UniqueId,
                                   ProductName = pro.Name,
                                   ProductNumber = pro.Number,
                                   ClientName = cli.Name,
                                   Quantity = sto.Quatity,
                                   Cost = sto.Cost,
                                   ExitDate = sto.ExitDate
                               }).ToList();

            return View(ordersExit);
        }

        [HttpGet]
        // GET: Order
        public ActionResult OrderExit()
        {
            ViewBag.Clients = clientService.GetAll().Where(x => x.IsActive == true).ToList();
            ViewBag.Products = productService.GetAll().Where(x => x.IsActive == true).ToList();
            ViewBag.Units = unitService.GetAll().Where(x => x.IsActive == true).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult OrderExit(WMS.WebUI.Models.OrderExit orderExit , Guid ProductId ,Guid ClientId)
        {
            try
            {
                Stock stock = stockService.GetAll().Where(x => x.ProductId == ProductId && x.IsActive == true).FirstOrDefault();

                if (stock != null)
                {
                    if (stock.Quatity == 0)
                    {
                        throw new Exception("This product is not abeliable in stock");
                    }
                    else if (stock.Quatity < orderExit.Quantity)
                    {
                        throw new Exception("only " + stock.Quatity + " exist in stock. you have requested more quantity");
                    }

                }
                else
                {
                    throw new Exception("This product is not abeliable in stock");
                }

                WMS.Models.OrderExit orderExitdata = new WMS.Models.OrderExit();

                orderExitdata.ClientId = ClientId;
                orderExitdata.ProductId = ProductId;
                orderExitdata.ExitDate = orderExit.ExitDate;
                orderExitdata.Quatity = orderExit.Quantity;
                orderExitdata.Cost = orderExit.Cost;
                orderExitdata.CreatedBy = User.Identity.Name;
                orderExitdata.CreatedDate = DateTime.Now;

                orderExitService.Add(orderExitdata);
                orderExitService.Save();

                stock.Quatity = stock.Quatity - Convert.ToInt32( orderExit.Quantity);
                stock.UpdatedBy = User.Identity.Name;
                stock.UpdatedDate = DateTime.Now;

                stockService.Edit(stock);
                stockService.Save();
                return RedirectToAction("GetOrderExit");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Clients = clientService.GetAll().Where(x => x.IsActive == true).ToList();
                ViewBag.Products = productService.GetAll().Where(x => x.IsActive == true).ToList();
                ViewBag.Units = unitService.GetAll().Where(x => x.IsActive == true).ToList();
            }

            return View(orderExit);
        }


        // GET: Clients/Edit/5
        public ActionResult EditExit(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Clients = clientService.GetAll().Where(x => x.IsActive == true).ToList();
            ViewBag.Products = productService.GetAll().Where(x => x.IsActive == true).ToList();
            ViewBag.Units = unitService.GetAll().Where(x => x.IsActive == true).ToList();

            var ordersExit = (from sto in orderExitService.GetAll().ToList()
                               where sto.UniqueId == UniqueId
                               select new WMS.WebUI.Models.OrderExit
                               {
                                   OrderExitId = sto.UniqueId,
                                   ProductId = sto.ProductId,
                                   ClientId = sto.ClientId,
                                   Quantity = sto.Quatity,
                                   Cost = sto.Cost,
                                   ExitDate = sto.ExitDate
                               }).FirstOrDefault();

            if (ordersExit == null)
            {
                return HttpNotFound();
            }

            return View(ordersExit);
        }


        [HttpPost]
        public ActionResult EditExit(WMS.WebUI.Models.OrderExit orderExit)
        {
            try
            {
                Stock stock= stockService.GetAll().Where(x=>x.ProductId == orderExit.ProductId && x.IsActive == true).FirstOrDefault();

                if (stock != null)
                {
                    if (stock.Quatity == 0)
                    {
                        throw new Exception("This product is not abeliable in stock");
                    }else if(stock.Quatity < orderExit.Quantity)
                    {
                        throw new Exception("only "+ stock.Quatity + " exist in stock. you have requested more quantity");
                    }

                }else
                {
                    throw new Exception("This product is not abeliable in stock");
                }

                WMS.Models.OrderExit orderExitdata = orderExitService.FindBy(x => x.UniqueId == orderExit.OrderExitId).FirstOrDefault();

                orderExitdata.ClientId = orderExit.ClientId;
                orderExitdata.ProductId = orderExit.ProductId;
                orderExitdata.ExitDate = orderExit.ExitDate;
                orderExitdata.Quatity = orderExit.Quantity;
                orderExitdata.Cost = orderExit.Cost;
                orderExitdata.UpdatedBy = User.Identity.Name;
                orderExitdata.UpdatedDate = DateTime.Now;

                orderExitService.Edit(orderExitdata);
                orderExitService.Save();

                stock.Quatity = stock.Quatity - Convert.ToInt32(orderExit.Quantity);
                stock.UpdatedBy = User.Identity.Name;
                stock.UpdatedDate = DateTime.Now;

                stockService.Edit(stock);
                stockService.Save();

                return RedirectToAction("GetOrderExit");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Clients = clientService.GetAll().Where(x => x.IsActive == true).ToList();
                ViewBag.Products = productService.GetAll().Where(x => x.IsActive == true).ToList();
                ViewBag.Units = unitService.GetAll().Where(x => x.IsActive == true).ToList();
            }

            return View(orderExit);
        }

        [HttpGet]
        public ActionResult GetOrderEntryReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetOrderEntryReport(DateTime fromDate , DateTime toDate)
        {
            List<string> Entryheders = new List<string>();
            Entryheders.Add("Product Name");
            Entryheders.Add("Product Number");
            Entryheders.Add("Supplier Name");
            Entryheders.Add("Unit Name");
            Entryheders.Add("Quantity");
            Entryheders.Add("Cost");
            Entryheders.Add("Entry Date");

            var ordersEntry = (from sto in stockService.GetAll().ToList()
                               join sup in supplierService.GetAll().ToList() on sto.SupplierId equals sup.UniqueId
                               join pro in productService.GetAll().ToList() on sto.ProductId equals pro.UniqueId
                               join un in unitService.GetAll().ToList() on sto.UnitId equals un.UniqueId
                               where sto.CreatedDate.Date >= fromDate.Date && sto.CreatedDate.Date <= toDate.Date && sto.IsActive == true
                               select new OrderEnterReport
                               {
                                   ProductName = pro.Name,
                                   ProductNumber = pro.Number,
                                   SupplierName = sup.Name,
                                   UnitName = un.Name,
                                   Quantity = sto.Quatity,
                                   CostPerProduct = sto.CostPerProduct,
                                   EntryDate = sto.EntryDate.ToString("dd-MM-yyyy")
                               }).ToList();


            DataSet ds = new DataSet();
            DataTable orderEntryTable = ExportExcelhelper.ListToDataTable(ordersEntry);

            ds.Tables.Add(orderEntryTable);

            byte[] filecontent = ExportExcelhelper.ExportExcelDataset(ds, Entryheders, "Purchase Orders", true, null, null, Entryheders.ToArray());

            return File(filecontent, ExportExcelhelper.ExcelContentType, "OrderEntries.xlsx");
      }

        [HttpGet]
        public ActionResult GetOrderExitReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetOrderExitReport(DateTime fromDate, DateTime toDate)
        {
            List<string> Exitheders = new List<string>();
            Exitheders.Add("Product Name");
            Exitheders.Add("Product Number");
            Exitheders.Add("Client Name");
            Exitheders.Add("Quantity");
            Exitheders.Add("Cost");
            Exitheders.Add("Exit Date");

            var ordersExit = (from sto in orderExitService.GetAll().ToList()
                              join cli in clientService.GetAll().ToList() on sto.ClientId equals cli.UniqueId
                              join pro in productService.GetAll().ToList() on sto.ProductId equals pro.UniqueId
                              where sto.CreatedDate.Date >= fromDate.Date && sto.CreatedDate.Date <= toDate.Date && sto.IsActive == true
                              select new WMS.WebUI.Models.OrderExitReport
                              {
                                  ProductName = pro.Name,
                                  ProductNumber = pro.Number,
                                  ClientName = cli.Name,
                                  Quantity = sto.Quatity,
                                  Cost = sto.Cost,
                                  ExitDate = sto.ExitDate.ToString("dd-MM-yyyy")
                              }).ToList();

            DataSet ds = new DataSet();
            DataTable orderExitTable = ExportExcelhelper.ListToDataTable(ordersExit);

            ds.Tables.Add(orderExitTable);

            byte[] filecontent = ExportExcelhelper.ExportExcelDataset(ds, Exitheders, "Sell Orders", true, null,null,Exitheders.ToArray());

            return File(filecontent, ExportExcelhelper.ExcelContentType, "OrderExit.xlsx");
        }

        public ActionResult PrintOrderEntry(Guid uniqueId)
        {
            var ordersEntry = (from sto in stockService.GetAll().ToList()
                               join sup in supplierService.GetAll().ToList() on sto.SupplierId equals sup.UniqueId
                               join pro in productService.GetAll().ToList() on sto.ProductId equals pro.UniqueId
                               join un in unitService.GetAll().ToList() on sto.UnitId equals un.UniqueId
                               where sto.UniqueId == uniqueId
                               select new OrderEntry
                               {
                                   ProductName = pro.Name,
                                   ProductNumber = pro.Number,
                                   SupplierName = sup.Name,
                                   SupplierAddress = sup.Address,
                                   UnitName = un.Name,
                                   Quantity = sto.Quatity,
                                   CostPerProduct = sto.CostPerProduct,
                                   EntryDate = sto.EntryDate
                               }).FirstOrDefault();

            return View(ordersEntry);
        }

        public ActionResult PrintOrderExit(Guid uniqueId)
        {
            var ordersExit = (from sto in orderExitService.GetAll().ToList()
                              join cli in clientService.GetAll().ToList() on sto.ClientId equals cli.UniqueId
                              join pro in productService.GetAll().ToList() on sto.ProductId equals pro.UniqueId
                              where sto.UniqueId == uniqueId 
                              select new WMS.WebUI.Models.OrderExit
                              {
                                  OrderExitId = sto.UniqueId,
                                  ProductName = pro.Name,
                                  ProductNumber = pro.Number,
                                  ClientAddress = cli.Address,
                                  ClientName = cli.Name,
                                  Quantity = sto.Quatity,
                                  Cost = sto.Cost,
                                  ExitDate = sto.ExitDate
                              }).FirstOrDefault();


            return View(ordersExit);
        }

    }
}