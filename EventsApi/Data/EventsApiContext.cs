using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventsApi.Core.Entities;

namespace EventsApi.Data
{
    public class EventsApiContext : DbContext
    {
        public EventsApiContext (DbContextOptions<EventsApiContext> options)
            : base(options)
        {
        }

        public DbSet<CodeEvent> CodeEvent { get; set; }
    }
}
