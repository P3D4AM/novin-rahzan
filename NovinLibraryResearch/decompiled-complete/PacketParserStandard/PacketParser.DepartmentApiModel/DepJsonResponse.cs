using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace PacketParser.DepartmentApiModel;

public class DepJsonResponse
{
	[JsonProperty("Result")]
	[JsonPropertyName("Result")]
	public bool Data { get; set; }

	public List<string> Message { get; set; } = new List<string>();

}
