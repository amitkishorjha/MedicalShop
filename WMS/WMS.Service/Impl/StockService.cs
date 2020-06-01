using WMS.Service.Interface;
using WMS.Models;
using WMS.Repository.Impl;
using WMS.Repository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace WMS.Service.Impl
{
    public class StockService : IStockService
    {
        private readonly IStockRepository stockRepository;

        public StockService(IStockRepository stockRepository)
        {
            this.stockRepository = stockRepository;
        }

        public void Add(Stock entity)
        {
            stockRepository.Add(entity);
        }

        public void Delete(Stock entity)
        {
            entity.IsActive = false;
            stockRepository.Edit(entity);
        }

        public void Edit(Stock entity)
        {
            entity.IsActive = false;
            stockRepository.Edit(entity);

            Stock am = new Stock();

            am.UniqueId = Guid.NewGuid();
            am.Id = entity.UniqueId;
            am.SupplierId = entity.SupplierId;
            am.ProductId = entity.ProductId;
            am.Quatity = entity.Quatity;
            am.CostPerProduct = entity.CostPerProduct;
            am.UnitId = entity.UnitId;
            am.EntryDate = entity.EntryDate;
            am.CreatedBy = entity.CreatedBy;
            am.CreatedDate = DateTime.Now;

            this.stockRepository.Add(am);
        }

        public IQueryable<Stock> FindBy(Expression<Func<Stock, bool>> predicate)
        {
            return stockRepository.FindBy(predicate);
        }

        public IQueryable<Stock> GetAll()
        {
            return stockRepository.GetAll().Where(x => x.IsActive == true);
        }

        public void Save()
        {
            stockRepository.Save();
        }
    }
}