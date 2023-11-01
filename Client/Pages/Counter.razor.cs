using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using OCTOBER.Shared.DTO;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using static System.Net.WebRequestMethods;
using OCTOBER.Client.Services;

namespace OCTOBER.Client.Pages
{
    public partial class Counter: ComponentBase
    {
        [Inject]
        protected HttpClient Http { get; set; }

        [Inject]
        protected LookupService _LookupService { get; set; }

        List<CourseDTO>? lstCourse { get; set; }

        private int SelectedCourse { get; set; }


        protected JsonSerializerOptions options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            PropertyNameCaseInsensitive = true
        };

        private int currentCount = 0;

        private void IncrementCount()
        {
            currentCount++;
        }

        protected override async Task OnInitializedAsync()
        {
         //   lstCourse =  await Http.GetFromJsonAsync<List<CourseDTO>>($"api/Course/Get", options);

            lstCourse = await _LookupService.GetCourses();

            currentCount = 17;
        }



    }
}
