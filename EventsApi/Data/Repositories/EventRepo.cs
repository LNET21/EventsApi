using EventsApi.Data;

namespace EventsApi.Data.Repositories
{
    public class EventRepo
    {
        private EventsApiContext context;

        public EventRepo(EventsApiContext context)
        {
            this.context = context;
        }
    }
}