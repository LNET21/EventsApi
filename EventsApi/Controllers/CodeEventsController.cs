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
using EventsApi.Core.Repositories;
using Microsoft.AspNetCore.JsonPatch;

namespace EventsApi.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class CodeEventsController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public CodeEventsController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CodeEvent>>> GetEvents(bool includeLectures)
        {
            var events = await uow.EventRepo.GetAsync(includeLectures);
            return Ok(mapper.Map<IEnumerable<CodeEventDto>>(events));
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<ActionResult<CodeEvent>> GetEvent(string name, bool includeLectures)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest();

            var result = await uow.EventRepo.GetAsync(name, includeLectures);

            if (result is null) return NotFound();

            var dto = mapper.Map<CodeEventDto>(result);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<CodeEvent>> CreateEvents(CodeEventDto dto)
        {
            if (await uow.EventRepo.GetAsync(dto.Name, false) != null)
            {
                ModelState.AddModelError("Name", "Name is in use");
                return BadRequest(ModelState);
            }

            var codeEvent = mapper.Map<CodeEvent>(dto);
            await uow.EventRepo.AddAsync(codeEvent);

            if (await uow.CompleteAsync())
            {
                var model = mapper.Map<CodeEventDto>(codeEvent);
                return CreatedAtAction(nameof(GetEvent), new { name = model.Name }, model);
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{name}")]
        public async Task<ActionResult<CodeEventDto>> PutEvent(string name, CodeEventDto dto)
        {
            var codeevent = await uow.EventRepo.GetAsync(name, false);

            if (codeevent is null) return StatusCode(StatusCodes.Status404NotFound);

            mapper.Map(dto, codeevent);

            // repo.Update(eventday);
            if (await uow.CompleteAsync())
            {
                return Ok(mapper.Map<CodeEventDto>(codeevent));
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpPatch("{name}")]
        public async Task<ActionResult<CodeEventDto>> PatchEvent(string name, JsonPatchDocument<CodeEventDto> patchDocument)
        {
            var codeEvent = await uow.EventRepo.GetAsync(name, false); 

            if (codeEvent is null) return NotFound();

            var dto = mapper.Map<CodeEventDto>(codeEvent);

            patchDocument.ApplyTo(dto, ModelState);

            if (!TryValidateModel(dto)) return BadRequest(ModelState);

            mapper.Map(dto, codeEvent);

            if (await uow.CompleteAsync())
                return Ok(mapper.Map<CodeEventDto>(codeEvent));
            else
                return StatusCode(500);
        }
    }
}
