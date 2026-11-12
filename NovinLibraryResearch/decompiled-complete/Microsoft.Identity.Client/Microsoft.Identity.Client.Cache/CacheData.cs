using System;

namespace Microsoft.Identity.Client.Cache;

[Obsolete("This is expected to be removed in MSAL.NET v3 and ADAL.NET v5. We recommend using SerializeMsalV3/DeserializeMsalV3. Read more: https://aka.ms/msal-net-3x-cache-breaking-change", false)]
public class CacheData
{
	public byte[] AdalV3State { get; set; }

	public byte[] UnifiedState { get; set; }
}
