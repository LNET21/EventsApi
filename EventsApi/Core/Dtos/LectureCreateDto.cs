using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsApi.Core.Dtos
{
    public class LectureCreateDto
    {
        public string Title { get; set; }
        public int Level { get; set; }
    }
}
