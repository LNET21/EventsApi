using EventsApi.Core.Entities;
using EventsApi.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventsApi.Data.Repositories
{
    public class EventRepo
    {
        private EventsApiContext db;

        public EventRepo(EventsApiContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<CodeEvent>> GetAsync()
        {
            return await db.CodeEvent.ToListAsync();
        }
    }
}