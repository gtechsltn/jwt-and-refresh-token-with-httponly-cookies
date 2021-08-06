namespace Api.Testing.Dtos
{
    public class StoreListResponse
    {
        public string Id { get; set; }
        public string StoreName { get; set; }
        public string StoreCD { get; set; }
        public string Province { get; set; }
        public long VideoCount { get; set; }
        public string DataUsage { get; set; }
        public int ThisMonthPVCount { get; set; }
        public int LastMonthPVCount { get; set; }
        public int Status { get; set; }
        public string ReviewState { get; set; }
        public bool IsMasterStore { get; set; }
    }
}