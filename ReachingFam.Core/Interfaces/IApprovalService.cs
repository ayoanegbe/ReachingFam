using ReachingFam.Core.Enums;
using ReachingFam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Interfaces
{
    public interface IApprovalService
    {
        //Task<List<ApprovalQueue>> GetItemsForApproval(string className, DateTime startDate, DateTime endDate);
        Task<(T, T)> GetItemForApproval<T>(int id) where T : class;
        Task<bool> UpdateApprovalQueue(string className, string moduleName, UpdateAction action, string oldValue, string newValue, string userName);
    }
}
