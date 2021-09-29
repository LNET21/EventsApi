using EventsApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsApi.Data.Repositories
{
    public class LectureRepo
    {
        private readonly EventsApiContext db;

        public LectureRepo(EventsApiContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Lecture>> GetLectureForEvent(string name)
        {
            return await db.Lecture.Where(l => l.CodeEvent.Name == name).ToListAsync();
        }

        public async Task AddAsync(Lecture lecture)
        {
            await db.AddAsync(lecture);
        }

        public async Task<Lecture> GetLectureAsync(int? id)
        {
            return await db.Lecture.FindAsync(id);
        }
    }
}
