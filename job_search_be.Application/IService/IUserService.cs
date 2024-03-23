using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.User;
using job_search_be.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface IUserService
    {
        PagedDataResponse<UserDto> Items(CommonListQuery commonQuery);
        DataResponse<User> Delete(Guid id);
        DataResponse<User> GetById(Guid id);
        List<Permission> GetUserPermissions(Guid id);
    }
}
