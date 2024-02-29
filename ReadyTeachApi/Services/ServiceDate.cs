using ReadyTeachApi.Services.Interfaces;

namespace ReadyTeachApi.Services
{
    public class ServiceDate : IServiceDate
    {
        public DateTime GetDate()
        {
            return DateTime.Now;
        }
    }
}
