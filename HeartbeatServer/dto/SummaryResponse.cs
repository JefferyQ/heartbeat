namespace HeartbeatServer
{
    public class SummaryResponse
    {
        public string[] Servers { get; set; }
        public string[] Applications { get; set; }
        public string[] Methods { get; set; }
        public int TotalAppStats { get; set; }
    }
}