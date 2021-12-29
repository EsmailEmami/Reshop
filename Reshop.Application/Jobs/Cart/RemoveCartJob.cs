using System;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using Quartz;
using Reshop.Application.Interfaces.User;
using System.Threading.Tasks;
using Reshop.Application.Services.User;
using Reshop.Domain.Interfaces.Product;
using Reshop.Domain.Interfaces.User;

namespace Reshop.Application.Jobs.Cart
{
    [DisallowConcurrentExecution]
    public class RemoveCartJob : IJob
    {
        //private string apiUrl = "https://localhost:44311/api/Cart";

        //private readonly HttpClient _httpClient;

        //public RemoveCartJob(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;
        //}


        public Task Execute(IJobExecutionContext context)
        {

            var time = DateTime.Now.AddDays(-15);


            //var result = _httpClient.DeleteAsync(apiUrl + "/" + "RemoveOrdersAfterDateTime" + "/" + time).Result;


            return Task.CompletedTask;
        }
    }
}
