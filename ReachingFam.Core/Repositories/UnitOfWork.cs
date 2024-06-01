using ReachingFam.Core.Data;
using ReachingFam.Core.Interfaces;

namespace ReachingFam.Core.Repositories
{
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork, IDisposable
    {
        protected readonly ApplicationDbContext _context = context;

        public Task<int> SaveChangesAsync()
            => _context.SaveChangesAsync();

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
            => _context.SaveChangesAsync(cancellationToken);

        public void Dispose() 
            => _context.Dispose();
    }
}
