namespace MSTodoApi.Infrastructure
{
    public static class Constants
    {
        public static string OutlookBaseAddress = "https://outlook.office.com/api/v2.0/";
        public static string SelectedTaskFields = "Subject,Id";
        public static string SelectedEventFields = "Subject,Start,End";

        public static string TokenHeaderKey = "X-MSOutlookAPI-Token";
    }
}