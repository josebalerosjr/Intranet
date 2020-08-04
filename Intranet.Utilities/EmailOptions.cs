namespace Intranet.Utilities
{
    public class EmailOptions
    {
        public string SMTPHostClient { get; set; }
        public int SMTPHostPort { get; set; }
        public bool SMTPHostBool { get; set; }
        public string AuthEmailQshe { get; set; }
        public string AuthPasswordQshe { get; set; }
        public string AuthEmailCorpComm { get; set; }
        public string AuthPasswordCorpComm { get; set; }
        public string AuthEmailMain { get; set; }
        public string AuthPasswordMain { get; set; }
        public string AuthDomain { get; set; }
        public string DefaultCNC { get; set; }
        public string SPEMLink { get; set; }
    }
}