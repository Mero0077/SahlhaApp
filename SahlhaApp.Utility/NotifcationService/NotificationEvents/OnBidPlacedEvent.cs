using SahlhaApp.Models.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Utility.NotifcationService.NotificationEvents
{
    public class OnBidPlacedEvent
    {
       public TaskBid TaskBid { get; set; }

        public OnBidPlacedEvent(TaskBid taskBid)
        {
            TaskBid = taskBid;
        }
    }
    public delegate Task OnBidPlacedEventHandler(OnBidPlacedEvent BidPlacedEvent);
}
