using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatusPageServices.ResponseDTO;
using StatusPageServices.ResponseDTO.User;
using CreateUserDto = StatusPageServices.RequestDTO.Users.StatusPageServices.RequestDTO.Users.CreateUserDto;

namespace StatusPageRazorUI.Pages
{
    public class UsersModel : PageModel
    {
        private const string UsersApiUrl = "https://localhost:7246/api/users";
        private readonly IHttpClientFactory _httpClientFactory;

        public UsersModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public PagedResult<UserDto> Users { get; private set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var items = await client.GetFromJsonAsync<List<UserDto>>(UsersApiUrl)
                ?? new List<UserDto>();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                items = items
                    .Where(u => u.Username.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            Users = BuildPagedResult(items, PageNumber, PageSize);

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync(string username, string password)
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var dto = new CreateUserDto(username, password);
            await client.PostAsJsonAsync(UsersApiUrl, dto);

            return RedirectToPage("/Users", new { PageNumber, PageSize, SearchTerm });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!Request.Cookies.TryGetValue("JWTToken", out var token) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.DeleteAsync($"{UsersApiUrl}/{id}");

            return RedirectToPage("/Users", new { PageNumber, PageSize, SearchTerm });
        }

        private static PagedResult<UserDto> BuildPagedResult(List<UserDto> items, int pageNumber, int pageSize)
        {
            var totalCount = items.Count;
            var pagedItems = items
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<UserDto>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
