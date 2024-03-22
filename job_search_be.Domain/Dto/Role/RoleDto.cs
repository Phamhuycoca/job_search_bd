using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Role
{
    public class RoleDto:BaseEntity
    {
        public Guid RoleId { get; set; }
        public string? NameRole { get; set; }
    }
}
