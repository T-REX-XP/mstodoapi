using Newtonsoft.Json.Linq;

namespace MSTodoApi.Infrastructure.Auth
{
    public class InMemoryTokenStore : ITokenStore
    {
        private string _refreshToken = "MCRtJqUTCO5mtksYKPJ*6WvIG8zl8Zi8c7bxNFSvX7NHdFfqbd3Tsh4vVOoKNPj4w5lTpQZUvgU1eY8QuJc1ejnyXC9zGBOf8Psk0Wz2d6LLL*8Yxwa18SQPccb5Q1cPOq3LAGLEf2mCS4u3teeV8y3K21LloaWJ6*yPM9FozmXSjMpj7IavJm3F0rHioEOwMVE8cQi0unhae8o!Edo!utfxsEA0H6pilPLwgFQF4OBezRmrPfmOfrob4WyufPPCenHFrvR!y9ANaObfssFR2!RheCmrtJiNyU91q7MDOmR!TwKME46Z8RN!aGtzM3mRNc67X0jwsMpPnGIIC3KCjHIEcHwPWazwpL1pcxXknWXxhg79j6meGwdJf24NREBnxIcj24RiPRKtwTZzNNqO*KmT7ATxDSHgOfOzTg432Vhpl8ndYRRK0S8WXiWTjNK4PXr8QmD8WI!gosgMukVZVGcdOtku1Bf8IW8vxOVJXf!mL*qGBzDneOJ6wJClHegyrqLF!E13mP4fOfPyBRfkMJc7VRANtK8W71QfgRzNuKiqguQYucYyQZYegE2dnayBt9fl3qPuUlCabas12s*kBxCsIklgXB!2EAD4uZzghS8IPDMIHXXfaUkdIoksQzait!Q$$*f6jI6TX4*0bkFbsNvbYhZgwGV2pECImsGffmy9pHutg0h4!*iA28VUmL!i6pPSP3qPTDYEHJ*mxGpQxfZ4Dgnv3j0KNpvBdq6newPCQcR2RnwO8D2MzgvSmK8*h9KvID7W5vgQpizOLQZ2mTMZ2WYX351p9Nb8q5tdqlt8VtdjZH1RWg4fAbjcPlyLQJkVJ02JBv6LJgVxXrIvoNkq5XxaSMEDiiL!7ftM54eirVfp2EIzCYH8z1N7*Qjhiq3vgER70I2N*wNZs5Pecms9GoY3ypn1!TJ9JNoZqR!Mwzyyfmxdj619BzOWX02FzHR2Lpomx3wwx6MazLQWRI3mgkfkm*zM4erEPJpRzcOzPrWGgY7aPvviY*k2DsHbVBtxHa7RFv7Zrw0dQ1XZxgvT9OiqCTcbnwESjAJhAASNwRYaz!qtF2tqe2ULpuswIuLt6jdZ4yCVHHgR2WQOu3e8b3pK0KfgdLb4NwzUq9XhXm8Cya5BKD*eIAuy1PFptloZ4QPHdpXpZLTN5Z4p0w!YAzzsw$$";
        private string _accessToken = "EwAwA+l3BAAUWm1xSeJRIJK6txKjBez4GzapzqMAATO6lo1gXaK0gOreUa55roS6pA37rfop7wJ0c5TPX538Mw6MvZIdo0QzZXdYBMTa00AeidoCcWgVOf+7Wm+ZEfgsuTknnic1ylvKnLLk4Laak38nfMwXIkRhWxjipNnKFkw36I6dtgyWixHtFKo3BgdDU6OYMGEdaKNVR6BHGLMrN0IxfznvIXXptrSU88Pdj54xnGVIVnJjj05EmoASF3YnhVdHpbQh3faUqK1T4/ecpeHVF66u2MJNf8gSlR2EFvVlMCGFwpok86qgUA36Y9Ai9eVAMvuSwIsyYRV4eMU40JFBzkJfM55YUgOLGm7OcbLRKt+kzeOc4ALP/gW6W6cDZgAACOEiMRsQRyv2AAJ2ZLcqj9A5yB0FKqxBTZcFpFDIgRcALcPE3Ky+U5+vdCs9TpG0eSKcfsalYAmlB2kxkaWmo7PZ3y+DO/kSA/2C02xdaajGlYcbUvsLvSJNtIDd0cTmb+sArAsqhSAaJPGwZO8lwO5xScuxnDjZKw5VQ3q5cUW2LkF+nO1ifnSKtTUUv9kUvEnAhUnJgNzw67I5qc29fr7H5O79weiKEMLxo4g7QJfoFBp/InvR8+SFkoXalJ6278grtU3fxXloneMgdM3iZXECvnHLmPtPhm31P2V1eVKQxo22Ip9UrynXxSXcCfQzIjKcHKQEb0vaOzyjSMN7tKxbhKjKG0uhs3kNM904gkOzb33acb4effhKaJqh2xXGi0T0kusIZLvI+hI8UYsBN28HDIkmpCcqyx2toDa0WCe4jwj/xL0KWwdyRAOySfm8wOpvNreNP61UBSOB7UoMvyhwV73Gp55cTQOTwOYs0QKBuuwKkL0FYnJeIsEGBWVQ+RqBktjLJK/IvtSPW3dWCIybKlJNQ6Lo98ftkrVTisNENwT0/Xp81eYMTNfXDWMsNl2Myi0MPpvNE1sVB1eDzW3o8EoQYSApAPuknvR2YGDNvkxB65O1XFG/Y8aLr3iL1H57NyPfNf5R9DzenqeVssJzX+fFNlWQ6JDiv3zt8qNI80QiZBM6SBUsBTkC";

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