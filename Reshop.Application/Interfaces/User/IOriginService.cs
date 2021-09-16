using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.User;

namespace Reshop.Application.Interfaces.User
{
    public interface IOriginService
    {
        #region state

        IEnumerable<State> GetStates();
        Task<ResultTypes> AddStateAsync(State state);
        Task<ResultTypes> RemoveStateAsync(int stateId);
        Task<ResultTypes> EditStateAsync(State state);
        Task<State> GetStateByIdAsync(int stateId);
        string GetStateNameById(int stateId);

        #endregion

        #region city

        IEnumerable<City> GetCities();
        Task<ResultTypes> AddCityAsync(City city);
        Task<ResultTypes> RemoveCityAsync(int cityId);
        Task<ResultTypes> EditCityAsync(City city);
        Task<City> GetCityByIdAsync(int cityId);
        IEnumerable<City> GetCitiesOfState(int stateId);
        string GetCityNameById(int cityId);

        #endregion
    }
}