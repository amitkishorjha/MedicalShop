using WMS.Service.Interface;
using WMS.Models;
using WMS.Repository.Impl;
using WMS.Repository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace WMS.Service.Impl
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            this.supplierRepository = supplierRepository;
        }

        public void Add(Supplier entity)
        {
            supplierRepository.Add(entity);
        }

        public void Delete(Supplier entity)
        {
            entity.IsActive = false;
            supplierRepository.Edit(entity);
        }

        public void Edit(Supplier entity)
        {
            supplierRepository.Edit(entity);
        }

        public IQueryable<Supplier> FindBy(Expression<Func<Supplier, bool>> predicate)
        {
            return supplierRepository.FindBy(predicate);
        }

        public IQueryable<Supplier> GetAll()
        {
            return supplierRepository.GetAll().Where(x => x.IsActive == true);
        }

        public void Save()
        {
            supplierRepository.Save();
        }
    }
}