using WMS.Repository.Common;
using WMS.Models;
using WMS.Repository.Interface;

namespace WMS.Repository.Impl
{
    public class UserRoleMappingRepository :
        GenericRepository<WMSContext, UserRoleMapping>, IUserRoleMappingRepository
    {
    }
}