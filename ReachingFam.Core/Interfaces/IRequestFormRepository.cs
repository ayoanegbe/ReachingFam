using ReachingFam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Interfaces
{
    public interface IRequestFormRepository
    {
        Task<bool> SetRequestForm(RequestForm requestForm);
    }
}
