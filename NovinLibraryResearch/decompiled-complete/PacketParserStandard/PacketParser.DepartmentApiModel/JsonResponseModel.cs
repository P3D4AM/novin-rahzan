using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace PacketParser.DepartmentApiModel;

public class JsonResponseModel
{
	public string Message { get; set; }

	[JsonProperty("Result")]
	[JsonPropertyName("Result")]
	public object Data { get; set; }
}
