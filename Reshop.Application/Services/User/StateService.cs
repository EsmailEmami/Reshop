using Reshop.Application.Enums;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.User;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.User;

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
            var state = await _stateRepository.GetStateByIdAsync(stateId);
            if (state is null) return ResultTypes.Failed;

            try
            {
                var stateCities = _stateRepository.GetStateCitiesByStateId(state.StateId);

                if (stateCities is not null)
                {
                    await foreach (var stateCity in stateCities)
                    {
                        _stateRepository.RemoveStateCity(stateCity);
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

        public async Task<AddOrEditStateViewModel> GetStateDataForEditAsync(int stateId)
        {
            var state = await _stateRepository.GetStateByIdAsync(stateId);
            if (state == null) return null;

            var stateCitiesId = _stateRepository.GetCitiesIdOfState(stateId);

            return new AddOrEditStateViewModel()
            {
                StateId = state.StateId,
                StateName = state.StateName,
                SelectedCities = stateCitiesId as IEnumerable<int>,
                Cities = _stateRepository.GetCities() as IEnumerable<City>
            };
        }

        public async Task<State> GetStateByIdAsync(int stateId)
            =>
                await _stateRepository.GetStateByIdAsync(stateId);

        public string GetStateNameById(int stateId)
            =>
                _stateRepository.GetStateNameById(stateId);

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

        public async Task<ResultTypes> RemoveCityAsync(int cityId)
        {
            var city = await _stateRepository.GetCityByIdAsync(cityId);

            if (city is null) return ResultTypes.Failed;

            try
            {
                var statesOfCity = _stateRepository.GetStateCitiesByCityId(city.CityId);

                if (statesOfCity is not null)
                {
                    await foreach (var stateCity in statesOfCity)
                    {
                        _stateRepository.RemoveStateCity(stateCity);
                    }

                    await _stateRepository.SaveChangesAsync();
                }

                _stateRepository.RemoveCity(city);
                await _stateRepository.SaveChangesAsync();
                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditCityAsync(City city)
        {
            try
            {
                _stateRepository.UpdateCity(city);
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

        public IAsyncEnumerable<City> GetCitiesOfState(int stateId)
            =>
                _stateRepository.GetCitiesOfState(stateId);

        public string GetCityNameById(int cityId)
            =>
                _stateRepository.GetCityNameById(cityId);

        public async Task RemoveCitiesOfStateAsync(int stateId)
        {
            var stateCities = _stateRepository.GetStateCitiesByStateId(stateId);


            if (stateCities is not null)
            {
                await foreach (var stateCity in stateCities)
                {
                    _stateRepository.RemoveStateCity(stateCity);
                }

                await _stateRepository.SaveChangesAsync();
            }
        }

        public async Task AddStateCityAsync(StateCity stateCity)
        {
            await _stateRepository.AddStateCityAsync(stateCity);
            await _stateRepository.SaveChangesAsync();
        }
    }
}