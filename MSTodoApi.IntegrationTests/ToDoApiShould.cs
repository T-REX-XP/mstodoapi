using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using MSTodoApi.Infrastructure;
using MSTodoApi.IntegrationTests.Infrastructure;
using MSTodoApi.Model;
using MSTodoApi.ViewModel;
using Newtonsoft.Json;
using Xunit;

namespace MSTodoApi.IntegrationTests
{
    public class ToDoApiShould :IDisposable
    {
        private readonly CustomTestServer _server;
        private readonly HttpClient _client;
        
        private static readonly HttpClient TestDataClient= new HttpClient();
        public static ITokenStore TokenStore = new InMemoryTokenStore();

        private readonly List<TaskModel> _testTaskModels;
        private readonly List<EventModel> _testEventModels;

        public ToDoApiShould()
        {
            var credentials = new AppCredentials
            {
                AppId = "5b9ed3b6-652c-4ba5-b6cd-5438dba1ac15",
                AppSecret = "U4byxd0fsqYjTopaem6a2YV"
            };
            
            // I need to create own `TestServer` because default `TestServer` 
            // doesn't allow you to configure the client created by testserver.
            // For example adding other delegating handler
            _server = new CustomTestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            
            // This creates test http client with RefreshTokenHandler, 
            // so when you get `Unauthorized` the handler will try to get new access_token and refresh_token
            // and store them using `token store`
            
            _client = _server.CreateClient(TokenStore, credentials);
            
            _testEventModels = new List<EventModel>();
            _testTaskModels = new List<TaskModel>();
            
            TestDataClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenStore.AccessToken);
             CreateTestData().Wait();
        }

        [Fact]
        public async void ReturnTasksAndEvents_WithOverdueTasks()
        {
            var requestUri = "/api/todos";
            
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add(Constants.TokenHeaderKey, TokenStore.AccessToken);
            
            var response = await _client.SendAsync(request);

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
