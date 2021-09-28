using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsApi.Core.Dtos
{
    public class LectureDto
    {
        public string Title { get; set; }
        public int Level { get; set; }
       
        public int SpeakerId { get; set; }
    }
}
