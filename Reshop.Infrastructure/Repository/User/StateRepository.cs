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
        public IAsyncEnumerable<State> GetStates() => _context.States;

        public async Task AddStateAsync(State state) => await _context.States.AddAsync(state);

        public void RemoveState(State state) => _context.States.Remove(state);

        public void UpdateState(State state) => _context.States.Update(state);

        public async Task<State> GetStateByIdAsync(int stateId)
            =>
                await _context.States.FindAsync(stateId);

        #endregion

        #region city

        public IAsyncEnumerable<City> GetCities() => _context.Cities;

        public async Task AddCityAsync(City city) => await _context.Cities.AddAsync(city);

        public void RemoveCity(City city) => _context.Cities.Remove(city);

        public void UpdateCity(City city) => _context.Cities.Update(city);

        public async Task<City> GetCityByIdAsync(int cityId)
            =>
                await _context.Cities.FindAsync(cityId);

        #endregion

        #region stateCity

        public async Task AddStateCityAsync(StateCity stateCity)
            =>
                await _context.StateCities.AddAsync(stateCity);

        public void RemoveStateCity(StateCity stateCity)
            =>
                _context.StateCities.Remove(stateCity);

        public void UpdateStateCity(StateCity stateCity)
            =>
                _context.StateCities.Update(stateCity);

        public IAsyncEnumerable<City> GetCitiesOfState(int stateId)
            =>
                _context.StateCities.Where(c => c.StateId == stateId)
                    .Select(c => c.City) as IAsyncEnumerable<City>;

        public IAsyncEnumerable<StateCity> GetStateCitiesByStateId(int stateId)
            =>
                _context.StateCities.Where(c => c.StateId == stateId) as IAsyncEnumerable<StateCity>;

        public IAsyncEnumerable<StateCity> GetStateCitiesByCityId(int cityId)
            =>
                _context.StateCities.Where(c => c.CityId == cityId) as IAsyncEnumerable<StateCity>;

        public IAsyncEnumerable<int> GetCitiesIdOfState(int stateId)
            =>
                _context.StateCities.Where(c => c.StateId == stateId)
                    .Select(c => c.CityId) as IAsyncEnumerable<int>;

        #endregion

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}