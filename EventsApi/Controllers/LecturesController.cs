using AutoMapper;
using EventsApi.Core.Dtos;
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

        [HttpPost]
        public async Task<ActionResult<LectureDto>> Create(string name)
        {
            return null;
        }


    }
}
