using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.Entities.User;

namespace Reshop.Domain.Interfaces.User
{
    public interface IStateRepository
    {
        #region state

        IAsyncEnumerable<State> GetStates();
        Task AddStateAsync(State state);
        void RemoveState(State state);
        void UpdateState(State state);
        Task<State> GetStateByIdAsync(int stateId);

        #endregion

        #region city

        IAsyncEnumerable<City> GetCities();
        Task AddCityAsync(City city);
        void RemoveCity(City city);
        void UpdateCity(City city);
        Task<City> GetCityByIdAsync(int cityId);

        #endregion

        #region StateCity

        Task AddStateCityAsync(StateCity stateCity);
        void RemoveStateCity(StateCity stateCity);
        void UpdateStateCity(StateCity stateCity);
        IAsyncEnumerable<City> GetCitiesOfState(int stateId);
        IAsyncEnumerable<StateCity> GetStateCitiesByStateId(int stateId);
        IAsyncEnumerable<StateCity> GetStateCitiesByCityId(int cityId);
        IAsyncEnumerable<int> GetCitiesIdOfState(int stateId);

        #endregion

        Task SaveChangesAsync();
    }
}
