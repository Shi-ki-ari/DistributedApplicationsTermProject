using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatusPageRazorUI.Models;

namespace StatusPageRazorUI.Pages
{
    public class ServicesModel : PageModel
    {
        private const string ServicesApiUrl = "api/services";
        private readonly IHttpClientFactory _httpClientFactory;

        public ServicesModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public PagedResult<ServiceDto> Services { get; private set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = $"{ServicesApiUrl}?PageNumber={PageNumber}&PageSize={PageSize}";
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                url += $"&SearchTerm={Uri.EscapeDataString(SearchTerm)}";
            }

            Services = await client.GetFromJsonAsync<PagedResult<ServiceDto>>(url)
                ?? new PagedResult<ServiceDto> { PageNumber = PageNumber, PageSize = PageSize };

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync(string name, string targetUrl, int categoryId, bool isOnline)
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.PostAsJsonAsync(ServicesApiUrl, new
            {
                Name = name,
                TargetUrl = targetUrl,
                CategoryId = categoryId,
                IsOnline = isOnline
            });

            return RedirectToPage("/Services", new { PageNumber, PageSize, SearchTerm });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.DeleteAsync($"{ServicesApiUrl}/{id}");

            return RedirectToPage("/Services", new { PageNumber, PageSize, SearchTerm });
        }
    }
}
