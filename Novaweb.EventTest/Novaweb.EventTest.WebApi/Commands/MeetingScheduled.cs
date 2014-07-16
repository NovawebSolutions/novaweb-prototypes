using NEventStore;

namespace Novaweb.EventTest.WebApi.Commands
{
    public class MeetingScheduled :EventMessage
    {
        public string MeetingName { get; set; }
    }
}