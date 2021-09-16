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

        IEnumerable<State> GetStates();
        Task AddStateAsync(State state);
        void RemoveState(State state);
        void UpdateState(State state);
        Task<State> GetStateByIdAsync(int stateId);
        string GetStateNameById(int stateId);

        #endregion

        #region city

        IEnumerable<City> GetCities();
        Task AddCityAsync(City city);
        void RemoveCity(City city);
        void UpdateCity(City city);
        Task<City> GetCityByIdAsync(int cityId);
        string GetCityNameById(int cityId);

        #endregion

        IEnumerable<City> GetCitiesOfState(int stateId);

        Task SaveChangesAsync();
    }
}
