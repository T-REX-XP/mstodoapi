using Newtonsoft.Json.Linq;

namespace MSTodoApi.Infrastructure.Auth
{
    public class InMemoryTokenStore : ITokenStore
    {
        private string _refreshToken = "MCQfTtNyUUqlreSA8XQEQa4v4D19QMutTOOp5seR4kuAlxBjXE3dHaMhd98azf6MD28cN45iwprkz1QqqnCN6BXueDbU0uKvlgxiSRcEWVb*f6jI6TX4*0bkFbsNvbYhZgwGV2pECImsGffmy9pHutg0h4!*iA28VUmL!i6pPSP3qPTDYEHJ*mxGpQxfZ4Dgnv3j0KNpvBdq6newPCQcR2RnwO8D2MzgvSmK8*h9KvID7W5vgQpizOLQZ2mTMZ2WYX351p9Nb8q5tdqlt8VtdjZH1RWg4fAbjcPlyLQJkVJ02JBv6LJgVxXrIvoNkq5XxaSMEDiiL!7ftM54eirVfp2EIzCYH8z1N7*Qjhiq3vgER70I2N*wNZs5Pecms9GoY3ypn1!TJ9JNoZqR!Mwzyyfmxdj619BzOWX02FzHR2Lpomx3wwx6MazLQWRI3mgkfkm*zM4erEPJpRzcOzPrWGgY7aPvviY*k2DsHbVBtxHa7RFv7Zrw0dQ1XZxgvT9OiqCTcbnwESjAJhAASNwRYaz!qtF2tqe2ULpuswIuLt6jdZ4yCVHHgR2WQOu3e8b3pK0KfgdLb4NwzUq9XhXm8Cya5BKD*eIAuy1PFptloZ4QPHdpXpZLTN5Z4p0w!YAzzsw$$";
        private string _accessToken = "EwAwA+l3BAAUWm1xSeJRIJK6txKjBez4GzapzqMAAfuaeC0y8JlMOrFDnzdOoFD75YGcxHJV4PZYA6JFEwU7GeA7jbOfDCpVLElyyWX5qsc+KYA8aGlUSzfustgHCM9W+MgA+MlS/t8RoAqtTIeZwYsatwyHZXk0whU4z+LBWJrCV+sElt6I4T0JYWulHWvX7HGeZ3HSxzntCE+ChYiurAz41v/VbQiY86wKrXABF1+7kbvNLWEpyTtSV8mZxYeIf5S7VzAs6pqzaZ7m2kll/5iz5AJ6TwFCv4ADoeskXMbM64Gq3Lvo3appCDb4S1PvQNh9fvA044h6L29hoI7R6kDBb84d6Atbyq4vmZACiWaYTQTc050U57i1Ne4LSx4DZgAACLW+Z80yWjQdAALC1+cknTFrWDnUjX0aNb6d64LOoBrKiEkVZI+LSQniAhoyEf9TCjpzzk35cagYWvYX5qOcMSiX06LvKLEiiWciFCfurg0YAKFDiuBr8KGa1hC1sc08TPr4k80BilCLa/EWaxgUd4poZ58QA8tIjkwEUA2pmbCafBZVwVAWVLb2NvbsIwtR73CkfF8kP2dIh+VJGn5NwBdgEesnTfyHzzLrKfviUQOrBHDUcUBUCCbkZLNKEVWR8AWoc0kgh681VyoiZkNBPTE2UaC2aTOM8xNMhkUxeRjkCZGEGiIgJhh/ykvIpyQ4nGSP7PwsZSJsTNBCIoiOGw7bWJHWIRMzJ4pApckHecevaoVU1bXDQpAkhWRFMz+lSIHzodxsTEHJikRbdgOdcgl8pHoNFGlWzTRFwhXiwzGs+CKYP7w12Qds+/xuV3UmmTby5QQoBHzgoGPn7NH4H8KHR7Yat9Hl3hmpokNaGSSNk3xhyPv8uIhIETifBylDH1UhptQniUpxiDE5Mwqhtk0YmWIUT0l592eEPCTaEIYCCqDiM8Bsks5QpfL/dP7uEzkr1+U6m9p0Qg65UkM5ax+Kt/FZILaYjm+pbqVZjXBn3nc565bDcZ4nL9rw7I/7QfEmX/Goff+bov/+CpecXsTPeCYuk3fbeg5HcwUkxvZrW2dYhw7BaHbK1TkC";

        public string AccessToken
        {
            get => _accessToken;
            set => _accessToken = value;
        }

        public string RefreshToken
        {
            get => _refreshToken;
            set => _refreshToken = value;
        }

        public void UpdateTokens(string json)
        {
            JToken jToken = JToken.Parse(json);
            
            _accessToken = jToken["access_token"].ToString();
            _refreshToken = jToken["refresh_token"].ToString();
        }
    }
}