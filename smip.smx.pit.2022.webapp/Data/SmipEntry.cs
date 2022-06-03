using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace smip.smx.pit._2022.webapp.Data
{
    public class SmipEntry
    {

        private Authenticator _authenticator { get; set; }
        public Authenticator Authenticator
        {
            get
            {
                return _authenticator;
            }
            set
            {
                if (_authenticator != value)
                {
                    _authenticator = value;
                    _graphQLHttpClient = null;
                    tokenString = "";
                    jwtToken = null;
                }
            }
        }
        public string tokenString { get; set; }
        public JwtSecurityToken jwtToken { get; set; }

        public SmipEntry()
        {
        }

        private GraphQLHttpClient _graphQLHttpClient { get; set; }
        public GraphQLHttpClient graphQLClient
        {
            get
            {
                if (_graphQLHttpClient == null)
                {
                    _graphQLHttpClient = new GraphQLHttpClient(Authenticator.GraphQlEndpoint, new NewtonsoftJsonSerializer());
                }

                return _graphQLHttpClient;
            }
        }

        public async Task<bool> AuthorizeGraphQLClient()
        {
            if (jwtToken == null)
            {
                if (await RefreshTokenAsync())
                {
                    graphQLClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                }
                else
                {
                    return false;
                }
            }

            if ((jwtToken.ValidTo - DateTime.UtcNow).TotalMinutes < 5)
            {
                if (await RefreshTokenAsync())
                {
                    graphQLClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        public async Task<bool> RefreshTokenAsync()
        {
            try
            {
                var authGraphQLClient = new GraphQLHttpClient(Authenticator.GraphQlEndpoint, new NewtonsoftJsonSerializer());

                // Step 1: request a challenge

                string authRequestQuery = $"mutation authRequest {{authenticationRequest(input: {{authenticator: \"{Authenticator.ClientId}\", role: \"{Authenticator.Role}\", userName: \"{Authenticator.UserName}\"}}) {{jwtRequest {{challenge message}}}}}}";
                GraphQLRequest authRequest = new GraphQLRequest() { Query = authRequestQuery };
                GraphQLResponse<JObject> authResponse = await authGraphQLClient.SendQueryAsync<JObject>(authRequest);
                var aChallenge = authResponse.Data["authenticationRequest"]["jwtRequest"]["challenge"].Value<string>();

                // Step 2: get token

                var authValidationQuery = $"mutation authValidation {{authenticationValidation(input: {{authenticator: \"{Authenticator.ClientId}\", signedChallenge: \"{aChallenge}|{Authenticator.ClientSecret}\"}}) {{    jwtClaim  }}}}";

                GraphQLRequest validationRequest = new GraphQLRequest() { Query = authValidationQuery };
                GraphQLResponse<JObject> validationQLResponse = await authGraphQLClient.SendQueryAsync<JObject>(validationRequest);
                tokenString = validationQLResponse.Data["authenticationValidation"]["jwtClaim"].Value<string>();
                jwtToken = new JwtSecurityToken(tokenString);

                return true;
            }
            catch (Exception e)
            {
                var m = e.Message;
                return false;
            }
        }


        public async Task<T> GetGraphQLDataAsync<T>(string smpQuery, string node)
        {
            if (await AuthorizeGraphQLClient())
            {
                try
                {
                    GraphQLRequest dataRequest = new GraphQLRequest() { Query = smpQuery };
                    var aResponse = await graphQLClient.SendQueryAsync<JObject>(dataRequest);
                    return aResponse.Data[node].ToObject<T>();
                }
                catch (Exception e)
                {
                    var m = e.Message;
                    return default(T);
                }
            }
            else
            {
                return default(T);
            }
        }

        public async Task<GraphQLResponse<JObject>> GetGraphQLDataAsync(string smpQuery)
        {
            if (await AuthorizeGraphQLClient())
            {
                try
                {
                    GraphQLRequest dataRequest = new GraphQLRequest() { Query = smpQuery };
                    var aResponse = await graphQLClient.SendQueryAsync<JObject>(dataRequest);
                    return aResponse;
                }
                catch (Exception e)
                {
                    var m = e.Message;
                    return default(GraphQLResponse<JObject>);
                }
            }
            else
            {
                return default(GraphQLResponse<JObject>);
            }
        }

        public async Task<T> SetGraphQLDataAsync<T>(string smpQuery, string node)
        {
            if (await AuthorizeGraphQLClient())
            {
                try
                {
                    GraphQLRequest dataRequest = new GraphQLRequest() { Query = smpQuery };
                    var aResponse = await graphQLClient.SendMutationAsync<JObject>(dataRequest);
                    return aResponse.Data[node].ToObject<T>();
                }
                catch (Exception e)
                {
                    var m = e.Message;
                    return default(T);
                }
            }
            else
            {
                return default(T);
            }
        }

        public async Task<GraphQLResponse<JObject>> SetGraphQLDataAsync(string smpQuery)
        {
            if (await AuthorizeGraphQLClient())
            {
                try
                {
                    GraphQLRequest dataRequest = new GraphQLRequest() { Query = smpQuery };
                    var aResponse = await graphQLClient.SendMutationAsync<JObject>(dataRequest);
                    return aResponse;
                }
                catch (Exception e)
                {
                    var m = e.Message;
                    return default(GraphQLResponse<JObject>);
                }
            }
            else
            {
                return default(GraphQLResponse<JObject>);
            }
        }

    }


}
