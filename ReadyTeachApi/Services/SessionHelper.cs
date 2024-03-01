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

        public async Task Add(string sessionKey, int? sessionValue)
        {
            await Task.Run(() => _httpContextAccessor.HttpContext.Session.SetInt32(sessionKey, sessionValue ?? 0));
        }

        public async Task<int?> Get(string sessionKey)
        {
            return await Task.FromResult(_httpContextAccessor.HttpContext.Session.GetInt32(sessionKey));
        }
    }
}
