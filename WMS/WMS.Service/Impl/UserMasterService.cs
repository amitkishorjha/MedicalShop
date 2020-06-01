using WMS.Service.Interface;
using WMS.Models;
using WMS.Repository.Impl;
using WMS.Repository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace WMS.Service.Impl
{
    public class UserMasterService : IUserMasterService
    {
        private readonly IUserMasterRepository userMasterRepository;

        public UserMasterService(IUserMasterRepository userMasterRepository)
        {
            this.userMasterRepository = userMasterRepository;
        }

        public void Add(UserMaster entity)
        {
            userMasterRepository.Add(entity);
        }

        public void Delete(UserMaster entity)
        {
            entity.IsActive = false;
            userMasterRepository.Edit(entity);
        }

        public void Edit(UserMaster entity)
        {
            userMasterRepository.Edit(entity);
        }

        public IQueryable<UserMaster> FindBy(Expression<Func<UserMaster, bool>> predicate)
        {
            return userMasterRepository.FindBy(predicate);
        }

        public IQueryable<UserMaster> GetAll()
        {
            return userMasterRepository.GetAll().Where(x => x.IsActive == true);
        }

        public void Save()
        {
            userMasterRepository.Save();
        }
    }
}