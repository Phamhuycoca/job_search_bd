using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Role;
using job_search_be.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface IRoleService
    {
        PagedDataResponse<RoleDto> Items(CommonListQuery commonQuery);
        DataResponse<RoleDto> Create(RoleDto roleDto);
    }
}
