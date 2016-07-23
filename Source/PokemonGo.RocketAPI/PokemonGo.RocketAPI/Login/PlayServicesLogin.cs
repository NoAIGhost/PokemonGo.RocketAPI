using PokemonGo.RocketAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Login
{
    public class PlayServicesLogin : ILoginProvider
    {
        // TODO: Implement GPSOAuth login
        public string Username { get; set; }
        public string Password { get; set; }
        public string Service { get; set; } // TODO:  Set default value
        public string AppName { get; set; } // TODO: Set default value
        public string ClientSignature { get; set; } // TODO: Set default value
        public string DeviceCountry { get; set; } = "us";
        public string OperatorCountry { get; set; } = "us";
        public string Language { get; set; } = "en";
        public int SdkVersion { get; set; } = 21;

        public string Token { get; private set; }

        public Dictionary<string,string> MasterResponse { get; private set; }
        public Dictionary<string,string> OAuthResponse { get; private set; }

        


        public async Task<TokenHolder> Authorize()
        {
            if (string.IsNullOrEmpty(Username))
                throw new Exception("No username");

            if (string.IsNullOrEmpty(Password))
                throw new Exception("No password");

            var client = new GPSOAuthClient(Username, Password);
            MasterResponse = await client.PerformMasterLogin();

            if(MasterResponse.ContainsKey("Token"))
            {
                Token = MasterResponse["Token"];
                OAuthResponse = await client.PerformOAuth(Token, Service, AppName, ClientSignature, DeviceCountry, OperatorCountry, Language, SdkVersion);

                // TODO: Get actual name of tokens!!!
                return new TokenHolder { AccessToken = OAuthResponse["Token"] };
            }

            return null;
        }
    }
}
