namespace WebApp.Services
{
    public class ScopedService : IScopedService
    {
        private readonly Guid Id;

        public ScopedService()
        {
            Id = Guid.NewGuid();
        }

        public string GetGuid()
        {
            return Id.ToString();
        }
    }
}
