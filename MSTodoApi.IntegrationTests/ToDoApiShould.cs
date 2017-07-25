using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MSTodoApi.Infrastructure;
using MSTodoApi.Infrastructure.Auth;
using MSTodoApi.Model;
using MSTodoApi.ViewModel;
using Newtonsoft.Json;
using Xunit;

namespace MSTodoApi.IntegrationTests
{
    public class ToDoApiShould :IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        
        private static readonly HttpClient TestDataClient= new HttpClient();

        private readonly List<TaskModel> _testTaskModels;
        private readonly List<EventModel> _testEventModels;
        
        public ToDoApiShould()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();

            TestDataClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", new InMemoryTokenStore().AccessToken);
            
            _testEventModels = new List<EventModel>();
            _testTaskModels = new List<TaskModel>();
            
             CreateTestData().Wait();
        }

        [Fact]
        public async void ReturnTasksAndEvents_WithOverdueTasks()
        {
            var request = "/api/todos";
            var response = await _client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TodosViewModel>(json);

            Assert.Equal(2, model.Tasks.Count);  // yesterday and today
            Assert.Equal(1, model.Events.Count); // today
        }

        public void Dispose()
        {
            _server?.Dispose();
            _client?.Dispose();
          
             DeleteTestData().Wait();
            
            TestDataClient?.Dispose();
        }

        private async Task CreateTestData()
        {
            DateTime[] dateTimes =
            {
                DateTime.UtcNow.AddDays(-1), // yesterday (overdue)
                DateTime.UtcNow, // today
                DateTime.UtcNow.AddDays(1) // tomorrow
            };

            foreach (var dueDateTime in dateTimes)
            {
                var taskModelFor = TestDataHelper.TaskModelFor(dueDateTime);
                var eventModelFor = TestDataHelper.EventModelFor(dueDateTime);
                Console.WriteLine($"[TASK]-> {taskModelFor}");
                Console.WriteLine($"[EVENT]-> {eventModelFor}");
                var task = await PostJsonAsync<TaskModel>("me/tasks", taskModelFor);
                var evt = await PostJsonAsync<EventModel>("me/events", eventModelFor);
            
                _testEventModels.Add(evt);
                _testTaskModels.Add(task);
            }
        }

        private async Task DeleteTestData()
        {
            //Clean events
            try
            {
                foreach (var eventModel in _testEventModels)
                {
                    var resp = await TestDataClient
                        .DeleteAsync(Constants.OutlookBaseAddress + $"me/events/{eventModel.Id}")
                        .ConfigureAwait(false);
                    if (!resp.IsSuccessStatusCode)
                    {
                        Console.WriteLine("[EVENT] The response status code was not successful.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            //Clean tasks
            try
            {
                foreach (var taskModel in _testTaskModels)
                {
                    var resp = await TestDataClient
                        .DeleteAsync(Constants.OutlookBaseAddress + $"me/tasks('{taskModel.Id}')")
                        .ConfigureAwait(false);

                    if (!resp.IsSuccessStatusCode)
                    {
                        Console.WriteLine("[TASK] The response status code was not successful.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static async Task<T> PostJsonAsync<T>(string resource, string data) where T : class
        {
            var stringContent = new StringContent(data, Encoding.UTF8, "application/json");
            var requestUri = $"{Constants.OutlookBaseAddress}{resource}";
            using(HttpResponseMessage response = await TestDataClient.PostAsync(requestUri,
                    stringContent).ConfigureAwait(false)){

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("The response status code was not successful.");
                }

                using (HttpContent content = response.Content)
                {
                    var json = await content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
        }
    }
}
