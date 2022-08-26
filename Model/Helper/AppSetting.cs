namespace EmailScheduler.Model.Helper
{
    public class AppSetting
    {
        public string Smtp { get; set; }
        public string EmailFrom { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public string UserFile { get; set; }
        public string MessageFile { get; set; }
    }
}
