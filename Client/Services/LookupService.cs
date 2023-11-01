using Microsoft.Extensions.Options;
using OCTOBER.Shared.DTO;
using static System.Net.WebRequestMethods;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;

namespace OCTOBER.Client.Services
{
    public class LookupService
    {
        private string RestAPI = "api/Lookup";
        protected HttpClient Http { get; set; }
        protected JsonSerializerOptions options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            PropertyNameCaseInsensitive = true
        };

        public LookupService(HttpClient client)
        {
            Http = client;
        }

        public async Task<List<CourseDTO>> GetCourses()
        {
            return await Http.GetFromJsonAsync<List<CourseDTO>>($"api/Course/Get", options);

        }
    }

}
