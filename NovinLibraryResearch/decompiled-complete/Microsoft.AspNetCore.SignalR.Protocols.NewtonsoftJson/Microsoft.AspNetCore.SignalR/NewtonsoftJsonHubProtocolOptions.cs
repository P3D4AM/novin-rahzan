using Microsoft.AspNetCore.SignalR.Protocol;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.SignalR;

public class NewtonsoftJsonHubProtocolOptions
{
	public JsonSerializerSettings PayloadSerializerSettings { get; set; } = NewtonsoftJsonHubProtocol.CreateDefaultSerializerSettings();

}
