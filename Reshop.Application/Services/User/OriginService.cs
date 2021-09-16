using Reshop.Application.Enums;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.User;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.User;

namespace Reshop.Application.Services.User
{
    public class OriginService : IOriginService
    {
        #region constructor

        private readonly IOriginRepository _stateRepository;

        public OriginService(IOriginRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        #endregion


        public IEnumerable<State> GetStates() =>
            _stateRepository.GetStates();

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
            if (state == null) return ResultTypes.Failed;

            try
            {
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

        public async Task<State> GetStateByIdAsync(int stateId)
            =>
                await _stateRepository.GetStateByIdAsync(stateId);

        public string GetStateNameById(int stateId)
            =>
                _stateRepository.GetStateNameById(stateId);

        public IEnumerable<City> GetCities() => _stateRepository.GetCities();

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

            if (city == null) return ResultTypes.Failed;

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

        public IEnumerable<City> GetCitiesOfState(int stateId)
            =>
                _stateRepository.GetCitiesOfState(stateId);

        public string GetCityNameById(int cityId)
            =>
                _stateRepository.GetCityNameById(cityId);
    }
}