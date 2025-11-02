using Microsoft.JSInterop;
using PrideLink.Shared.LoginDetails;
using System.Text.Json;

namespace PrideLink.Client.Helpers
{
    public class jwtHelper
    {
        private readonly IJSRuntime _JS;
        public jwtHelper(IJSRuntime jS)
        {
            _JS = jS;
        }

        // Return nullable token safely
        public async Task<jwtToken?> GetJWTToken()
        {
            try
            {
                string? result = await _JS.InvokeAsync<string>("localStorage.getItem", "Token");
                Console.WriteLine("1 Token is: " + result);
                if (result == null || string.IsNullOrWhiteSpace(result))
                {
                    Console.WriteLine("no token found");
                    return null;
                }

                // Try deserialize, return null on failure
                var token = JsonSerializer.Deserialize<jwtToken>(result);
                return token;
            }
            catch
            {
                return null;
            }
        }
    }
}
