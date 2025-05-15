namespace NextWebApi.DTOs
{
    public class MemoDTO
    {
        public int TotalCount { get; set; }
        public int CompletedCount { get; set; }
        public string CompletionsPercentage {
            get
            {
                if (TotalCount == 0)
                {
                    return "0.00%";
                }
                return $"{(double)CompletedCount / TotalCount * 100:0.00}%";
            }
                
        }
    }
}
