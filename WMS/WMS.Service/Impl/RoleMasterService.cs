using WMS.Service.Interface;
using WMS.Models;
using WMS.Repository.Impl;
using WMS.Repository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace WMS.Service.Impl
{
    public class RoleMasterService : IRoleMasterService
    {
        private readonly IRoleMasterRepository roleMasterRepository;

        public RoleMasterService(IRoleMasterRepository roleMasterRepository)
        {
            this.roleMasterRepository = roleMasterRepository;
        }

        public void Add(RoleMaster entity)
        {
            roleMasterRepository.Add(entity);
        }

        public void Delete(RoleMaster entity)
        {
            entity.IsActive = false;
            roleMasterRepository.Edit(entity);
        }

        public void Edit(RoleMaster entity)
        {
            roleMasterRepository.Edit(entity);
        }

        public IQueryable<RoleMaster> FindBy(Expression<Func<RoleMaster, bool>> predicate)
        {
            return roleMasterRepository.FindBy(predicate);
        }

        public IQueryable<RoleMaster> GetAll()
        {
            return roleMasterRepository.GetAll().Where(x => x.IsActive == true);
        }

        public void Save()
        {
            roleMasterRepository.Save();
        }
    }
}