using WMS.Service.Interface;
using WMS.Models;
using WMS.Repository.Impl;
using WMS.Repository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace WMS.Service.Impl
{
    public class OrderExitService : IOrderExitService
    {
        private readonly IOrderExitRepository orderExitRepository;

        public OrderExitService(IOrderExitRepository orderExitRepository)
        {
            this.orderExitRepository = orderExitRepository;
        }

        public void Add(OrderExit entity)
        {
            orderExitRepository.Add(entity);
        }

        public void Delete(OrderExit entity)
        {
            entity.IsActive = false;
            orderExitRepository.Edit(entity);
        }

        public void Edit(OrderExit entity)
        {
            entity.IsActive = false;
            orderExitRepository.Edit(entity);

            OrderExit am = new OrderExit();

            am.UniqueId = Guid.NewGuid();
            am.Id = entity.UniqueId;
            am.ClientId = entity.ClientId;
            am.ProductId = entity.ProductId;
            am.Quatity = entity.Quatity;
            am.Cost = entity.Cost;
            am.ExitDate = entity.ExitDate;
            am.CreatedBy = entity.CreatedBy;
            am.CreatedDate = DateTime.Now;

            this.orderExitRepository.Add(am);
        }

        public IQueryable<OrderExit> FindBy(Expression<Func<OrderExit, bool>> predicate)
        {
            return orderExitRepository.FindBy(predicate);
        }

        public IQueryable<OrderExit> GetAll()
        {
            return orderExitRepository.GetAll().Where(x => x.IsActive == true);
        }

        public void Save()
        {
            orderExitRepository.Save();
        }
    }
}