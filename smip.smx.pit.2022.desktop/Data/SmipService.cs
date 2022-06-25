using GraphQL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using smip.sdk;
using smip.sdk.SmipModel;

namespace smip.smx.pit._2022.webapp.Data
{
    public class SmipService
    {
        SmipEntry _smipEntry { get; set; }
        IConfiguration configuration;
        public SmipService(IConfiguration config)
        {
            configuration = config;
            _smipEntry = new SmipEntry();
            _smipEntry.Authenticator = configuration.GetRequiredSection("SMIP").Get<Authenticator>();
        }
        public async Task<GraphQLResponse<JObject>> GetLibrariesAsync()
        {
            var aQuery = "{ libraries { id displayName } }";
            return await _smipEntry.GetGraphQLDataAsync(aQuery);
        }

        public async Task<SmipEquipment> GetCountersTs(DateTimeOffset startTime, DateTimeOffset endTime)
        {
            var aQuery = $@"
                query q1 {{
                    equipments(condition: {{ displayName: ""Amatrol Station 3"" }}) {{
                        id
                        displayName
                        attributes(
                            filter: {{
                                displayName: {{ in: [""Count White"", ""Count Black"", ""Count Aluminum""] }}
                            }}
                        ) 
                        {{
                            id
                            displayName
                            getTimeSeries(
                                startTime: ""{startTime.UtcDateTime.ToString("o")}"", 
                                endTime: ""{endTime.UtcDateTime.ToString("o")}"",
                                filter: {{intvalue: {{ greaterThanOrEqualTo: ""0"" }} }}
                            ) {{
                                ts
                                intvalue
                            }}
                        }}
                    }}
                }}
            ";
            var aResponse = await _smipEntry.GetGraphQLDataAsync<List<SmipEquipment>>(aQuery, "equipments");
            return aResponse.First();
        }

    }
}
