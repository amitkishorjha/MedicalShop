using WMS.Service.Interface;
using WMS.Models;
using WMS.Repository.Impl;
using WMS.Repository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace WMS.Service.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public void Add(Category entity)
        {
            categoryRepository.Add(entity);
        }

        public void Delete(Category entity)
        {
            entity.IsActive = false;
            categoryRepository.Edit(entity);
        }

        public void Edit(Category entity)
        {
            categoryRepository.Edit(entity);
        }

        public IQueryable<Category> FindBy(Expression<Func<Category, bool>> predicate)
        {
            return categoryRepository.FindBy(predicate);
        }

        public IQueryable<Category> GetAll()
        {
            return categoryRepository.GetAll().Where(x => x.IsActive == true);
        }

        public void Save()
        {
            categoryRepository.Save();
        }
    }
}