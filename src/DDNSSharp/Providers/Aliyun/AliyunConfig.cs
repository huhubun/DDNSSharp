namespace DDNSSharp.Providers.Aliyun
{
    class AliyunConfig : IProviderConfig
    {
        /// <summary>
        /// AccessKeyId
        /// </summary>
        public string Id { get; set; }

        public string Secret { get; set; }
    }
}
