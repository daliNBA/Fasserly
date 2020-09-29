﻿using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Error;
using Fasserly.Infrastructure.Mediator.TrainingMediator;
using Fasserly.WebUI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Fasserly.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingController : BaseController
    {
        public TrainingController()
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Training>>> Get()
        {
            return await Mediator.Send(new List.Query());
            //throw new RestException(HttpStatusCode.NotFound, new { training = "Not TESTED" });

        }

        // GET api/Training/2
        [HttpGet("{id}")]
        public async Task<ActionResult<Training>> Get(Guid id)
        {
            var training = await Mediator.Send(new Detail.Query(id));
            if (training == null)
                return NotFound();

            return training;
        }

        // POST api/Training
        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await Mediator.Send(command);
        }

        // DELETE api/Training/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            var training = await Mediator.Send(new Delete.Command { Id = new Guid() });
            if (training == null)
                return NotFound();

            return training;
        }

        private Training ParserToTraining(TrainingViewModel vm)
        {
            return new Training
            {
                TrainingId = vm.TrainingId,
                Title = vm.Title,
                Description = vm.Description,
                IsActive = vm.IsActive,
                Language = vm.Language,
                Price = vm.Price,
                Rating = vm.Rating,
                UpdateDate = vm.UpdateDate,
                DateOfCreation = vm.DateOfCreation,
            };
        }
    }
}