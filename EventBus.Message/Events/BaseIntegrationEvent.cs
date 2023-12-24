using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.Events
{
    public class BaseIntegrationEvent
    {
        //Co-Relation Id
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public BaseIntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }
        public BaseIntegrationEvent(Guid id , DateTime creationDate)
        {
                Id = id;
            CreationDate = creationDate;
        }
                
    }
}
