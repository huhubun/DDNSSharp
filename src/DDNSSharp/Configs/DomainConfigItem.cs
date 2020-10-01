using DDNSSharp.Enums;
using System;
using System.Diagnostics.CodeAnalysis;

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

        /// <summary>
        /// DNS 提供商
        /// </summary>
        public string Provider { get; set; }

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
