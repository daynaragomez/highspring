using Microsoft.AspNetCore.Http;

namespace Highspring.Web.Infrastructure;

public static class GuestSessionAccessor
{
    private const string CookieName = "highspring_guest_session";

    public static string GetOrCreateGuestSessionId(HttpContext httpContext)
    {
        if (httpContext.Request.Cookies.TryGetValue(CookieName, out var existing)
            && !string.IsNullOrWhiteSpace(existing))
        {
            return existing;
        }

        var guestSessionId = Guid.NewGuid().ToString("N");
        httpContext.Response.Cookies.Append(CookieName, guestSessionId, new CookieOptions
        {
            HttpOnly = true,
            IsEssential = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(30)
        });

        return guestSessionId;
    }
}
