# DDNSSharp

A cross-platform DDNS tool.  
一款跨平台 DDNS 工具。

## 安装
- 需要 [.NET Core 3.1 Runtime](https://dotnet.microsoft.com/download/dotnet-core)
- [下载 DDNSSharp](https://github.com/huhubun/DDNSSharp/releases)

## 使用
1. 通过 `add` 命令将域名添加到 DDNSSharp 中
1. 通过 `crontab` 等方式配置定时任务，每隔 `1` 分钟执行一次 `sync` 命令，例如 `* * * * * /usr/bin/dotnet/dotnet /home/pi/ddnssharp/ddnssharp.dll sync`

## 命令

对于 Windows 平台，可以通过可执行文件来使用：

```bash
ddnssharp.exe [options]
```

对于 Linux 平台，确保 `ddnssharp` 文件具有可执行权限：

```bash
ddnssharp [options]
```

macOS 受到公证[<sup>1</sup>](https://developer.apple.com/cn/documentation/xcode/notarizing_macos_software_before_distribution/) <sup>[2](https://docs.microsoft.com/zh-cn/dotnet/core/whats-new/dotnet-core-3-1#macos-apphost-and-notarization)</sup>的影响，不提供 macOS 的 DDNSSharp 可执行文件。但不论 Windows 平台、Linux 平台还是 macOS 平台，都可以通过 `dotnet` 命令使用 DDNSSharp：

```bash
dotnet DDNSSharp.dll [options]
```

### `list`

显示 DDNSSharp 中记录的域名列表。

#### Sample
```
$ ddnssharp list

ip.bun.plus   | AAAA | eth0 | aliyun | Success | 12/11/2020 00:19:29 
ipv6.bun.plus | AAAA | eth0 | aliyun | Success | 12/11/2020 00:19:26 
```

### `add`

向 DDNSSharp 添加域名信息。

#### Sample
```
$ ddnssharp add -t AAAA -i eth0 -p aliyun --ali-access-key-id YOUR_ACCESS_KEY_ID --ali-secret YOUR_SECRET ipv6.bun.plus

Added successfully!
```

#### Note
由于不同域名解析提供商所需的参数不同，请通过 `ddnssharp add --help` 命令来查看每个提供商使用的参数列表。

### `delete`

从 DDNSSharp 中删除域名信息。

#### Sample
```
$ ddnssharp delete ipv6.bun.plus

Please confirm that you want to delete this record:
  Domain: ipv6.bun.plus
  Type: AAAA
  Provider: aliyun
 [y/N] y
Deleted successfully!
```

### `ip`

展示 DDNSSharp 用于更新域名解析使用的 IP 信息。（如果同一网卡，同一类型有多个地址，将使用第一个。）

#### Sample
```
$ ddnssharp ip

Interface: eth0
IPv4 | 10.0.0.20
IPv6 | 2408:1111:2222:3333:4444:5555:6666:7777

Interface: wlan0
IPv4 | 10.0.0.46
IPv6 | 2408:1111:2222:3333:4444:5555:6666:8888
```

### `sync`

将本机 IP 信息同步到指定域名的解析记录中。

#### Sample
```
$ ddnssharp sync

Current domain: ipv6.bun.plus
Try use sub domain api to search. SubDomain = ipv6.bun.plus, Type = AAAA
Is sub domain
Search succeed: id = 2333333333333333, RR = ipv6, currentValue = 2408:2333:2333:2333:2333:2333:2333:2333
Interface eth0 InterNetworkV6 address is 2408:1111:2222:3333:4444:5555:6666:7777
ipv6.bun.plus success
```

### `provider`

查看支持的域名解析提供商名称

#### Sample
```
$ ddnssharp provider

aliyun
```

## 编译
- 需要 .NET Core 3.1 SDK