# DDNSSharp

A cross-platform DDNS tool.  
一款跨平台 DDNS 工具。

## 安装
- 需要 [.NET Core 3.1 Runtime](https://dotnet.microsoft.com/download/dotnet-core)
- [下载](./releases)

## Domain management 域名管理

想要让域名解析跟随本机的 IP 地址变化而变化，需要将域名添加到 DDNSSharp 中。

### list

`ddnssharp list`

显示 DDNSSharp 中记录的域名列表。

#### Sample
```
$ dotnet DDNSSharp.dll list

ip.bun.plus   | AAAA | aliyun 
ipv6.bun.plus | AAAA | aliyun
```

### add

`ddnssharp add`

向 DDNSSharp 添加域名信息。

#### Sample
```
$ dotnet DDNSSharp.dll add ip.bun.plus --provider aliyun --type AAAA --interface eth0

Added successfully!
```

### delete

`ddnssharp delete`

从 DDNSSharp 中删除域名信息。

#### Sample
```
$ dotnet DDNSSharp.dll delete ipv6.bun.plus

Please confirm that you want to delete this record:
  Domain: ipv6.bun.plus
  Type: AAAA
  Provider: aliyun
 [y/N] y
Deleted successfully!
```

### ip

`ddnssharp ip`

展示 DDNSSharp 用于更新域名解析使用的 IP 信息。（如果同一网卡，同一类型有多个地址，将使用第一个。）

#### Sample
```
$ dotnet DDNSSharp.dll ip

Interface name: eth0
IPv4 | 10.0.0.20
IPv6 | 2408:1111:2222:3333:4444:5555:6666:7777
Interface name: wlan0
IPv4 | 10.0.0.46
IPv6 | 2408:1111:2222:3333:4444:5555:6666:8888
```

### sync

`ddnssharp sync`

将本机 IP 信息同步到指定域名的解析记录中。

#### Sample
```
$ dotnet DDNSSharp.dll sync

Current domain: ipv6.bun.plus
Try use sub domain api to search. SubDomain = ipv6.bun.plus, Type = AAAA
Is sub domain
Search succeed: id = 2333333333333333, RR = ipv6, currentValue = 2408:2333:2333:2333:2333:2333:2333:2333
Interface eth0 InterNetworkV6 address is 2408:1111:2222:3333:4444:5555:6666:7777
ipv6.bun.plus success
```

## Provider management 域名解析提供商管理

DDNSSharp 通过域名解析提供商的 API 来修改解析记录，通过 `provider` 下的命令来设置访问 API 所需的必要信息。**请确保您提供的信息拥有操作域名解析记录的权限。**

### 支持列表

下列域名解析提供商已被 DDNSSharp 支持：

- 阿里云
- Cloudflare *(coming soon)*

### 查看支持的和已设置的提供商信息

`ddnssharp provider`

#### Sample
```
$ dotnet DDNSSharp.dll provider

List of supported providers:
  cloudflare
  aliyun

List of already configured providers:
  aliyun
```

### set

`ddnssharp provider set`

设置域名解析提供商信息。

#### Sample
```
$ dotnet DDNSSharp.dll provider set aliyun --id oooo --secret xxxx

Now start to set up 'aliyun' provider.
Saved.
```

#### Note
由于不同域名解析提供商所需的参数不同，请通过 `provider set --help` 命令来查看每个提供商使用的参数列表。

```
$ dotnet DDNSSharp.dll provider set --help

Usage: ddnssharp provider set [options] <Name> [[--] <arg>...]

Arguments:
  Name          Provider name

List of supported providers:
  cloudflare
  aliyun

Options:
  -?|-h|--help  Show help information

[Note] Different providers have different options
Provider 'cloudflare' supports:
  --id        Cloudflare Account_ID
  --token     Cloudflare Token
Provider 'aliyun' supports:
  --id        aliyun accessKeyId
  --secret    aliyun accessSecret
```

### delete

`ddnssharp provider delete`

移除域名解析提供商信息。

#### Sample
```
$ dotnet DDNSSharp.dll provider delete aliyun

Now start to delete the configuration of 'aliyun' provider.
Deleted.
```
