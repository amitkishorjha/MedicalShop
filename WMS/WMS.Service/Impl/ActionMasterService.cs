using WMS.Service.Interface;
using WMS.Models;
using WMS.Repository.Impl;
using WMS.Repository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace WMS.Service.Impl
{
    public class ActionMasterService : IActionMasterService
    {
        private readonly IActionMasterRepository ActionMasterRepository;

        public ActionMasterService(IActionMasterRepository ActionMasterRepository)
        {
            this.ActionMasterRepository = ActionMasterRepository;
        }

        public void Add(ActionMaster entity)
        {
            ActionMasterRepository.Add(entity);
        }

        public void Delete(ActionMaster entity)
        {
            entity.IsActive = false;
            ActionMasterRepository.Edit(entity);
        }

        public void Edit(ActionMaster entity)
        {
            ActionMasterRepository.Edit(entity);
        }

        public IQueryable<ActionMaster> FindBy(Expression<Func<ActionMaster, bool>> predicate)
        {
            return ActionMasterRepository.FindBy(predicate);
        }

        public IQueryable<ActionMaster> GetAll()
        {
            return ActionMasterRepository.GetAll().Where(x => x.IsActive == true);
        }

        public void Save()
        {
            ActionMasterRepository.Save();
        }
    }
}