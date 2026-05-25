using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatusPageRazorUI.Models;

namespace StatusPageRazorUI.Pages
{
    public class IncidentsModel : PageModel
    {
        private const string IncidentsApiUrl = "api/incidents";
        private readonly IHttpClientFactory _httpClientFactory;

        public IncidentsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public PagedResult<IncidentDto> Incidents { get; private set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = $"{IncidentsApiUrl}?PageNumber={PageNumber}&PageSize={PageSize}";
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                url += $"&SearchTerm={Uri.EscapeDataString(SearchTerm)}";
            }

            Incidents = await client.GetFromJsonAsync<PagedResult<IncidentDto>>(url)
                ?? new PagedResult<IncidentDto> { PageNumber = PageNumber, PageSize = PageSize };

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync(string title, string description, DateTime startTime, DateTime? endTime, bool isScheduled, bool isSystemGenerated, int serviceId, int? assignedEngineerId)
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.PostAsJsonAsync(IncidentsApiUrl, new
            {
                Title = title,
                Description = description,
                StartTime = startTime,
                EndTime = endTime,
                IsScheduled = isScheduled,
                ServiceId = serviceId,
                AssignedEngineerId = assignedEngineerId,
                IsSystemGenerated = isSystemGenerated
            });

            return RedirectToPage("/Incidents", new { PageNumber, PageSize, SearchTerm });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.DeleteAsync($"{IncidentsApiUrl}/{id}");

            return RedirectToPage("/Incidents", new { PageNumber, PageSize, SearchTerm });
        }
    }
}
