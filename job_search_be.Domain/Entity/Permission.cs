using job_search_be.Domain.BaseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Entity
{
    public sealed class Permission : BaseEntity
    {
        public Guid PermissionId { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }
        public ICollection<Role> Roles { get; set; }

    }
}
