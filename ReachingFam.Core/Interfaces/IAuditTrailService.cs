using ReachingFam.Core.Enums;
using ReachingFam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Interfaces
{
    public interface IAuditTrailService
    {
        Task<bool> UpdateAuditTrail(string className, UpdateAction action, string oldValue, string newValue, string userName);
        Task<bool> UpdateAuditTrail(string className, UpdateAction action, string newValue, string userName);
        Task<List<AuditTrail>> GetAuditTrail(string className, DateTime startDate, DateTime endDate);
        Task<(T, T)> GetAuditData<T>(int id) where T : class;
    }
}
