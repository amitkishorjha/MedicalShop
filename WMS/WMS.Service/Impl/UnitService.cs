using WMS.Service.Interface;
using WMS.Models;
using WMS.Repository.Impl;
using WMS.Repository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace WMS.Service.Impl
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository unitRepository;

        public UnitService(IUnitRepository unitRepository)
        {
            this.unitRepository = unitRepository;
        }

        public void Add(Unit entity)
        {
            unitRepository.Add(entity);
        }

        public void Delete(Unit entity)
        {
            entity.IsActive = false;
            unitRepository.Edit(entity);
        }

        public void Edit(Unit entity)
        {
            unitRepository.Edit(entity);
        }

        public IQueryable<Unit> FindBy(Expression<Func<Unit, bool>> predicate)
        {
            return unitRepository.FindBy(predicate);
        }

        public IQueryable<Unit> GetAll()
        {
            return unitRepository.GetAll().Where(x => x.IsActive == true);
        }

        public void Save()
        {
            unitRepository.Save();
        }
    }
}