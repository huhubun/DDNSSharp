using DDNSSharp.Helpers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Net.Sockets;
using static DDNSSharp.Helpers.IPHelper;

namespace DDNSSharp.Commands
{
    class IpCommand
    {
        int OnExecute(CommandLineApplication app, IConsole console)
        {
            foreach (var iface in GetAvailableNetworkInterfaces())
            {
                console.Out.WriteLine($"Interface name: {iface.Name}");
                foreach (var ipInfo in iface.GetAvailableIPAddress())
                {
                    string ipTypeDisplay;

                    if (ipInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipTypeDisplay = "IPv4";
                    }
                    else if (ipInfo.Address.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        ipTypeDisplay = "IPv6";
                    }
                    else
                    {
                        throw new NotSupportedException($"Not supported this ip address family {ipInfo.Address.AddressFamily}");
                    }

                    console.Out.WriteLine($"{ipTypeDisplay} | {ipInfo.Address}");
                }
            }

            return 0;
        }
    }
}
