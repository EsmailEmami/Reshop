using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.User;
using Reshop.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Infrastructure.Repository.User
{
    public class StateRepository : IStateRepository
    {
        #region constructor

        private readonly ReshopDbContext _context;

        public StateRepository(ReshopDbContext context)
        {
            _context = context;
        }

        #endregion

        #region state
        public IEnumerable<State> GetStates() =>
            _context.States;

        public async Task AddStateAsync(State state) => await _context.States.AddAsync(state);

        public void RemoveState(State state) => _context.States.Remove(state);

        public void UpdateState(State state) => _context.States.Update(state);

        public async Task<State> GetStateByIdAsync(int stateId)
            =>
                await _context.States.FindAsync(stateId);

        public string GetStateNameById(int stateId)
            =>
                _context.States.SingleOrDefault(c => c.StateId == stateId)?.StateName;

        #endregion

        #region city

        public IEnumerable<City> GetCities() => _context.Cities;

        public async Task AddCityAsync(City city) => await _context.Cities.AddAsync(city);

        public void RemoveCity(City city) => _context.Cities.Remove(city);

        public void UpdateCity(City city) => _context.Cities.Update(city);

        public async Task<City> GetCityByIdAsync(int cityId)
            =>
                await _context.Cities.FindAsync(cityId);

        public string GetCityNameById(int cityId)
            =>
                _context.Cities.SingleOrDefault(c => c.CityId == cityId)?.CityName;

        #endregion

     
        public IEnumerable<City> GetCitiesOfState(int stateId)
            =>
                _context.Cities.Where(c=> c.StateId == stateId);

    

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}