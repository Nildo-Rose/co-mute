using System;
using System.Collections.Generic;

#nullable disable

namespace co_mute_master.Models
{
    public partial class Register
    {
        public Register()
        {
            CarPools = new HashSet<CarPool>();
            JoinLeaveOpps = new HashSet<JoinLeaveOpp>();
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<CarPool> CarPools { get; set; }
        public virtual ICollection<JoinLeaveOpp> JoinLeaveOpps { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
