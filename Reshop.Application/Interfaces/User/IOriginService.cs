using System;
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

        IEnumerable<Tuple<int,string>> GetStates();
        Task<Tuple<IEnumerable<Tuple<int, string>>, int, int>> GetStatesWithPaginationAsync(int pageId = 1, int take = 18, string filter = "");
        Task<ResultTypes> AddStateAsync(State state);
        Task<ResultTypes> RemoveStateAsync(int stateId);
        Task<ResultTypes> EditStateAsync(State state);
        Task<State> GetStateByIdAsync(int stateId);
        string GetStateNameById(int stateId);

        #endregion

        #region city

        IEnumerable<City> GetCities();
        Task<Tuple<IEnumerable<Tuple<int, string>>, int, int>> GetCitiesWithPaginationAsync(int pageId = 1, int take = 18, string filter = "", List<int> states = null);
        Task<ResultTypes> AddCityAsync(City city);
        Task<ResultTypes> RemoveCityAsync(int cityId);
        Task<ResultTypes> EditCityAsync(City city);
        Task<City> GetCityByIdAsync(int cityId);
        IEnumerable<City> GetCitiesOfState(int stateId);
        string GetCityNameById(int cityId);

        #endregion
    }
}