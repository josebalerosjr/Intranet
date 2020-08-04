namespace Intranet.Utilities
{
    public class EmailOptions
    {
        public string SMTPHostClient { get; set; }
        public int SMTPHostPort { get; set; }
        public bool SMTPHostBool { get; set; }
        public string AuthEmail { get; set; }
        public string AuthPassword { get; set; }
        public string AuthDomain { get; set; }
    }
}