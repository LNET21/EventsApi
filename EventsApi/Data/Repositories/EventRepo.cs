using EventsApi.Core.Entities;
using EventsApi.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<CodeEvent> GetAsync(string name, bool includeLectures)
        {
            //ToDo nullcheck name

            var query = db.CodeEvent
                          .Include(e => e.Location)
                          .AsQueryable();

            if (includeLectures)
            {
                query = query.Include(e => e.Lectures);
            }

            return await query.FirstOrDefaultAsync(e => e.Name == name);

        }

        public async Task AddAsync(CodeEvent codeEvent)
        {
            await db.AddAsync(codeEvent);
        }

        public async Task<bool> CompleteAsync()
        {
            return (await db.SaveChangesAsync()) >= 0;
        }
    }
}