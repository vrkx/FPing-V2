using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FPing_V2.F_Source
{
    public class PingService
    {
        // A method to ping a single server asynchronously.
        public async Task<long> PingServerAsync(string serverAddress)
        {
            try
            {
                using (Ping pingSender = new Ping())
                {
                    PingReply reply = await pingSender.SendPingAsync(serverAddress);
                    if (reply.Status == IPStatus.Success)
                    {
                        return reply.RoundtripTime;
                    }
                    else
                    {
                        return -1; // Indicate a failed ping
                    }
                }
            }
            catch
            {
                return -1; // Indicate a failed ping due to an exception
            }
        }

        // A method to ping multiple servers.
        public async Task<Dictionary<string, long>> PingAllServersAsync(List<string> serverAddresses)
        {
            var results = new Dictionary<string, long>();
            var tasks = new List<Task<(string, long)>>();

            foreach (var address in serverAddresses)
            {
                tasks.Add(Task.Run(async () =>
                {
                    long latency = await PingServerAsync(address);
                    return (address, latency);
                }));
            }

            var resultsArray = await Task.WhenAll(tasks);

            foreach (var result in resultsArray)
            {
                results[result.Item1] = result.Item2;
            }

            return results;
        }
    }
}
