using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatusPageServices.RequestDTO.IncidentUpdates;
using StatusPageServices.ResponseDTO;
using StatusPageServices.ResponseDTO.IncidentUpdates;

namespace StatusPageRazorUI.Pages
{
    public class IncidentUpdatesModel : PageModel
    {
        private const string IncidentUpdatesApiUrl = "https://localhost:7246/api/incidentupdates";
        private readonly IHttpClientFactory _httpClientFactory;

        public IncidentUpdatesModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public PagedResult<IncidentUpdateDto> Updates { get; private set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var items = await client.GetFromJsonAsync<List<IncidentUpdateDto>>(IncidentUpdatesApiUrl)
                ?? new List<IncidentUpdateDto>();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                items = items
                    .Where(u => u.Message.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                        || u.UpdateStatus.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            Updates = BuildPagedResult(items, PageNumber, PageSize);

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync(string message, string updateStatus, int incidentId, int engineerId)
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var dto = new CreateIncidentUpdateDto(message, updateStatus, incidentId, engineerId);
            await client.PostAsJsonAsync(IncidentUpdatesApiUrl, dto);

            return RedirectToPage("/IncidentUpdates", new { PageNumber, PageSize, SearchTerm });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.DeleteAsync($"{IncidentUpdatesApiUrl}/{id}");

            return RedirectToPage("/IncidentUpdates", new { PageNumber, PageSize, SearchTerm });
        }

        private static PagedResult<IncidentUpdateDto> BuildPagedResult(List<IncidentUpdateDto> items, int pageNumber, int pageSize)
        {
            var totalCount = items.Count;
            var pagedItems = items
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<IncidentUpdateDto>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
