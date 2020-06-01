using WMS.Service.Interface;
using WMS.Models;
using WMS.Repository.Impl;
using WMS.Repository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace WMS.Service.Impl
{
    public class RoleActionMappingService : IRoleActionMappingService
    {
        private readonly IRoleActionMappingRepository roleActionMappingRepository;

        public RoleActionMappingService(IRoleActionMappingRepository roleActionMappingRepository)
        {
            this.roleActionMappingRepository = roleActionMappingRepository;
        }

        public void Add(RoleActionMapping entity)
        {
            roleActionMappingRepository.Add(entity);
        }

        public void Delete(RoleActionMapping entity)
        {
            entity.IsActive = false;
            roleActionMappingRepository.Edit(entity);
        }

        public void Edit(RoleActionMapping entity)
        {
            entity.IsActive = false;
            roleActionMappingRepository.Edit(entity);

            RoleActionMapping am = new RoleActionMapping();

            am.UniqueId = Guid.NewGuid();
            am.Id = entity.UniqueId;
            am.ActionMasterId = entity.ActionMasterId;
            am.RoleMasterId = entity.RoleMasterId;
            am.CreatedBy = entity.CreatedBy;
            am.CreatedDate = DateTime.Now;
            this.roleActionMappingRepository.Add(am);
        }

        public IQueryable<RoleActionMapping> FindBy(Expression<Func<RoleActionMapping, bool>> predicate)
        {
            return roleActionMappingRepository.FindBy(predicate);
        }

        public IQueryable<RoleActionMapping> GetAll()
        {
            return roleActionMappingRepository.GetAll().Where(x => x.IsActive == true);
        }

        public void Save()
        {
            roleActionMappingRepository.Save();
        }
    }
}