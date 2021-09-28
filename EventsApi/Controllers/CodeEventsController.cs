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
        public async Task<ActionResult<IEnumerable<CodeEvent>>> GetEvents(bool includeLectures)
        {
            var events = await eventRepo.GetAsync(includeLectures);
            return Ok(mapper.Map<IEnumerable<CodeEventDto>>(events));
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<ActionResult<CodeEvent>> GetEvent(string name, bool includeLectures)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest();

            var result = await eventRepo.GetAsync(name, includeLectures);

            if (result is null) return NotFound();

            var dto = mapper.Map<CodeEventDto>(result);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<CodeEvent>> CreateEvents(CodeEventDto dto)
        {
            if(await eventRepo.GetAsync(dto.Name, false) != null)
            {
                ModelState.AddModelError("Name", "Name is in use");
                return BadRequest();
            }

            var codeEvent = mapper.Map<CodeEvent>(dto);
            await eventRepo.AddAsync(codeEvent);

            if(await eventRepo.CompleteAsync())
            {
                var model = mapper.Map<CodeEventDto>(codeEvent);
                return CreatedAtAction(nameof(GetEvent), new { name = model.Name }, model);
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }






    }
}
