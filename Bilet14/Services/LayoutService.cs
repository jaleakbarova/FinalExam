using Bilet14.DAL;

namespace Bilet14.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }

        public Dictionary<string,string> GetService()
        {
            return _context.Settings.ToList().ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
