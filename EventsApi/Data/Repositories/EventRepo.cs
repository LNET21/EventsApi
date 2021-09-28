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

        public async Task<IEnumerable<CodeEvent>> GetAsync(bool includeLectures)
        {
            return includeLectures ? await db.CodeEvent
                .Include(e => e.Location)
                .Include(e => e.Lectures)
                .ToListAsync() :
                await db.CodeEvent
                .Include(e => e.Location)
                .ToListAsync();
        }
    }
}