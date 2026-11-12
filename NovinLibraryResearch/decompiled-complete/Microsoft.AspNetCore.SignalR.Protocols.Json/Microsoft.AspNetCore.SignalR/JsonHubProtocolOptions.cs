using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace Microsoft.AspNetCore.SignalR;

public class JsonHubProtocolOptions
{
	public JsonSerializerOptions PayloadSerializerOptions { get; set; } = JsonHubProtocol.CreateDefaultSerializerSettings();

}
