using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace DDNSSharp.Helpers
{
    public static class IpHelper
    {
        /// <summary>
        /// 获取可用的网卡信息。当含有 IPv4 地址，或 Scope 为 Global 类型的 IPv6 地址时，认为其可用。
        /// </summary>
        /// <returns></returns>
        public static List<NetworkInterface> GetAvailableNetworkInterfaces()
        {
            return NetworkInterface
                        .GetAllNetworkInterfaces()
                        .Where(i => i.OperationalStatus == OperationalStatus.Up)
                        .Where(i => i.GetAvailableIPAddress().Any())
                        .ToList();
        }
    }

    public static class NetworkInterfaceExtensions
    {
        public static IEnumerable<UnicastIPAddressInformation> GetAvailableIPAddress(this NetworkInterface iface)
        {
            foreach (var ipInfo in iface.GetIPProperties().UnicastAddresses)
            {
                var address = ipInfo.Address;
                if (
                    address.AddressFamily == AddressFamily.InterNetwork ||
                    (address.AddressFamily == AddressFamily.InterNetworkV6 && address.ScopeId == 0))
                {
                    yield return ipInfo;
                }
            }
        }
    }
}
