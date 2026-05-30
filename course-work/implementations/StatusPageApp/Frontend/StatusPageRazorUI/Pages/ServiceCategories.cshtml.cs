using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatusPageRazorUI.Models;

namespace StatusPageRazorUI.Pages
{
    public class ServiceCategoriesModel : PageModel
    {
        private const string ServiceCategoriesApiUrl = "api/servicecategories";
        private readonly IHttpClientFactory _httpClientFactory;

        public ServiceCategoriesModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public PagedResult<ServiceCategoryDto> Categories { get; private set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var items = await client.GetFromJsonAsync<List<ServiceCategoryDto>>(ServiceCategoriesApiUrl)
                ?? new List<ServiceCategoryDto>();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                items = items
                    .Where(c => c.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                        || (c.Description?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false))
                    .ToList();
            }

            Categories = BuildPagedResult(items, PageNumber, PageSize);

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync(string name, string? description, int displayOrder, bool notify)
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.PostAsJsonAsync(ServiceCategoriesApiUrl, new
            {
                Name = name,
                Description = description,
                DisplayOrder = displayOrder,
                Notify = notify
            });

            return RedirectToPage("/ServiceCategories", new { PageNumber, PageSize, SearchTerm });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.DeleteAsync($"{ServiceCategoriesApiUrl}/{id}");

            return RedirectToPage("/ServiceCategories", new { PageNumber, PageSize, SearchTerm });
        }

        private static PagedResult<ServiceCategoryDto> BuildPagedResult(List<ServiceCategoryDto> items, int pageNumber, int pageSize)
        {
            var totalCount = items.Count;
            var pagedItems = items
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<ServiceCategoryDto>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
