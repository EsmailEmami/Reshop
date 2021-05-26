using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Infrastructure.Context;

namespace Reshop.Api.Controllers.StateAndCity
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

        [HttpGet]
        public IActionResult GetCitiesOfState()
        {

            var cities = _stateService.GetCities();

            return new ObjectResult(cities);
        }
    }
}
