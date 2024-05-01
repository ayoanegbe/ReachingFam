using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Interfaces
{
    public interface IResolverService
    {
        int ResolveInterger(string data);
        string ResolveString(string data);
        T ResolveObject<T>(string data) where T : class;
    }
}
