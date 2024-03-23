using AutoMapper;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Abstract;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Role;
using job_search_be.Domain.Dto.User;
using job_search_be.Domain.Entity;
using job_search_be.Domain.Repositories;
using job_search_be.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRoleRepository _roleRepository;
        public UserService(IUserRepository userRepository, IMapper mapper, IPermissionRepository permissionRepository,IRoleRepository roleRepository)
        {

            _userRepository = userRepository;
            _mapper = mapper;
            _permissionRepository = permissionRepository;
            _roleRepository = roleRepository;
        }

        public DataResponse<User> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public DataResponse<User> GetById(Guid id)
        {
            var user = _userRepository.Delete(id);
            if (user != null)
            {
                return new DataResponse<User>(user, 200, "Success");
            }
            throw new ApiException(400, "Lỗi không thể xóa");
        }

        public List<Permission> GetUserPermissions(Guid id)
        {
            var user = _userRepository.GetById(id);
            var role = _mapper.Map<RoleDto>(_roleRepository.GetById(user.RoleId ?? Guid.Empty));
            var permissions = _permissionRepository.GetAllData().Where(e=>e.PermissionId==role.PermissionId).ToList();
            return permissions;
        }
        public PagedDataResponse<UserDto> Items(CommonListQuery commonQuery)
        {
            var query = _mapper.Map<List<UserDto>>(_userRepository.GetAllData());
            if (!string.IsNullOrEmpty(commonQuery.keyword))
            {
                query = query.Where(x => x.FullName.Contains(commonQuery.keyword)).ToList();
            }
            var paginatedResult = PaginatedList<UserDto>.ToPageList(query, commonQuery.page, commonQuery.limit);
            return new PagedDataResponse<UserDto>(paginatedResult, 200, paginatedResult.Count());
        }

      
    }
}
