using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.Entities.User;

namespace Reshop.Domain.Interfaces.User
{
    public interface IOriginRepository
    {
        #region state

        IEnumerable<Tuple<int, string>> GetStates();
        IEnumerable<Tuple<int, string>> GetStatesWithPagination(int skip, int take, string filter);
        Task AddStateAsync(State state);
        void RemoveState(State state);
        void UpdateState(State state);
        Task<State> GetStateByIdAsync(int stateId);
        string GetStateNameById(int stateId);
        Task<int> GetStatesCountAsync();
        Task<int> GetStateIdOfCityAsync(int cityId);
        #endregion

        #region city

        IEnumerable<City> GetCities();
        IEnumerable<Tuple<int, string>> GetCitiesWithPagination(int skip, int take, string filter = "", List<int> states = null);
        Task AddCityAsync(City city);
        void RemoveCity(City city);
        void UpdateCity(City city);
        Task<City> GetCityByIdAsync(int cityId);
        string GetCityNameById(int cityId);
        Task<int> GetCitiesCountAsync();

        #endregion

        IEnumerable<City> GetCitiesOfState(int stateId);

        Task SaveChangesAsync();
    }
}
