using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using MyTestLib;

namespace WebMvc.Events
{
    public class MyChunkingCookieManager : ICookieManager
    {
        private readonly ChunkingCookieManager _cookieManager;

        public MyChunkingCookieManager()
        {
            _cookieManager = new ChunkingCookieManager();
        }

        public string GetRequestCookie(HttpContext context, string key)
        {
            return _cookieManager.GetRequestCookie(context, key);
        }

        public void AppendResponseCookie(HttpContext context, string key, string value, CookieOptions options)
        {
            _cookieManager.AppendResponseCookie(context, key, value, options);
        }

        public void DeleteCookie(HttpContext context, string key, CookieOptions options)
        {
            var i = new FourSeven();
            _cookieManager.DeleteCookie(context, key, options);
        }
    }
}