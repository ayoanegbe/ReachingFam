using System.Net.Sockets;
using System.Net;
using DnsClient;

namespace ReachingFam.Core.Services
{
    public static class DnsCheck
    {
        public static async Task<bool> IsEmailValid(string emailAddress)
        {
            string host = emailAddress.Split(Convert.ToChar("@"))[1];
            return await CheckDnsEntriesAsync(host);
        }

        private static async Task<bool> CheckDnsEntriesAsync(string domain)
        {
            try
            {
                var lookup = new LookupClient();

                var result = await lookup.QueryAsync(domain, QueryType.MX).ConfigureAwait(false);

                var records = result.Answers.Where(record => record.RecordType == DnsClient.Protocol.ResourceRecordType.MX);
                return records.Any();
            }
            catch (DnsResponseException)
            {
                return false;
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            /*throw new Exception("No network adapters with an IPv4 address in the system!")*/;
            return null;
        }
    }
}
