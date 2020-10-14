using Aliyun.Acs.Alidns.Model.V20150109;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using DDNSSharp.Attributes;
using DDNSSharp.Configs;
using DDNSSharp.Helpers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDNSSharp.Providers.Aliyun
{
    [Provider(Name = "aliyun")]
    class AliyunProvider : ProviderBase
    {
        public AliyunProvider(CommandLineApplication app)
        {
            App = app;
        }

        public static new IEnumerable<ProviderOption> GetOptions()
        {
            yield return new ProviderOption("--id", "aliyun accessKeyId", CommandOptionType.SingleValue);
            yield return new ProviderOption("--secret", "aliyun accessSecret", CommandOptionType.SingleValue);
        }

        public override void Sync(IEnumerable<DomainConfigItem> domainConfigItems)
        {
            var config = GetConfig<AliyunConfig>();

            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", config.Id, config.Secret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            client.SetConnectTimeoutInMilliSeconds(60000);
            client.SetReadTimeoutInMilliSeconds(60000);

            foreach (var item in domainConfigItems)
            {
                Console.WriteLine($"Current domain: {item.Domain}");

                // 更新该记录的同步时间
                item.LastSyncTime = DateTime.Now;

                // 更新解析记录使用的请求，为了便于 catch 时获取相关数据，所以定义在 try 块外面
                UpdateDomainRecordRequest updateRequest = null;

                try
                {
                    string recordId;
                    string rr;
                    string value;

                    var domainName = item.Domain;
                    var domainTypeString = item.Type.ToString();

                    // 先用获取子域名的 API DescribeSubDomainRecordsRequest 进行尝试，如果查询不到该子域名，再尝试使用 DescribeDomainRecordsRequest 进行根域名查询
                    Console.WriteLine($"Try use sub domain api to search. SubDomain = {domainName}, Type = {domainTypeString}");

                    var subQueryRequest = new DescribeSubDomainRecordsRequest
                    {
                        SubDomain = domainName,
                        Type = domainTypeString
                    };

                    var subQueryResponse = client.GetAcsResponse(subQueryRequest);
                    if (subQueryResponse.TotalCount > 0)
                    {
                        Console.WriteLine("Is sub domain");

                        // 虽然 DescribeSubDomainRecordsRequest 没有问题，安全起见还是过滤一下（具体问题参见后面 DescribeDomainRecordsRequest 的注释）
                        var result = subQueryResponse.DomainRecords.SingleOrDefault(d => d.Type == domainTypeString);
                        recordId = result?.RecordId;
                        rr = result?.RR;
                        value = result?._Value;
                    }
                    else
                    {
                        Console.WriteLine($"Not sub domain. Try use root domain api to search. SearchMode = EXACT, KeyWord = @, DomainName = {domainName}, TypeKeyWord = {domainTypeString}, Type = {domainTypeString}");

                        // 由于 Type 和 TypeKeyWord 属性实测没有作用，过滤不了域名类型
                        // 所以得到查询结果后还需要再手动过滤一次
                        var rootQueryRequest = new DescribeDomainRecordsRequest
                        {
                            SearchMode = "EXACT",
                            DomainName = domainName,
                            KeyWord = "@",
                            TypeKeyWord = domainTypeString,
                            Type = domainTypeString
                        };

                        var rootQueryResponse = client.GetAcsResponse(rootQueryRequest);
                        var result = rootQueryResponse.DomainRecords.SingleOrDefault(d => d.Type == domainTypeString);
                        recordId = result?.RecordId;
                        rr = result?.RR;
                        value = result?._Value;
                    }

                    Console.WriteLine($"Search succeed: id = {recordId}, RR = {rr}, currentValue = {value}");

                    if (recordId == null)
                    {
                        throw new KeyNotFoundException($"Update failed: cannot find a record for the domain name {item.Domain}.");
                    }

                    var address = IPHelper.GetAddress(item.Interface, item.AddressFamily);
                    Console.WriteLine($"Interface {item.Interface} {item.AddressFamily} address is {address}");

                    updateRequest = new UpdateDomainRecordRequest
                    {
                        RecordId = recordId,
                        RR = rr,
                        Type = domainTypeString,
                        _Value = address
                    };

                    var updateResponse = client.GetAcsResponse(updateRequest);

                    Console.WriteLine($"{item.Domain} updated successfully: {value} -> {address}");
                    Console.WriteLine();

                    item.IsLastSyncSuccess = true;
                    item.LastSyncSuccessIP = address;
                    item.LastSyncSuccessTime = DateTime.Now;
                }
                catch (ClientException ex)
                {
                    // 如果更新时返回了 DomainRecordDuplicate 错误
                    // 有可能是本地记录的 LastSyncSuccessIP 和机器实际的 IP 以及域名解析提供商一侧的地址不匹配（例如有段时间里程序没有执行，但 IP 发生了改变，就会造成 LastSyncSuccessIP 和提供商处的 IP 地址，以及实际地址不匹配）
                    if (ex.ErrorCode == "DomainRecordDuplicate" && !String.IsNullOrEmpty(item.LastSyncSuccessIP) && updateRequest._Value != item.LastSyncSuccessIP)
                    {
                        item.IsLastSyncSuccess = false;
                        item.LastSyncSuccessIP = updateRequest?._Value;

                        App.Out.WriteLine($"The update failed because the IP address recorded in the cache does not match the actual IP address and the IP address stored by the provider. The cache record has been updated, you can ignore this prompt.");
                    }
                    else
                    {
                        App.Error.WriteLine($"{item.Domain} update failed");
                        App.Error.WriteLine(ex);
                    }
                }
                catch (Exception ex)
                {
                    item.IsLastSyncSuccess = false;

                    Console.WriteLine(ex);
                }
                finally
                {
                    DomainConfigHelper.UpdateItem(item);
                }
            }
        }
    }
}
