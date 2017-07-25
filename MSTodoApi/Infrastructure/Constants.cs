namespace MSTodoApi.Infrastructure
{
    public static class Constants
    {
        public static string OutlookBaseAddress = "https://outlook.office.com/api/v2.0/";
        public static string TokenUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
        public static string SelectedTaskFields = "Subject,Id";
        public static string SelectedEventFields = "Subject,Start,End";
        
    }
}