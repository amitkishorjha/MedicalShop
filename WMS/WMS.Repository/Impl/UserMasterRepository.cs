using WMS.Repository.Common;
using WMS.Models;
using WMS.Repository.Interface;

namespace WMS.Repository.Impl
{
    public class UserMasterRepository :
        GenericRepository<WMSContext, UserMaster>, IUserMasterRepository
    {
    }
}