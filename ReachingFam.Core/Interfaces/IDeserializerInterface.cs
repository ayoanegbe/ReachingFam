using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Interfaces
{
    public interface IDeserializerInterface
    {
        T Deserialize<T>(string serializedData) where T : class;
    }
}
