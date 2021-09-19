using System;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.User;
using Reshop.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Reshop.Infrastructure.Repository.User
{
    public class OriginRepository : IOriginRepository
    {
        #region constructor

        private readonly ReshopDbContext _context;

        public OriginRepository(ReshopDbContext context)
        {
            _context = context;
        }

        #endregion

        #region state
        public IEnumerable<Tuple<int, string>> GetStates() =>
            _context.States.Select(c => new Tuple<int, string>(c.StateId, c.StateName));

        public IEnumerable<Tuple<int, string>> GetStatesWithPagination(int skip, int take, string filter)
        {
            IQueryable<State> states = _context.States;

            if (!string.IsNullOrEmpty(filter))
            {
                states = states.Where(c => c.StateName.Contains(filter));
            }


            states = states.Skip(skip).Take(take);

            return states.Select(c => new Tuple<int, string>(c.StateId, c.StateName));
        }


        public async Task AddStateAsync(State state) => await _context.States.AddAsync(state);

        public void RemoveState(State state) => _context.States.Remove(state);

        public void UpdateState(State state) => _context.States.Update(state);

        public async Task<State> GetStateByIdAsync(int stateId)
            =>
                await _context.States.FindAsync(stateId);

        public string GetStateNameById(int stateId)
            =>
                _context.States.SingleOrDefault(c => c.StateId == stateId)?.StateName;

        public async Task<int> GetStatesCountAsync() =>
            await _context.States.CountAsync();

        #endregion

        #region city

        public IEnumerable<City> GetCities() => _context.Cities;
        public IEnumerable<Tuple<int, string>> GetCitiesWithPagination(int skip, int take, string filter = "", List<int> states = null)
        {
            IQueryable<City> cities;

            if (states != null && states.Count > 0)
            {
                string idsOfState = "";

                int lastState = states.Last();

                foreach (var state in states)
                {
                    if (state == lastState)
                    {
                        idsOfState += state.ToString();
                    }
                    else
                    {
                        idsOfState += state + ",";
                    }
                }

                cities = _context.Cities
                    .FromSqlRaw($"SELECT * FROM dbo.Cities WHERE StateId IN ({idsOfState})");
            }
            else
            {
                cities = _context.Cities;
            }

            if (!string.IsNullOrEmpty(filter))
            {
                cities = cities.Where(c => c.CityName.Contains(filter));
            }

            cities = cities.Skip(skip).Take(take);

            return cities.Select(c => new Tuple<int, string>(c.StateId, $"{c.CityName} ({c.State.StateName})"));
        }

        public async Task AddCityAsync(City city) => await _context.Cities.AddAsync(city);

        public void RemoveCity(City city) => _context.Cities.Remove(city);

        public void UpdateCity(City city) => _context.Cities.Update(city);

        public async Task<City> GetCityByIdAsync(int cityId)
            =>
                await _context.Cities.FindAsync(cityId);

        public string GetCityNameById(int cityId)
            =>
                _context.Cities.SingleOrDefault(c => c.CityId == cityId)?.CityName;

        public async Task<int> GetCitiesCountAsync() =>
            await _context.Cities.CountAsync();

        #endregion


        public IEnumerable<City> GetCitiesOfState(int stateId)
            =>
                _context.Cities.Where(c => c.StateId == stateId);



        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}