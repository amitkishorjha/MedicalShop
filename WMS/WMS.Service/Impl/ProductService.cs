using WMS.Service.Interface;
using WMS.Models;
using WMS.Repository.Impl;
using WMS.Repository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace WMS.Service.Impl
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public void Add(Product entity)
        {
            productRepository.Add(entity);
        }

        public void Delete(Product entity)
        {
            entity.IsActive = false;
            productRepository.Edit(entity);
        }

        public void Edit(Product entity)
        {
            productRepository.Edit(entity);
        }

        public IQueryable<Product> FindBy(Expression<Func<Product, bool>> predicate)
        {
            return productRepository.FindBy(predicate);
        }

        public IQueryable<Product> GetAll()
        {
            return productRepository.GetAll().Where(x => x.IsActive == true);
        }

        public void Save()
        {
            productRepository.Save();
        }
    }
}