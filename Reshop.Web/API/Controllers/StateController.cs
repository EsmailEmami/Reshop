using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Application.Interfaces.User;

namespace Reshop.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;

        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }

        // https://localhost:44312/api/State?stateId=5
        [HttpGet]
        public IActionResult GetCitiesOfState(int stateId = 5)
        {

            var cities = _stateService.GetCitiesOfState(stateId);

            return new ObjectResult(cities);
        }
    }
}
