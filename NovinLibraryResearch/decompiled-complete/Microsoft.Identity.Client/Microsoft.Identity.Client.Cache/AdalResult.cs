using Microsoft.Identity.Json;

namespace Microsoft.Identity.Client.Cache;

[JsonObject]
[Preserve(AllMembers = true)]
internal sealed class AdalResult
{
	[JsonProperty]
	public AdalUserInfo UserInfo { get; internal set; }

	internal AdalResult()
	{
	}
}
