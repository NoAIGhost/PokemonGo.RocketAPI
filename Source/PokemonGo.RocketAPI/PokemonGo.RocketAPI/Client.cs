using AllEnum;
using Google.Protobuf;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Exceptions;
using PokemonGo.RocketAPI.GeneratedCode;
using PokemonGo.RocketAPI.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

// TODO: Implement all Client calls

namespace PokemonGo.RocketAPI
{
    public class Client
    {
        #region Private Fields
        private readonly HttpClient httpClient;
        #endregion

        #region Constructor
        public Client()
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                AllowAutoRedirect = false
            };
            httpClient = new HttpClient(new RetryHandler(handler));
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Niantic App");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");

            httpClient.DefaultRequestHeaders.ExpectContinue = false;
        }
        #endregion

        #region Public Properties
        public string AccessToken { get; set; }
        public AuthType AuthType { get; set; }
        #endregion

        #region Read-only Properties
        public double CurrentLat { get; private set; }
        public double CurrentLng { get; private set; }
        public double CurrentAlt { get; private set; }

        public string ApiUrl { get; private set; } = "";
        public Request.Types.UnknownAuth UnknownAuth { get; private set; }
        public string ApiRpcUrl { get { return $"https://{ApiUrl}/rpc"; } }
        #endregion


        #region Events 

        #endregion

        #region Public methods
        public async Task SetServer()
        {
            var serverRequest = RequestBuilder.GetInitialRequest(AccessToken, AuthType, CurrentLat, CurrentLng, CurrentAlt,
                RequestMethod.GetPlayer, RequestMethod.GetHatchedEggs, RequestMethod.GetInventory, RequestMethod.CheckAwardedBadges, RequestMethod.DownloadSettings);
            var serverResponse = await httpClient.PostProto(Resources.RpcUrl, serverRequest);

            if (serverResponse.Auth == null)
                throw new AccessTokenExpiredException();

            UnknownAuth = new Request.Types.UnknownAuth
            {
                Unknown71 = serverResponse.Auth.Unknown71,
                Timestamp = serverResponse.Auth.Timestamp,
                Unknown73 = serverResponse.Auth.Unknown73
            };

            ApiUrl = serverResponse.ApiUrl;
        }

        public async Task<GetPlayerResponse> GetProfile()
        {
            var request = RequestBuilder.GetInitialRequest(AccessToken, AuthType, CurrentLat, CurrentLng, CurrentAlt, RequestMethod.GetPlayer);
            return await httpClient.PostProtoPayload<Request, GetPlayerResponse>(ApiRpcUrl, request);
        }

        public async Task<GetMapObjectsResponse> GetMapObjects()
        {
            var message = new Request.Types.MapObjectsRequest
            {
                CellIds = ByteString.CopyFrom(ProtoHelper.EncodeUlongList(S2Helper.GetNearbyCellIds(CurrentLng, CurrentLat))),
                Latitude = Utils.FloatAsUlong(CurrentLat),
                Longitude = Utils.FloatAsUlong(CurrentLng),
                Unknown14 = ByteString.CopyFromUtf8("\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0")
            };

            var request = RequestBuilder.GetRequest(UnknownAuth, CurrentLat, CurrentLng, CurrentAlt,
                new Request.Types.Requests { Type = (int)RequestMethod.GetMapObjects, Message = message.ToByteString(), },
                new Request.Types.Requests { Type = (int)RequestMethod.GetHatchedEggs },
                new Request.Types.Requests { Type = (int)RequestMethod.GetInventory, Message = new Request.Types.Time { Time_ = DateTime.UtcNow.ToUnixTime() }.ToByteString() },
                new Request.Types.Requests { Type = (int)RequestMethod.CheckAwardedBadges },
                new Request.Types.Requests { Type = (int)RequestMethod.DownloadSettings, Message = new Request.Types.SettingsGuid() { Guid = ByteString.CopyFromUtf8("4a2e9bc330dae60e7b74fc85b98868ab4700802e") }.ToByteString() }
                );

            return await httpClient.PostProtoPayload<Request, GetMapObjectsResponse>(ApiRpcUrl, request);
        }
        #endregion

        #region Private methods

        #endregion
    }
}
