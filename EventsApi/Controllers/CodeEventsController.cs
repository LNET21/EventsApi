using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventsApi.Core.Entities;
using EventsApi.Data;
using EventsApi.Data.Repositories;
using AutoMapper;
using EventsApi.Core.Dtos;

namespace EventsApi.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class CodeEventsController : ControllerBase
    {
        private readonly EventsApiContext _context;
        private readonly IMapper mapper;
        private readonly EventRepo eventRepo;

        public CodeEventsController(EventsApiContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            eventRepo = new EventRepo(context);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CodeEvent>>> GetAllEvents(bool includeLectures)
        {
            var events = await eventRepo.GetAsync(includeLectures);
            return Ok(mapper.Map<IEnumerable<CodeEventDto>>(events));
        }


     
    }
}
