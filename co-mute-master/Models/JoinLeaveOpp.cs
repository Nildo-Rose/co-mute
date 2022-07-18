using System;
using System.Collections.Generic;

#nullable disable

namespace co_mute_master.Models
{
    public partial class JoinLeaveOpp
    {
        public int Id { get; set; }
        public int? CarOppId { get; set; }
        public int? RegId { get; set; }
        public bool? LeaveJoine { get; set; }
        public DateTime? DateJoined { get; set; }

        public virtual CarPool CarOpp { get; set; }
        public virtual Register Reg { get; set; }
    }
}
