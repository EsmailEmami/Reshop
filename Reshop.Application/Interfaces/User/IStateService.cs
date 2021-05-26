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
        Task<AddOrEditStateViewModel> GetStateDataForEditAsync(int stateId);
        Task<State> GetStateByIdAsync(int stateId);

        #endregion

        #region city

        IAsyncEnumerable<City> GetCities();
        Task<ResultTypes> AddCityAsync(City city);
        Task<ResultTypes> RemoveCityAsync(int cityId);
        Task<ResultTypes> EditCityAsync(City city);
        Task<City> GetCityByIdAsync(int cityId);
        IAsyncEnumerable<City> GetCitiesOfState(int stateId);

        #endregion

        #region state city

        Task RemoveCitiesOfStateAsync(int stateId);
        Task AddStateCityAsync(StateCity stateCity);

        #endregion
    }
}