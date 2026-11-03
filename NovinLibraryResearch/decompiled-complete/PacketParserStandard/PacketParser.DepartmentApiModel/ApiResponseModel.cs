using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace PacketParser.DepartmentApiModel;

public class ApiResponseModel<T>
{
	public string Message { get; set; }

	[JsonProperty("Result")]
	[JsonPropertyName("Result")]
	public T Data { get; set; }
}
