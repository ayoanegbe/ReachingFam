using ReachingFam.Core.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Services
{
    public class RecaptchaService(IOptions<RecaptchaSettings> settings)
    {
        private readonly RecaptchaSettings _settings = settings.Value;

        public virtual async Task<RecaptchaResponse> VerifyRecaptcha(string _token)
        {
            RecaptchaData data = new()
            {
                token = _token,
                secret = _settings.SecretKey
            };

            HttpClient client = new();

            var response = await client.GetStringAsync($"{_settings.ApiUrl}?secret={data.secret}&response={data.token}");

            RecaptchaResponse jsonResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(response);

            return jsonResponse;
        }
    }

    public class RecaptchaData
    {
        public string token { get; set; }
        public string secret { get; set; }
    }

    public class RecaptchaResponse
    {
        public bool success { get; set; }
        public double score { get; set; }
        public string action { get; set; }
        public DateTime challenge_ts { get; set; }
        public string hostname { get; set; }
    }
}
