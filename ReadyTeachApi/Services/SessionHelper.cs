using Microsoft.AspNetCore.Http;
using ReadyTeachApi.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace ReadyTeachApi.Services
{
    public class SessionHelper : ISessionHelper
    {
        public SessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public void Add(string sessionKey, int sessionValue)
        {
            _httpContextAccessor.HttpContext.Session.SetInt32(sessionKey, sessionValue);
        }

        public int? Get(string sessionKey)
        {
            return _httpContextAccessor.HttpContext.Session.GetInt32(sessionKey);
        }
    }
}
