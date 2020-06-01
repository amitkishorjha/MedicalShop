using WMS.Repository.Common;
using WMS.Models;
using WMS.Repository.Interface;

namespace WMS.Repository.Impl
{
    public class ClientRepository :
        GenericRepository<WMSContext, Client>, IClientRepository
    {
    }
}