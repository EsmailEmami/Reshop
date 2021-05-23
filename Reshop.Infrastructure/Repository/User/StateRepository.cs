using Microsoft.EntityFrameworkCore;
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

        public async Task<State> GetStateWithCitiesByIdAsync(int stateId)
            =>
                await _context.States.Where(c => c.StateId == stateId)
                    .Include(c => c.Cities).SingleOrDefaultAsync();

        #endregion

        #region city

        public IAsyncEnumerable<City> GetCities() => _context.Cities;

        public async Task AddCityAsync(City city) => await _context.Cities.AddAsync(city);

        public void RemoveCity(City city) => _context.Cities.Remove(city);

        public void UpdateCity(City city) => _context.Cities.Update(city);
        public IAsyncEnumerable<City> GetCitiesOfState(int stateId)
        {
            return _context.States.Where(c => c.StateId == stateId)
                .Select(c => c.Cities).SingleOrDefaultAsync() as IAsyncEnumerable<City>;
        }

        public async Task<City> GetCityByIdAsync(int cityId)
            =>
                await _context.Cities.FindAsync(cityId);

        #endregion

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}