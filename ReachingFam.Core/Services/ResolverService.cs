using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReachingFam.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReachingFam.Core.Services
{
    public class ResolverService(ILogger<ResolverService> logger, CustomIDataProtection protector) : IResolverService
    {
        private readonly ILogger<ResolverService> _logger = logger;
        private readonly CustomIDataProtection _protector = protector;

        public int ResolveInterger(string data)
        {
            try
            {
                string decodedString = _protector.Decode(data);
                int decodedNumber = int.Parse(decodedString);
                return decodedNumber;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"An error has occurred fetching item {ex}");
                return 0;
            }
        }

        public string ResolveString(string data)
        {
            try
            {
                return _protector.Decode(data);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"An error has occurred fetching item {ex}");
                return null;
            }
        }

        public T ResolveObject<T>(string data) where T : class
        {
            try
            {
                string obj = _protector.Decode(data);

                return JsonConvert.DeserializeObject<T>(obj);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"An error has occurred fetching item {ex}");
                return null;
            }
        }
    }
}
