using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fasserly.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        protected readonly TrainingDataServices dataService;
        public HomeController(TrainingDataServices dataService)
        {
            this.dataService = dataService;
        }

        [HttpGet]
        public async Task<IEnumerable<Training>> Get()
        {
            var training = await dataService.GetAllTraining();
            return training;
        }
    }
}
