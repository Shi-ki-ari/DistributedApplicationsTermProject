using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CreateUserDto = StatusPageServices.RequestDTO.Users.StatusPageServices.RequestDTO.Users.CreateUserDto;

namespace StatusPageRazorUI.Pages
{
    public class RegisterModel : PageModel
    {
        private const string RegisterApiUrl = "https://localhost:7246/api/auth/register";
        private readonly IHttpClientFactory _httpClientFactory;

        public RegisterModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!string.Equals(Password, ConfirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            var dto = new CreateUserDto(Username, Password);
            var response = await client.PostAsJsonAsync(RegisterApiUrl, dto);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Registration failed.");
                return Page();
            }

            return RedirectToPage("/Login");
        }
    }
}
