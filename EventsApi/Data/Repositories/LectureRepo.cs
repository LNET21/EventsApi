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
    }
}
