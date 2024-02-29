namespace ReadyTeachApi.Services.Interfaces
{
    public interface ISessionHelper
    {
        public void Add(string sessionKey, int sessionValue);

        public int? Get(string sessionKey);
    }
}
