using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using Workouts.Entities.CustomExceptions;
using Workouts.Entities.Dto;

namespace Workouts.Presentation.Services
{
    public class ApiCallService : IApiCallService
    {
        private HttpClient _client;

        public ApiCallService(IConfiguration configuration, HttpContext context)
        {
            string apiUrl = configuration["ApiUrl"] ?? string.Empty;
            _client = new HttpClient();
            SetupClient(_client, context, apiUrl);
        }

        // for testing
        public ApiCallService(HttpContext context, HttpMessageHandler handler)
        {
            string testApiUrl = "https://localhost:123";
            _client = new HttpClient(handler);
            SetupClient(_client, context, testApiUrl);
        }

        public async Task<long> GetUserId()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "UserId");
            var response = await _client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new DeactivatedUserException();
            }
            string content = await response.Content.ReadAsStringAsync();
            long id = Convert.ToInt64(content);
            return id;
        }

        public async Task<string> GetUsername()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "Username");
            var response = await _client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Forbidden)
            { 
                throw new DeactivatedUserException();
            }
            string username = await response.Content.ReadFromJsonAsync<string>() ?? string.Empty;
            return username;
        }

        public async Task<WorkoutDto> GetWorkout(long workoutId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"Workout/{workoutId}");
            var response = await _client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new WorkoutDoesNotExistException();
            }
            var workout = await response.Content.ReadFromJsonAsync<WorkoutDto>() ?? new WorkoutDto();
            return workout;
        }

        public async Task<Dictionary<long, string>> GetWorkouts(long userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"Workouts/{userId}");
            var response = await _client.SendAsync(request);
            var workoutData = await response.Content.ReadFromJsonAsync<Dictionary<long, string>>() ?? new Dictionary<long, string>();
            return workoutData;
        }

        public async Task<long> AddWorkout(WorkoutDto workout)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "Workout");
            request.Content = new StringContent(JsonConvert.SerializeObject(workout));
            var response = await _client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string validationErrors = await response.Content.ReadAsStringAsync();
                throw new InvalidWorkoutException(validationErrors);
            }
            long id = await response.Content.ReadFromJsonAsync<long>();
            return id;
        }

        public async Task ArchiveWorkout(long workoutId)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"ArchiveWorkout/{workoutId}");
            var response = await _client.SendAsync(request);
        }

        public async Task UnarchiveWorkout(long workoutId)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"UnarchiveWorkout/{workoutId}");
            var response = await _client.SendAsync(request);
        }

        public async Task UpdateUsername(string username)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"Username/{username}");
            var response = await _client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new DeactivatedUserException();
            }
        }

        public async Task UpdateWorkout(WorkoutDto workout)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"Workout");
            request.Content = new StringContent(JsonConvert.SerializeObject(workout));
            var response = await _client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string validationErrors = await response.Content.ReadAsStringAsync();
                throw new InvalidWorkoutException(validationErrors);
            }
        }

        private void SetupClient(HttpClient client, HttpContext context, string apiUrl)
        {
            string identifier = context.User.Claims
                .Where(c => c.Type.Equals(ClaimTypes.NameIdentifier))
                .Select(c => c.Value)
                .FirstOrDefault(string.Empty);
            string username = context.User.Claims
                .Where(c => c.Type.Equals(ClaimTypes.Name))
                .Select(c => c.Value)
                .FirstOrDefault(string.Empty);

            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("UserIdentifier", identifier);
            client.DefaultRequestHeaders.Add("Username", username);
        }
    }
}
