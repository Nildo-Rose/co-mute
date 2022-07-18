using System;
using System.Collections.Generic;

#nullable disable

namespace co_mute_master.Models
{
    public partial class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual Register User { get; set; }
    }
}
