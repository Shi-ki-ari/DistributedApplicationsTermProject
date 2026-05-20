using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatusPageServices.RequestDTO.Engineers;
using StatusPageServices.ResponseDTO;
using StatusPageServices.ResponseDTO.Engineers;

namespace StatusPageRazorUI.Pages
{
    public class EngineersModel : PageModel
    {
        private const string EngineersApiUrl = "https://localhost:7246/api/engineers";
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

        public PagedResult<EngineerDto> Engineers { get; private set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = $"{EngineersApiUrl}?PageNumber={PageNumber}&PageSize={PageSize}";
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                url += $"&SearchTerm={Uri.EscapeDataString(SearchTerm)}";
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

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var dto = new CreateEngineerDto(name, email, hiredDate, onCall, hourlyRate);
            var response = await client.PostAsJsonAsync(EngineersApiUrl, dto);

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

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"{EngineersApiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Engineers", new { PageNumber, PageSize, SearchTerm });
            }

            return RedirectToPage("/Engineers", new { PageNumber, PageSize, SearchTerm });
        }
    }
}
