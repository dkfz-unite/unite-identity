namespace Unite.Identity.Web.Helpers;

public class CookieHelper
{
    public const string SESSION_COOKIE_NAME = "unite_session";

    public static string GetSessionCookie(HttpRequest request)
    {
        if (request.Cookies.TryGetValue(SESSION_COOKIE_NAME, out var session))
        {
            return session;
        }
        else
        {
            return null;
        }
    }

    public static void SetSessionCookie(HttpResponse response, string session, DateTime expiryDate)
    {
        var options = new CookieOptions
        {
            Expires = expiryDate,
            SameSite = SameSiteMode.Lax,
            HttpOnly = true,
            Secure = true,
        };

        response.Cookies.Append(SESSION_COOKIE_NAME, session, options);
    }

    public static void DeleteSessionCookie(HttpResponse response)
    {
        response.Cookies.Delete(SESSION_COOKIE_NAME);
    }
}
