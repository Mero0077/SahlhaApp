using SahlhaApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Utility.NotifcationService.NotificationEvents
{
    public class OnTaskAssignedEvent
    {
       public TaskAssignment TaskAssignment { get; set; }

        public OnTaskAssignedEvent(TaskAssignment taskAssignment)
        {
            TaskAssignment = taskAssignment;
        }
    }

    public delegate Task OnTaskAssignedEventHandler(OnTaskAssignedEvent onTaskAssignedEvent);
}
