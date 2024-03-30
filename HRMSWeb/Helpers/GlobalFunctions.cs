using Microsoft.AspNetCore.Localization;

namespace HRMSWeb.Helpers
{
    public class GlobalFunctions
    {
        public static string FormatCookie(string cookie)
        {
            // c=sq-AL|uic=sq-AL
            var list = cookie.Split('=', StringSplitOptions.RemoveEmptyEntries).ToList();

            return list[list.Count - 1];
        }
        public static string GetCulture(HttpContext httpContext)
        {
            var cookie = httpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];

            if (cookie != null)
            {
                var formatedCookie = FormatCookie(cookie);

                return formatedCookie;
            }
            else
            {
                // Fallback
                return "sq-AL";
            }
        }
    }
}
