using AutoMapper;
using EventsApi.Core.Dtos;
using EventsApi.Core.Entities;
using EventsApi.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsApi.Controllers
{
    [Route("api/events/{name}/lectures")]
    [ApiController]
    public class LecturesController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public LecturesController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LectureDto>>> Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest();

            var lectures = await uow.LecturesRepo.GetLectureForEvent(name);
            return Ok(mapper.Map<IEnumerable<LectureDto>>(lectures));
        }


        [HttpGet("{id}")]
       // [Route("{id}")]
        public async Task<ActionResult<LectureDto>> GetLecture(string name, int? id)
        {
            if (string.IsNullOrWhiteSpace(name) || id is null) return BadRequest();
            var lecture = await uow.LecturesRepo.GetLectureAsync(id);
            return Ok(mapper.Map<LectureDto>(lecture));
        }

       [HttpPost]
        public async Task<ActionResult<LectureDto>> Create(string name, LectureCreateDto dto)
        {
            var codeEvent = await uow.EventRepo.GetAsync(name, false);

            if (codeEvent is null) return NotFound();

            var lecture = mapper.Map<Lecture>(dto);
            lecture.CodeEvent = codeEvent;
            await uow.LecturesRepo.AddAsync(lecture);

            if (await uow.CompleteAsync())
            {
                var model = mapper.Map<LectureDto>(lecture);
                return CreatedAtAction(nameof(GetLecture), new { name = codeEvent.Name, id = model.Id }, model);
            }
            else
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }


    }
}
