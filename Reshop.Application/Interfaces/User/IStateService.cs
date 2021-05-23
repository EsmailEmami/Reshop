using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.User;

namespace Reshop.Application.Interfaces.User
{
    public interface IStateService
    {
        #region state

        IAsyncEnumerable<State> GetStates();
        Task<ResultTypes> AddStateAsync(State state);
        Task<ResultTypes> RemoveStateAsync(int stateId);
        Task<ResultTypes> EditStateAsync(State state);
        Task<State> GetStateWithCitiesByIdAsync(int stateId);

        #endregion

        #region city

        IAsyncEnumerable<City> GetCities();
        Task<ResultTypes> AddCityAsync(City city);
        Task<ResultTypes> RemoveCityAsync(City city);
        Task<ResultTypes> UpdateCityAsync(City city);
        Task<City> GetCityByIdAsync(int cityId);

        #endregion
    }
}