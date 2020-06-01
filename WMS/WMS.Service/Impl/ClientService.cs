using WMS.Service.Interface;
using WMS.Models;
using WMS.Repository.Impl;
using WMS.Repository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace WMS.Service.Impl
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public void Add(Client entity)
        {
            clientRepository.Add(entity);
        }

        public void Delete(Client entity)
        {
            entity.IsActive = false;
            clientRepository.Edit(entity);
        }

        public void Edit(Client entity)
        {
            clientRepository.Edit(entity);
        }

        public IQueryable<Client> FindBy(Expression<Func<Client, bool>> predicate)
        {
            return clientRepository.FindBy(predicate);
        }

        public IQueryable<Client> GetAll()
        {
            return clientRepository.GetAll().Where(x => x.IsActive == true);
        }

        public void Save()
        {
            clientRepository.Save();
        }
    }
}