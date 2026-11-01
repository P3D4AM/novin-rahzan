using Newtonsoft.Json;

namespace TimeAttendanceDAL.Settings;

public class UpdateDetailsClass
{
	[JsonProperty("a1")]
	public bool IsPrsnTafsilUsedUpdate { get; set; }

	[JsonProperty("a2")]
	public bool IsTaxTableUpdated { get; set; }
}
