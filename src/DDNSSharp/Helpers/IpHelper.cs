using DDNSSharp.Exceptions.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace DDNSSharp.Helpers
{
    public static class IPHelper
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

        public static string GetAddress(string interfaceName, AddressFamily addressFamily)
        {
            var iface = NetworkInterface.GetAllNetworkInterfaces().SingleOrDefault(i => i.Name == interfaceName);
            if (iface == null)
            {
                throw new InterfaceNotFoundException();
            }

            var address = iface.GetAvailableIPAddress().FirstOrDefault(a => a.Address.AddressFamily == addressFamily);
            if (address == null)
            {
                throw new AddressFamilyNotFoundException();
            }

            return address.Address.ToString();
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
