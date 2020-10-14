using DDNSSharp.Helpers;

namespace DDNSSharp.Configs
{
    public static class DomainConfigItemExtension
    {
        public static bool IsIPChanged(this DomainConfigItem configItem)
        {
            var currentAddress = IPHelper.GetAddress(configItem.Interface, configItem.AddressFamily);
            var lastAddress = configItem.LastSyncSuccessCurrentIP;

            return currentAddress != lastAddress;
        }
    }
}
