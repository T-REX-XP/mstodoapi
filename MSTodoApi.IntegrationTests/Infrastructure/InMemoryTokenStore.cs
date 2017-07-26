using Newtonsoft.Json.Linq;

namespace MSTodoApi.IntegrationTests.Infrastructure
{
    public class InMemoryTokenStore : ITokenStore
    {
        private string _refreshToken = "MCTiZcZLEWwZwXxEAW1ngvFxAtfhi3wW4QKmsT!*JRNhFe6MB*D0nWCOTcGGbzPQmBGzmp5TpYLANKQ2RjnIxnf51c0bSfY4fr4bLQWCRI72JvC9pCS1qa9QXW*eOXAQlGMgR5BJ3wBZs8fswubFpPY!4b12AtquD7zssgFxUuJnX3JD8WyXg57C5Ld0mMGOX8nQRtQM1kgqtCZKdevex1FsSaARmR*vLUG0rwwGUJcTU10B0JH2UnUkEi!AFsCc0!U8A3X03NSJtlu1V!5zo4mamIuEwEuNP7kjNieDaiAdRXiXBYIjpQ*M9YQ7XPFlVZlMIizE7w6XCLpwj3DWbsuF5jgWvPxY87K4GOYCJW1YasZByu*DBYNvAt2cTBoDgc5Dl0WF8avlErUVYSOfGc6WV90tjsPtnL2J1dQGuQdZTdPHacWTsXUu5RHINJ0VpYAC9jPgD975sm2Hle1XhQs4aVs1PWGn2sNIDaD7GoTi!qpNI98KMFyLuJeWvt8AZQTDh7wlv!jTPEWk6O*Pwwj2XB6bHCsLdNjputufdJzXs1SMPPiCnks6VE6*zGd8N8gNx6rZjAAP4IkUZyEyjL0N7L!rqKh4kt79w76DGNo4DI9IW1e45yQ*YF0*ECPVWbg$$";
        private string _accessToken = "EwAwA+l3BAAUWm1xSeJRIJK6txKjBez4GzapzqMAAfEYr8oX8BwdCtOGUFy33r6m8Dsq0w2Dj0LMIU0MGziVaqCAP6cDdOa4mJK7GQ3VQw9INQl5ogyuW06p8iS3qL4IS8CB/VKIIwLgaitsW+WHxC7wp4pGBY0sK6pfCBdKeoxAL6x6WyrUuHB7sTJA4C6c/7NfNl4tzYs4b5BZChhu/DD9UIkaJTlhQDRlMPVUSNQxj7jMx4tNFkh2gB5sJLyg9t740AS5hTz14AWPN4EcPE+p5k037k6Gi7nQvFsR5TvzhBzKFrCDwn5XbLaRRWiO7omuEiUSVHKHsslOHhmtauxP9n3jthTIQoICkibytcf1OT0JtUE25y9VokgoYzwDZgAACMJNYU6cfozdAAJPk/Rs4DL5iOx1zTHRlvyK9dMnBmnZZRDZh3kEJxHwowcmV6JLtRmHAKGpfZ/kGLX3KRU4pfJLnVVIMeOP7uBDHjqmF6fs4rcXHLG9iN2engDo4fjU9A7jpV6bHG89Y9qT0UcM+X0S/LAO/ATMRfwDcbLMAHSTwFrby5EaUkffk6jEMf1sMuL052w3Zspz15VcUoIHjz/iiGv+zWz8KRwBENFbywbAfvZOzvF3hHxZ7ZeTdxUgsxbCoyUquw7wTO0HCIyvWM+IKGEf/ZjrZGzJWvYWU4nIyizHm08ze2ftN2qKCDRHbwSdUOAUO10/iQJUziGCVeemcQSOv6rLlkKoMdzJOzphLFucn5aevV5R4Lr2TG6iibHWGK1GFnPs/98fKwC0E3xT2p0aqhIA07Q7cbXgLAZzquT8WN1ljx7csNwoMJhFUhD0Q6tEKyw3XwTzx2sYFlQtHgCkIB0Iskx+E771l/oCpSv+gegOwDKiy9YbRYVRKtACz37KB1/K+wZmccDXjlvhEpWpi0jPtNfQbwg8gkjDOJ+xn8lDHQ2ICdT03F40N5nhXjOkTQqoT7be2ifL0NDIinwOkODALFpwQn6W2Vcf9gLsCw1bQxmzNVStNUaOAh46HMuMDyhanbkZLdESjG58ReS7CSWPGl2AubkLoGdK75kNfCEYY+r1ODkC";

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