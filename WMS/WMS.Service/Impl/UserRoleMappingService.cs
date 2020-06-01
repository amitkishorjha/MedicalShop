using WMS.Service.Interface;
using WMS.Models;
using WMS.Repository.Impl;
using WMS.Repository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace WMS.Service.Impl
{
    public class UserRoleMappingService : IUserRoleMappingService
    {
        private readonly IUserRoleMappingRepository userRoleMappingRepository;

        public UserRoleMappingService(IUserRoleMappingRepository userRoleMappingRepository)
        {
            this.userRoleMappingRepository = userRoleMappingRepository;
        }

        public void Add(UserRoleMapping entity)
        {
            userRoleMappingRepository.Add(entity);
        }

        public void Delete(UserRoleMapping entity)
        {
            entity.IsActive = false;
            userRoleMappingRepository.Edit(entity);
        }

        public void Edit(UserRoleMapping entity)
        {
            entity.IsActive = false;
            userRoleMappingRepository.Edit(entity);

            UserRoleMapping am = new UserRoleMapping();

            am.UniqueId = Guid.NewGuid();
            am.Id = entity.UniqueId;
            am.UserMasterId = entity.UserMasterId;
            am.RoleMasterId = entity.RoleMasterId;
            am.CreatedBy = entity.CreatedBy;
            am.CreatedDate = DateTime.Now;
            this.userRoleMappingRepository.Add(am);
        }

        public IQueryable<UserRoleMapping> FindBy(Expression<Func<UserRoleMapping, bool>> predicate)
        {
            return userRoleMappingRepository.FindBy(predicate);
        }

        public IQueryable<UserRoleMapping> GetAll()
        {
            return userRoleMappingRepository.GetAll().Where(x => x.IsActive == true);
        }

        public void Save()
        {
            userRoleMappingRepository.Save();
        }
    }
}