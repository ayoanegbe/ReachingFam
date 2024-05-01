using Newtonsoft.Json;
using ReachingFam.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Repositories
{
    public class Deserializer
    {
        public T Deserialize<T>(string serializedData) where T : class
        {
            return JsonConvert.DeserializeObject<T>(serializedData);
        }
    }
}
