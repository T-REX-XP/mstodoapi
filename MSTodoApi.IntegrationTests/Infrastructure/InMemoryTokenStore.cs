using Newtonsoft.Json.Linq;

namespace MSTodoApi.IntegrationTests.Infrastructure
{
    public class InMemoryTokenStore : ITokenStore
    {
        private string _refreshToken = "MCVVdR8TDtVK3Is6Xzs52l7upUEd96GcziCXFlER2VYFXZPixsap7c9zewlotNkyybcHydCiyFxg2ohXLZ*5CiNI42d1LusLmoPX7mdvpZLVEwfnN5LjNY8cYvA!FyiqmZzQ2TiMGD4RnrcgB*xhR1J85KHnCmO4bTibYz3QyvmuAy*iBMDw!yUXoueEVGxuQ8fsWnx7wdD*pq*Jjg2A48i8hVfKo6sK0cb5SRiNsyUyKAQ*5dO6jCXBEl1T97EHT7v0XRvkdziCDKJTTTue8dxKmPJ0wBqIV3Vd4lvFnInObbwCvWvoVVBJhrT1BbY*KAXkJ8O2cMRPgNTpYriEdk3iCRqvtDk35kGyFXN3QKO4CWE9YP1YyOaRC0yjJd1tYcGO9TOF0Ad15fl7yqRRbohGGbEwVbDAeFXE!HeoUNskULV2ZZG752dGPyl5NTWOy77qcBHsldoEyy6xbU17dIvBU3ttJ8m7sG7POYIjyyxqFTZ9QG9R7iViNK6uhB4ZBtxjNTgm*aXi70E3Kh44XJE8FJPHsIldcflk1upNzlCqxcgbQR5ab0aAAD8TMxmXjz53ZEZlvvrJBADXX7N9*ZUetOuj*L3hBuN8aSLkADB2Zxkwj3UwRPSypSD4*rgMCQA$$";
        private string _accessToken = "EwA4A+l3BAAUWm1xSeJRIJK6txKjBez4GzapzqMAAfeBgLKRPP/+mJKjw4MvBZfmZ49ZaIA8NcD7hq/YYuu+xmx0aPcSihmzedMfxvOxaRooEshJiGSR/AnHZ/SB639gQWbK02jAq3qpRdXKjqHpXnKKzMHPYrywovHJMyiyZnOaMNKQhoqAJYVlnXyXq9jAFr5Ymku+bhNgBcYOm+lOqzbjC5mlorKFeb3kTkcNE+No/OGG9/5iWGu2PmA91PVLwrr6WcBSsrEh61jVS/UNmZE52VbFNh7fNtaAHcV+9eqiMQkatn1qDH0o/q2lZo0dVX641L1ogZAK4wn6CJ/3Yr0ZlhPbaflQRBgNGesWJCuIfp53vAgWLQvLfDriP6IDZgAACCFOiACxIunMCAJWouc1v0yV5nCSsc0sOAQJSC2kl2xesPpP5HceR6fMKeRJr5JXFmSmXhv7jTge81wIDp7D3YJ4FQDEKiliqv3zEoQEkN0tQh2hbRW86Dbqf2yuL03QmDBqvPIF4wPckZ7SeTTzf7Go7vhwFqYdfbCC6/X5LNni2x0sexxc8xoi5WdEuZNWaBZG6LKfXDwZHf1EyqkX9F7K2IGITpq/Cf733cY6l2uztrCwTp3StnIf04kRJii6yiRmkDcTOY0mnRJWfY5XHeL1T2NjqlAXzuzHngf1QvlyUZjCdiyKC9eZBRN/hbRzGwvLyt7Komk3duDjAOG6exNEbfY2LUlHA7iYQbCGluT+gvFbbqwXyRgg6gBDII8svSgJwaL9fbhNAN8gJWOwYd6B9mHksAUyernmMRx9PcQ7SqkNZc5r0VsjNX1E2HmzmFXtpN2ZbbbjUGioB64SFoKSmMEYEM/fvL+tEeQQysncvsa8+0dJX1BX/DbxkSMzoFWUd1cdtw8Gdesz1na2VcCYQDvP3piHlGgftZtMoMiBgh97z9QQ/TJRg2LFiUZ1c/pGMp07s2s0BhHPISayTbxQZkDfD6XC7gMDRttLJLjVasbXJ+KdiP3oOkpRH3OB1aef2uACQwNscJbOzdF7E8744cqMALE9K58YLK6EE1MN9bMjcrYvaP++WIGfET3wusXIOQI=";

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
            JObject jToken = JObject.Parse(json);
            
            _accessToken = jToken["access_token"].ToString();
            _refreshToken = jToken["refresh_token"].ToString();
        }
    }
}