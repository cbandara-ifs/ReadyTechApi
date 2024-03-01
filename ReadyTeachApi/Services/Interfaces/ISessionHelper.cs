namespace ReadyTeachApi.Services.Interfaces
{
    public interface ISessionHelper
    {
        public Task Add(string sessionKey, int? sessionValue);

        public Task<int?> Get(string sessionKey);
    }
}
