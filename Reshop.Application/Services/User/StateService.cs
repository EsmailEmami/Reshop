using Reshop.Application.Enums;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Application.Services.User
{
    public class StateService : IStateService
    {
        #region constructor

        private readonly IStateRepository _stateRepository;

        public StateService(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        #endregion


        public IAsyncEnumerable<State> GetStates() => _stateRepository.GetStates();

        public async Task<ResultTypes> AddStateAsync(State state)
        {
            try
            {
                await _stateRepository.AddStateAsync(state);
                await _stateRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> RemoveStateAsync(int stateId)
        {
            var state = await _stateRepository.GetStateWithCitiesByIdAsync(stateId);
            if (state is null) return ResultTypes.Failed;

            try
            {
                if (state.Cities is not null)
                {
                    foreach (var city in state.Cities)
                    {
                        _stateRepository.RemoveCity(city);
                    }

                    await _stateRepository.SaveChangesAsync();
                }

                _stateRepository.RemoveState(state);
                await _stateRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditStateAsync(State state)
        {
            try
            {
                _stateRepository.UpdateState(state);
                await _stateRepository.SaveChangesAsync();
                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<State> GetStateWithCitiesByIdAsync(int stateId)
            =>
                await _stateRepository.GetStateWithCitiesByIdAsync(stateId);

        public IAsyncEnumerable<City> GetCities() => _stateRepository.GetCities();

        public async Task<ResultTypes> AddCityAsync(City city)
        {
            try
            {
                await _stateRepository.AddCityAsync(city);
                await _stateRepository.SaveChangesAsync();
                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> RemoveCityAsync(City city)
        {
            try
            {
                _stateRepository.RemoveCity(city);
                await _stateRepository.SaveChangesAsync();
                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> UpdateCityAsync(City city)
        {
            try
            {
                _stateRepository.RemoveCity(city);
                await _stateRepository.SaveChangesAsync();
                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<City> GetCityByIdAsync(int cityId)
            =>
                await _stateRepository.GetCityByIdAsync(cityId);
    }
}