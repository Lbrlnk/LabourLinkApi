using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Events
{
    public class ProfileCompletedEvent
    {
        public Guid UserId { get; set; }
    }
}
