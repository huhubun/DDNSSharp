namespace DDNSSharp.Commands.SyncCommands
{
    public enum SyncStatus
    {
        /// <summary>
        /// 无状态，例如域名刚刚被添加但尚未进行同步时将被标记为该状态
        /// </summary>
        None,

        /// <summary>
        /// 同步成功
        /// </summary>
        Success,

        /// <summary>
        /// 同步失败
        /// </summary>
        Failure,

        /// <summary>
        /// 忽略，当 IP 地址没有变化时将被标记为该状态
        /// </summary>
        Ignore
    }
}
