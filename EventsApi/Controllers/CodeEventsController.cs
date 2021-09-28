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

namespace EventsApi.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class CodeEventsController : ControllerBase
    {
        private readonly EventsApiContext _context;
        private readonly EventRepo eventRepo;

        public CodeEventsController(EventsApiContext context)
        {
            _context = context;
            eventRepo = new EventRepo(context);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CodeEvent>>> GetAllEvents()
        {
            var events = await eventRepo.GetAsync();
            return Ok(events);
        }


     
    }
}
