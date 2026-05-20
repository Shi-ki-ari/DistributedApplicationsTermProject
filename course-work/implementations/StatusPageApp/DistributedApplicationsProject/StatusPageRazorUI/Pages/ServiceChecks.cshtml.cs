using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatusPageServices.ResponseDTO;
using StatusPageServices.ResponseDTO.ServiceChecks;

namespace StatusPageRazorUI.Pages
{
    public class ServiceChecksModel : PageModel
    {
        private const string ServiceChecksApiUrl = "https://localhost:7246/api/servicechecks";
        private readonly IHttpClientFactory _httpClientFactory;

        public ServiceChecksModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public PagedResult<ServiceCheckDto> Checks { get; private set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var items = await client.GetFromJsonAsync<List<ServiceCheckDto>>(ServiceChecksApiUrl)
                ?? new List<ServiceCheckDto>();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                items = items
                    .Where(c => c.ServiceId.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                        || (c.StatusCode?.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false)
                        || (c.ErrorMessage?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false))
                    .ToList();
            }

            Checks = BuildPagedResult(items, PageNumber, PageSize);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.DeleteAsync($"{ServiceChecksApiUrl}/{id}");

            return RedirectToPage("/ServiceChecks", new { PageNumber, PageSize, SearchTerm });
        }

        public async Task<IActionResult> OnPostSweepAsync()
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.PostAsync($"{ServiceChecksApiUrl}/sweep", null);

            return RedirectToPage("/ServiceChecks", new { PageNumber, PageSize, SearchTerm });
        }

        private static PagedResult<ServiceCheckDto> BuildPagedResult(List<ServiceCheckDto> items, int pageNumber, int pageSize)
        {
            var totalCount = items.Count;
            var pagedItems = items
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<ServiceCheckDto>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
