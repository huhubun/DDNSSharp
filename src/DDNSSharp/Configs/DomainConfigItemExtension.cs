using DDNSSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDNSSharp.Configs
{
    public static class DomainConfigItemExtension
    {
        public static bool IsIPChanged(this DomainConfigItem configItem)
        {
            var currentAddress = IPHelper.GetAddress(configItem.Interface, configItem.AddressFamily);
            var lastAddress = configItem.LastSyncSuccessIP;

            return currentAddress != lastAddress;
        }
    }
}
