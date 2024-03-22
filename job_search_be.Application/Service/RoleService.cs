using AutoMapper;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Role;
using job_search_be.Domain.Dto.User;
using job_search_be.Domain.Entity;
using job_search_be.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public DataResponse<RoleDto> Create(RoleDto roleDto)
        {
            throw new NotImplementedException();
        }

        public PagedDataResponse<RoleDto> Items(CommonListQuery commonQuery)
        {
            var query = _mapper.Map< List<RoleDto>>(_roleRepository.GetAllData());
            if (!string.IsNullOrEmpty(commonQuery.keyword))
            {
                query = query.Where(x => x.NameRole.Contains(commonQuery.keyword)).ToList();
            }
            var paginatedResult = PaginatedList<RoleDto>.ToPageList(query, commonQuery.page, commonQuery.limit);
            return new PagedDataResponse<RoleDto>(paginatedResult, 200, paginatedResult.Count());
        }
    }
}
