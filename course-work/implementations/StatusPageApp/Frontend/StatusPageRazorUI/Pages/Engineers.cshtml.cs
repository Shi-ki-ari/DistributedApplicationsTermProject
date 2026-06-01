using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatusPageRazorUI.Models;

namespace StatusPageRazorUI.Pages
{
    public class EngineersModel : PageModel
    {
        private const string EngineersApiUrl = "api/engineers";
        private readonly IHttpClientFactory _httpClientFactory;

        public EngineersModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchEmail { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SortBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool SortDescending { get; set; }

        public PagedResult<EngineerDto> Engineers { get; private set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = $"{EngineersApiUrl}?PageNumber={PageNumber}&PageSize={PageSize}";
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                url += $"&SearchTerm={Uri.EscapeDataString(SearchTerm)}";
            }
            if (!string.IsNullOrWhiteSpace(SearchEmail))
            {
                url += $"&SearchEmail={Uri.EscapeDataString(SearchEmail)}";
            }
            if (!string.IsNullOrWhiteSpace(SortBy))
            {
                url += $"&SortBy={Uri.EscapeDataString(SortBy)}";
            }
            if (SortDescending)
            {
                url += $"&SortDescending=true";
            }

            Engineers = await client.GetFromJsonAsync<PagedResult<EngineerDto>>(url)
                ?? new PagedResult<EngineerDto> { PageNumber = PageNumber, PageSize = PageSize };

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync(string name, string email, DateTime? hiredDate, bool onCall, double hourlyRate)
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(EngineersApiUrl, new
            {
                Name = name,
                Email = email,
                HiredDate = hiredDate,
                OnCall = onCall,
                HourlyRate = hourlyRate
            });

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Engineers", new { PageNumber, PageSize, SearchTerm });
            }

            return RedirectToPage("/Engineers", new { PageNumber, PageSize, SearchTerm });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"{EngineersApiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Engineers", new { PageNumber, PageSize, SearchTerm });
            }

            return RedirectToPage("/Engineers", new { PageNumber, PageSize, SearchTerm });
        }

        public async Task<IActionResult> OnPostEditAsync(int id, string name, string email, DateTime hiredDate, bool onCall, decimal hourlyRate)
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.PutAsJsonAsync($"api/engineers/{id}", new
            {
                Id = id,
                Name = name,
                Email = email,
                HiredDate = hiredDate,
                OnCall = onCall,
                HourlyRate = hourlyRate
            });

            return RedirectToPage("/Engineers", new { PageNumber, PageSize, SearchTerm });
        }
    }
}
