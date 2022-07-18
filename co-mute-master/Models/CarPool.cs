using System;
using System.Collections.Generic;

#nullable disable

namespace co_mute_master.Models
{
    public partial class CarPool
    {
        public CarPool()
        {
            JoinLeaveOpps = new HashSet<JoinLeaveOpp>();
        }

        public int Id { get; set; }
        public TimeSpan? ExpectedArrialTime { get; set; }
        public TimeSpan? Departure { get; set; }
        public string Origins { get; set; }
        public string Destination { get; set; }
        public string AvailableSeats { get; set; }
        public string Owner { get; set; }
        public string Notes { get; set; }
        public string Rates { get; set; }
        public int? UserId { get; set; }

        public virtual Register User { get; set; }
        public virtual ICollection<JoinLeaveOpp> JoinLeaveOpps { get; set; }
    }
}
