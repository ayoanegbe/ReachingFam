using ReachingFam.Core.Data;
using ReachingFam.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Repositories
{
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        protected readonly ApplicationDbContext _context = context;

        public Task<int> SaveChangesAsync()
        => _context.SaveChangesAsync();

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
            => _context.SaveChangesAsync(cancellationToken);
    }
}
