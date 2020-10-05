namespace Intranet.Classes
{
    /**
     *  this class gets the  value from appsettings.json
     */

    public class AppSettings
    {
        public string SmtpHostClient { get; set; }
        public int SmtpHostPort { get; set; }
        public bool SmptHostBool { get; set; }
        public string AuthEmail { get; set; }
        public string AuthPass { get; set; }
        public string AuthRoles { get; set; }
        public string appDomain { get; set; }
        public string SenderName { get; set; }
        public string DefaultCNC { get; set; }
        public string DevSPEM { get; set; }
        public string SPEMLink { get; set; }
    }
}