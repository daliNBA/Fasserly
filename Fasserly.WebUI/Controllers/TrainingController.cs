﻿using Fasserly.Infrastructure.Mediator.TrainingMediator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fasserly.WebUI.Controllers
{
    [AllowAnonymous]
    public class TrainingController : BaseController
    {
        public TrainingController()
        {
        }

        [HttpGet]
        public async Task<ActionResult<List.TrainingEnvelope>> Get(int? limit, int? offset, bool isBuyer, bool isOwner, DateTime? startDate)
        {
            return await Mediator.Send(new List.Query(limit, offset, isBuyer, isOwner, startDate));
        }

        // GET api/Training/2C:\Users\Asus\source\repos\Fasserly\Fasserly.WebUI\Controllers\TrainingController.cs
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingDto>> Get(Guid id)
        {
            return await Mediator.Send(new Details.Query(id));
        }

        // POST api/Training
        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await Mediator.Send(command);
        }

        //Buy command
        [HttpPost("{id}/buy")]
        public async Task<ActionResult<Unit>> Buy(Guid id)
        {
            return await Mediator.Send(new Buy.Command { Id = id });
        }

        //Buy command
        [HttpDelete("{id}/buy")]
        public async Task<ActionResult<Unit>> Refund(Guid id)
        {
            return await Mediator.Send(new Refund.Command { Id = id });
        }

        // PUT api/Training/5
        [HttpPut("{id}")]
        //[Authorize(Policy = "IsTrainingOwner")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command command)
        {
            command.TrainingId = id;
            return await Mediator.Send(command);
        }

        // DELETE api/Training/5
        [HttpDelete("{id}")]
        //[Authorize(Policy = "IsTrainingOwner")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new Delete.Command { Id = id });
        }
    }
}
