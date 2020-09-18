using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.WebUI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fasserly.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        protected readonly TrainingDataServices dataService;
        protected readonly UserDataServices userdataService;

        public HomeController(TrainingDataServices dataService, UserDataServices userdataService)
        {
            this.dataService = dataService;
            this.userdataService = userdataService;
        }

        [HttpGet]
        public async Task<IEnumerable<TrainingViewModel>> Get()
        {
            var result = new List<TrainingViewModel>();
            var trainings = await dataService.GetAllTraining();
            var user = await userdataService.GetUserByEmail("dali.mahdoui@gmail.com", "Pa$$W0rd");
            foreach (var training in trainings)
                result.Add(new TrainingViewModel(training));
            return result;
        }

        // GET api/Training/2
        [HttpGet("{id}")]   
        public async Task<ActionResult<Training>> Get(Guid id)
        {
            var training = await dataService.GetTrainingById(id);
            if (training == null)
                return NotFound();
            return training;
        }

        // POST api/Training
        [HttpPost]
        public async Task<ActionResult<Unit>> Create(TrainingViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await dataService.CreateTraining(ParserToTraining(vm));
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
