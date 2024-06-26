﻿using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Entity
{
    public class Role : BaseEntity
    {
        public Guid RoleId { get; set; }
        public string? NameRole { get; set; }
        public virtual ICollection<User>? Users { get; set; }
        public Guid? PermissionId { get; set; }
        public Permission? Permission { get; set; }

    }
}
