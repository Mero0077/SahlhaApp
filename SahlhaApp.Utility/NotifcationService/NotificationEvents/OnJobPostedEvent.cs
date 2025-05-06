using SahlhaApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Utility.NotifcationService.NotificationEvents
{
    public class OnJobPostedEvent
    {
        public Job Job { get; }

        public OnJobPostedEvent(Job job)
        {
            Job = job;
        }
    }

    public delegate Task OnJobPostedEventHandler(OnJobPostedEvent jobPostedEvent);
}
