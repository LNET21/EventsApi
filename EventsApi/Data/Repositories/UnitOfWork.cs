using EventsApi.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsApi.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventsApiContext db;

        public EventRepo EventRepo { get; }
        public LectureRepo LecturesRepo { get; }

        public UnitOfWork(EventsApiContext db)
        {
            this.db = db;
            EventRepo = new EventRepo(db);
            LecturesRepo = new LectureRepo(db);
        }

        public async Task<bool> CompleteAsync()
        {
            return (await db.SaveChangesAsync()) >= 0;
        }
    }
}
