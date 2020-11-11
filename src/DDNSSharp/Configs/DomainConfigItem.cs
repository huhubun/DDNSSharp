using DDNSSharp.Commands.SyncCommands;
using DDNSSharp.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Text.Json.Serialization;

namespace DDNSSharp.Configs
{
    public class DomainConfigItem : IEquatable<DomainConfigItem>, IComparable<DomainConfigItem>
    {
        /// <summary>
        /// 域名
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 域名记录类型
        /// </summary>
        public DomainRecordType Type { get; set; }

        [JsonIgnore]
        public AddressFamily AddressFamily => Type switch
        {
            DomainRecordType.A => AddressFamily.InterNetwork,
            DomainRecordType.AAAA => AddressFamily.InterNetworkV6,
            _ => throw new NotSupportedException($"Not supported domain type '{Type}'.")
        };

        /// <summary>
        /// DNS 提供商
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// 访问 DNS 提供商 API 时需要用到的参数信息
        /// </summary>
        public Dictionary<string, string> ProviderInfo { get; set; }

        /// <summary>
        /// 该域名的 IP 地址来源于哪个网卡
        /// </summary>
        public string Interface { get; set; }

        /// <summary>
        /// 上次同步结果
        /// </summary>
        public SyncStatus LastSyncStatus { get; set; }

        /// <summary>
        /// 上次同步的时间
        /// </summary>
        public DateTime? LastSyncTime { get; set; }

        /// <summary>
        /// 上次同步成功时的时间
        /// </summary>
        public DateTime? LastSyncSuccessTime { get; set; }

        /// <summary>
        /// 上次同步成功前的 IP 地址
        /// </summary>
        public string LastSyncSuccessOriginalIP { get; set; }

        /// <summary>
        /// 上次同步成功后的 IP 地址
        /// </summary>
        public string LastSyncSuccessCurrentIP { get; set; }

        public int CompareTo([AllowNull] DomainConfigItem other)
        {
            const int EQUALS = 0;

            var domainResult = Domain.CompareTo(other.Domain);
            if (domainResult != EQUALS)
            {
                return domainResult;
            }

            var typeResult = Type.CompareTo(other.Type);
            if (typeResult != EQUALS)
            {
                return typeResult;
            }

            var providerResult = Provider.CompareTo(other.Provider);
            if (providerResult != EQUALS)
            {
                return providerResult;
            }

            return EQUALS;
        }

        public bool Equals([AllowNull] DomainConfigItem other)
        {
            return Equals(other, ignoreProvider: false);
        }

        public bool Equals([AllowNull] DomainConfigItem other, bool ignoreProvider)
        {
            var result = Domain == other.Domain &&
                           Type == other.Type;

            if (!ignoreProvider)
            {
                result = result && (Provider == other.Provider);
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj is DomainConfigItem)
            {
                return Equals(obj);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Domain, Type, Provider);
        }
    }
}
